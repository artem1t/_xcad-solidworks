//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Xarial.XCad.Attributes;
using Xarial.XCad.Enums;
using Xarial.XCad.Structures;
using Xarial.XCad.Sw.MacroFeature;
using Xarial.XCad.Sw.MacroFeature.Icons;
using Xarial.XCad.Sw.Utils;
using Xarial.XCad.Utils.Diagnostics;
using Xarial.XCad.Utils.Reflection;

namespace Xarial.XCad.Sw
{
    public abstract class SwMacroFeatureDefinition : IXCustomFeatureDefinition, ISwComFeature
    {
        static SwApplication m_Application;

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
        private readonly MacroFeatureParametersParser m_ParamsParser;

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
            m_ParamsParser = new MacroFeatureParametersParser();
            TryCreateIcons();
        }

        private void TryCreateIcons()
        {
            var iconsConverter = new IconsConverter(
                MacroFeatureIconInfo.GetLocation(this.GetType()), false);

            Image icon = null;

            this.GetType().TryGetAttribute<IconAttribute>(a =>
            {
                icon = a.Icon;
            });

            if (icon == null)
            {
                //TODO: add default icon
                //icon = Resources.default_icon;
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

        #endregion

        #region Overrides

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public object Edit(object app, object modelDoc, object feature)
        {
            LogOperation("Editing feature", app as ISldWorks, modelDoc as IModelDoc2, feature as IFeature);

            var doc = Application.SwDocuments[modelDoc as IModelDoc2];
            return OnEditDefinition(Application, doc, new SwMacroFeature(doc, feature as IFeature, m_ParamsParser));
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public object Regenerate(object app, object modelDoc, object feature)
        {
            LogOperation("Regenerating feature", app as ISldWorks, modelDoc as IModelDoc2, feature as IFeature);

            SetProvider(app as ISldWorks, feature as IFeature);

            var doc = Application.SwDocuments[modelDoc as IModelDoc2];

            var res = OnRebuild(Application, doc, new SwMacroFeature(doc, feature as IFeature, m_ParamsParser));

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
            return OnUpdateState(Application, doc, new SwMacroFeature(doc, feature as IFeature, m_ParamsParser));
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

        #endregion

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

                    foreach(var body in bodies)
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
}
