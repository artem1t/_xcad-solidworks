﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://xcad.xarial.com/license/
//*********************************************************************

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xarial.XCad.Annotations;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.Documents;
using Xarial.XCad.Features.CustomFeature;
using Xarial.XCad.Features.CustomFeature.Delegates;
using Xarial.XCad.Features.CustomFeature.Enums;
using Xarial.XCad.Features.CustomFeature.Structures;
using Xarial.XCad.Geometry;
using Xarial.XCad.Geometry.Structures;
using Xarial.XCad.Reflection;
using Xarial.XCad.SolidWorks.Annotations;
using Xarial.XCad.SolidWorks.Enums;
using Xarial.XCad.SolidWorks.Features.CustomFeature.Toolkit;
using Xarial.XCad.SolidWorks.Features.CustomFeature.Toolkit.Icons;
using Xarial.XCad.SolidWorks.Geometry;
using Xarial.XCad.SolidWorks.Utils;
using Xarial.XCad.Utils.Diagnostics;
using Xarial.XCad.Utils.Reflection;

namespace Xarial.XCad.SolidWorks.Features.CustomFeature
{
    public abstract class SwMacroFeatureDefinition : IXCustomFeatureDefinition, ISwComFeature
    {
        private static SwApplication m_Application;

        internal static SwApplication Application
        {
            get
            {
                if (m_Application == null)
                {
                    //TODO: extract application from current process
                }

                return m_Application;
            }
            set
            {
                m_Application = value;
            }
        }

        #region Initiation

        private readonly string m_Provider;
        private readonly ILogger m_Logger;

        public ILogger Logger
        {
            get
            {
                return m_Logger;
            }
        }

        public SwMacroFeatureDefinition()
        {
            //TODO: implement provider
            string provider = "";
            //this.GetType().TryGetAttribute<OptionsAttribute>(a =>
            //{
            //    provider = a.Provider;
            //});

            m_Provider = provider;
            m_Logger = new TraceLogger("xCad.MacroFeature");
            TryCreateIcons();
        }

        private void TryCreateIcons()
        {
            var iconsConverter = new IconsConverter(
                MacroFeatureIconInfo.GetLocation(this.GetType()), false);

            System.Drawing.Image icon = null;

            this.GetType().TryGetAttribute<IconAttribute>(a =>
            {
                icon = a.Icon;
            });

            if (icon == null)
            {
                icon = Defaults.Icon;
            }

            //TODO: create different icons for highlighted and suppressed
            var regular = icon;
            var highlighted = icon;
            var suppressed = icon;

            //Creation of icons may fail if user doesn't have write permissions or icon is locked
            try
            {
                iconsConverter.ConvertIcon(new MacroFeatureIcon(icon, MacroFeatureIconInfo.RegularName));
                iconsConverter.ConvertIcon(new MacroFeatureIcon(highlighted, MacroFeatureIconInfo.HighlightedName));
                iconsConverter.ConvertIcon(new MacroFeatureIcon(suppressed, MacroFeatureIconInfo.SuppressedName));
                iconsConverter.ConvertIcon(new MacroFeatureHighResIcon(icon, MacroFeatureIconInfo.RegularName));
                iconsConverter.ConvertIcon(new MacroFeatureHighResIcon(highlighted, MacroFeatureIconInfo.HighlightedName));
                iconsConverter.ConvertIcon(new MacroFeatureHighResIcon(suppressed, MacroFeatureIconInfo.SuppressedName));
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        #endregion Initiation

        #region Overrides

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public object Edit(object app, object modelDoc, object feature)
        {
            LogOperation("Editing feature", app as ISldWorks, modelDoc as IModelDoc2, feature as IFeature);

            var doc = Application.SwDocuments[modelDoc as IModelDoc2];
            return OnEditDefinition(Application, doc, new SwMacroFeature(doc, (modelDoc as IModelDoc2).FeatureManager, feature as IFeature, true));
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public object Regenerate(object app, object modelDoc, object feature)
        {
            LogOperation("Regenerating feature", app as ISldWorks, modelDoc as IModelDoc2, feature as IFeature);

            SetProvider(app as ISldWorks, feature as IFeature);

            var doc = Application.SwDocuments[modelDoc as IModelDoc2];

            var res = OnRebuild(Application, doc, new SwMacroFeature(doc, (modelDoc as IModelDoc2).FeatureManager, feature as IFeature, true));

            if (res != null)
            {
                return ParseMacroFeatureResult(res, app as ISldWorks, (feature as IFeature).GetDefinition() as IMacroFeatureData);
            }
            else
            {
                return null;
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public object Security(object app, object modelDoc, object feature)
        {
            var doc = Application.SwDocuments[modelDoc as IModelDoc2];
            return OnUpdateState(Application, doc, new SwMacroFeature(doc, (modelDoc as IModelDoc2).FeatureManager, feature as IFeature, true));
        }

        private void SetProvider(ISldWorks app, IFeature feature)
        {
            if (!string.IsNullOrEmpty(m_Provider))
            {
                if (app.IsVersionNewerOrEqual(SwVersion_e.Sw2016))
                {
                    var featData = feature.GetDefinition() as IMacroFeatureData;

                    if (featData.Provider != m_Provider)
                    {
                        featData.Provider = m_Provider;
                    }
                }
            }
        }

        private void LogOperation(string operName, ISldWorks app, IModelDoc2 modelDoc, IFeature feature)
        {
            Logger.Log($"{operName}: {feature?.Name} in {modelDoc?.GetTitle()} of SOLIDWORKS session: {app?.GetProcessID()}");
        }

        #endregion Overrides

        public virtual bool OnEditDefinition(IXApplication app, IXDocument model, IXCustomFeature feature)
        {
            return true;
        }

        public virtual CustomFeatureRebuildResult OnRebuild(IXApplication app, IXDocument model, IXCustomFeature feature)
        {
            return null;
        }

        public virtual CustomFeatureState_e OnUpdateState(IXApplication app, IXDocument model, IXCustomFeature feature)
        {
            return CustomFeatureState_e.Default;
        }

        private object ParseMacroFeatureResult(CustomFeatureRebuildResult res, ISldWorks app, IMacroFeatureData featData)
        {
            switch (res)
            {
                case CustomFeatureBodyRebuildResult bodyRes:
                    //TODO: validate if any non SwBody in the array
                    //TODO: get updateEntityIds from the parameters
                    return GetBodyResult(app, bodyRes.Bodies?.OfType<SwBody>().Select(b => b.Body), featData, true);

                default:
                    return GetStatusResult(res.Result, res.ErrorMessage);
            }
        }

        private object GetStatusResult(bool status, string error = "")
        {
            if (status)
            {
                return status;
            }
            else
            {
                if (!string.IsNullOrEmpty(error))
                {
                    return error;
                }
                else
                {
                    return status;
                }
            }
        }

        private static object GetBodyResult(ISldWorks app, IEnumerable<IBody2> bodies, IMacroFeatureData featData, bool updateEntityIds)
        {
            if (bodies != null)
            {
                if (CompatibilityUtils.IsVersionNewerOrEqual(app, SwVersion_e.Sw2013, 5))
                {
                    featData.EnableMultiBodyConsume = true;
                }

                if (updateEntityIds)
                {
                    if (featData == null)
                    {
                        throw new ArgumentNullException(nameof(featData));
                    }

                    foreach (var body in bodies)
                    {
                        object faces;
                        object edges;
                        featData.GetEntitiesNeedUserId(body, out faces, out edges);

                        if (faces is object[])
                        {
                            int nextId = 0;

                            foreach (Face2 face in faces as object[])
                            {
                                featData.SetFaceUserId(face, nextId++, 0);
                            }
                        }

                        if (edges is object[])
                        {
                            int nextId = 0;

                            foreach (Edge edge in edges as object[])
                            {
                                featData.SetEdgeUserId(edge, nextId++, 0);
                            }
                        }
                    }
                }

                if (bodies.Count() == 1)
                {
                    return bodies.First();
                }
                else
                {
                    return bodies;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(bodies));
            }
        }
    }

    public abstract class SwMacroFeatureDefinition<TParams> : SwMacroFeatureDefinition, IXCustomFeatureDefinition<TParams>
        where TParams : class, new()
    {
        private readonly MacroFeatureParametersParser m_ParamsParser;

        public SwMacroFeatureDefinition() : this(new MacroFeatureParametersParser())
        {
        }

        internal SwMacroFeatureDefinition(MacroFeatureParametersParser paramsParser) : base()
        {
            m_ParamsParser = paramsParser;
        }

        public void AlignDimension(IXDimension dim, Point[] pts, Vector dir, Vector extDir)
        {
            if (pts != null)
            {
                if (pts.Length == 2)
                {
                    var newPts = new Point[3]
                    {
                        pts[0],
                        pts[1],
                        new Point(0, 0, 0)//3 points required for SOLIDWORKS even if not used
                    };
                }
            }

            var refPts = pts.Select(p => m_ParamsParser.MathUtils.CreatePoint(p.ToArray()) as IMathPoint).ToArray();

            if (dir != null)
            {
                var dimDirVec = m_ParamsParser.MathUtils.CreateVector(dir.ToArray()) as MathVector;
                ((SwDimension)dim).Dimension.DimensionLineDirection = dimDirVec;
            }

            if (extDir != null)
            {
                var extDirVec = m_ParamsParser.MathUtils.CreateVector(extDir.ToArray()) as MathVector;
                ((SwDimension)dim).Dimension.ExtensionLineDirection = extDirVec;
            }

            ((SwDimension)dim).Dimension.ReferencePoints = refPts;
        }

        public abstract CustomFeatureRebuildResult OnRebuild(IXApplication app, IXDocument model, IXCustomFeature feature,
            TParams parameters, out AlignDimensionDelegate<TParams> alignDim);

        public override CustomFeatureRebuildResult OnRebuild(IXApplication app, IXDocument model, IXCustomFeature feature)
        {
            IXDimension[] dims;
            string[] dimParamNames;
            var param = (TParams)m_ParamsParser.GetParameters(feature, model, typeof(TParams), out dims, out dimParamNames,
                out IXBody[] _, out IXSelObject[] _, out CustomFeatureOutdateState_e _);

            AlignDimensionDelegate<TParams> alignDimsDel;
            var res = OnRebuild(app, model, feature, param, out alignDimsDel);

            m_ParamsParser.SetParameters(model, feature, param, out CustomFeatureOutdateState_e _);

            if (dims?.Any() == true)
            {
                if (alignDimsDel != null)
                {
                    for (int i = 0; i < dims.Length; i++)
                    {
                        alignDimsDel.Invoke(this, dimParamNames[i], dims[i]);

                        //IMPORTANT: need to dispose otherwise SW will crash once document is closed
                        ((IDisposable)dims[i]).Dispose();
                    }
                }
            }

            return res;
        }
    }

    public abstract class SwMacroFeatureDefinition<TParams, TPage> : SwMacroFeatureDefinition<TParams>
        where TParams : class, new()
        where TPage : class, new()
    {
        private readonly MacroFeatureParametersParser m_ParamsParser;

        public SwMacroFeatureDefinition() : this(new MacroFeatureParametersParser())
        {
        }

        internal SwMacroFeatureDefinition(MacroFeatureParametersParser parser) : base(parser)
        {
            m_ParamsParser = parser;
        }

        public abstract IXCustomFeatureEditor<TParams, TPage> Editor { get; }

        public override bool OnEditDefinition(IXApplication app, IXDocument model, IXCustomFeature feature)
        {
            Editor.Edit(model, ((SwMacroFeature)feature).ToParameters<TParams>(m_ParamsParser));
            return true;
        }

        public override CustomFeatureRebuildResult OnRebuild(IXApplication app, IXDocument model,
            IXCustomFeature feature, TParams parameters, out AlignDimensionDelegate<TParams> alignDim)
        {
            alignDim = null;

            return new CustomFeatureBodyRebuildResult()
            {
                Bodies = Editor.CreateGeometry(this, parameters, false, out alignDim)
            };
        }
    }
}