//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Text;
using Xarial.XCad.Sw.MacroFeature;

namespace Xarial.XCad.Sw
{
    public class SwMacroFeature : SwFeature, IXCustomFeature
    {
        private readonly MacroFeatureParametersParser m_ParamsParser;
        private readonly IModelDoc2 m_Model;

        internal SwMacroFeature(IModelDoc2 model, IFeature feat) 
            : base(model, feat)
        {
            m_ParamsParser = new MacroFeatureParametersParser();
            m_Model = model;
        }

        public TParams GetParameters<TParams>()
            where TParams : class, new()
        {
            return m_ParamsParser.GetParameters<TParams>(Feature,
                Feature.GetDefinition() as IMacroFeatureData,
                m_Model, out IDisplayDimension[] _, out string[] _, out IBody2[] _, out Enums.CustomFeatureOutdateState_e _);
        }

        public void SetParameters<TParams>(TParams param)
            where TParams : class, new()
        {
            m_ParamsParser.SetParameters(m_Model, Feature, Feature.GetDefinition() as IMacroFeatureData, 
                param, out Enums.CustomFeatureOutdateState_e _);
        }
    }
}
