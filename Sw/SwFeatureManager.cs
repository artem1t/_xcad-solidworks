//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xarial.XCad.Attributes;
using Xarial.XCad.Enums;
using Xarial.XCad.Sw.MacroFeature;
using Xarial.XCad.Sw.Utils;
using Xarial.XCad.Utils.CustomFeature;
using Xarial.XCad.Utils.Reflection;

namespace Xarial.XCad.Sw
{
    public class SwFeatureManager : IXFeatureManager
    {
        private readonly IFeatureManager m_FeatMgr;
        private readonly MacroFeatureParametersParser m_ParamsParser;
        private readonly SwDocument m_Model;

        internal SwFeatureManager(SwDocument model, IFeatureManager featMgr) 
        {
            m_Model = model;
            m_ParamsParser = new MacroFeatureParametersParser();
            m_FeatMgr = featMgr;
        }

        public IXCustomFeature CreateCustomFeature<TDef, TParams>(TParams param)
            where TDef : class, IXCustomFeatureDefinition, new()
            where TParams: class, new()
        {
            var feat = InsertComFeatureWithParameters(typeof(TDef), param);

            if (feat == null)
            {
                throw new Exception();
            }

            return new SwMacroFeature<TParams>(m_Model, feat, m_ParamsParser);
        }

        private IFeature InsertComFeatureWithParameters(Type macroFeatType, object parameters)
        {
            CustomFeatureParameter[] atts;
            IXSelObject[] selection;
            CustomFeatureDimensionType_e[] dimTypes;
            double[] dimValues;
            IXBody[] editBodies;

            m_ParamsParser.Parse(parameters,
                out atts, out selection, out dimTypes, out dimValues,
                out editBodies);

            string[] paramNames;
            string[] paramValues;
            int[] paramTypes;

            m_ParamsParser.ConvertParameters(atts, out paramNames, out paramTypes, out paramValues);

            //TODO: add dim types conversion

            return InsertComFeatureBase(macroFeatType,
                paramNames, paramTypes, paramValues,
                dimTypes?.Select(d => (int)d)?.ToArray(), dimValues,
                selection?.Cast<SwSelObject>()?.Select(s => s.Dispatch)?.ToArray(),
                editBodies?.Cast<SwBody>()?.Select(b => b.Body)?.ToArray());
        }

        private IFeature InsertComFeatureBase(Type macroFeatType,
            string[] paramNames, int[] paramTypes, string[] paramValues,
            int[] dimTypes, double[] dimValues, object[] selection, object[] editBodies)
        {
            if (!typeof(SwMacroFeatureDefinition).IsAssignableFrom(macroFeatType))
            {
                throw new InvalidCastException($"{macroFeatType.FullName} must inherit {typeof(SwMacroFeatureDefinition).FullName}");
            }

            var options = CustomFeatureOptions_e.Default;
            var provider = "";

            macroFeatType.TryGetAttribute<CustomFeatureOptionsAttribute>(a =>
            {
                options = a.Flags;
                provider = a.Provider;
            });

            var baseName = MacroFeatureInfo.GetBaseName(macroFeatType);

            var progId = MacroFeatureInfo.GetProgId(macroFeatType);

            if (string.IsNullOrEmpty(progId))
            {
                throw new NullReferenceException("Prog id for macro feature cannot be extracted");
            }

            var icons = MacroFeatureIconInfo.GetIcons(macroFeatType,
                CompatibilityUtils.SupportsHighResIcons(SwMacroFeatureDefinition.Application.Application, CompatibilityUtils.HighResIconsScope_e.MacroFeature));

            using (var selSet = new SelectionGroup(m_FeatMgr.Document.ISelectionManager))
            {
                if (selection != null && selection.Any())
                {
                    var selRes = selSet.AddRange(selection);

                    Debug.Assert(selRes);
                }

                var feat = m_FeatMgr.InsertMacroFeature3(baseName,
                    progId, null, paramNames, paramTypes,
                    paramValues, dimTypes, dimValues, editBodies, icons, (int)options) as IFeature;

                return feat;
            }
        }
    }
}
