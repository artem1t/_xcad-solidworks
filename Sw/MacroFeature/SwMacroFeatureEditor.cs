//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using Xarial.XCad.Delegates;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.Utils.CustomFeature;

namespace Xarial.XCad.Sw.MacroFeature
{
    public class SwMacroFeatureEditor<TCustomFeatureDef, TData, TPage> : BaseCustomFeatureEditor<TCustomFeatureDef, TData, TPage>
        where TCustomFeatureDef : class, IXCustomFeatureDefinition<TData>, new()
        where TData : class, new()
        where TPage : class, new()
    {
        internal SwMacroFeatureEditor(IXApplication app, IXExtension ext, CustomFeatureParametersParser paramsParser,
            DataConverterDelegate<TPage, TData> pageToDataConv, DataConverterDelegate<TData, TPage> dataToPageConv, 
            CreateGeometryDelegate<TData> geomCreator) : base(app, ext, paramsParser, pageToDataConv, dataToPageConv, geomCreator)
        {
        }

        protected override void DisplayPreview(IXBody[] bodies)
        {
            foreach (var body in bodies)
            {
                var swBody = (body as SwBody).Body;
                var model = (CurModel as SwDocument).Model;

                swBody.Display3(model, ConvertColor(Color.Yellow),
                    (int)swTempBodySelectOptions_e.swTempBodySelectOptionNone);
            }
        }

        private int ConvertColor(Color color)
        {
            return (color.R << 0) | (color.G << 8) | (color.B << 16);
        }

        protected override void HidePreview(IXBody[] bodies)
        {
            if (bodies != null)
            {
                for (int i = 0; i < bodies.Length; i++)
                {
                    Marshal.ReleaseComObject(bodies[i]);
                    bodies[i] = null;
                }
            }

            //TODO: check if this could be removed as it is causing flickering
            GC.Collect();
        }
    }
}
