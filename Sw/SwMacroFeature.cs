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
        private readonly SwDocument m_Model;

        private IMacroFeatureData m_FeatData;

        public IMacroFeatureData FeatureData => m_FeatData ?? (m_FeatData = Feature.GetDefinition() as IMacroFeatureData);

        internal SwMacroFeature(SwDocument model, IFeature feat, MacroFeatureParametersParser paramsParser) 
            : base(model.Model, feat)
        {
            m_ParamsParser = paramsParser;
            m_Model = model;
        }

        public IXConfiguration Configuration => new SwConfiguration((Feature.GetDefinition() as IMacroFeatureData).CurrentConfiguration);

        public TParams GetParameters<TParams>()
            where TParams : class, new()
        {
            //TODO: resolve model
            return (TParams)m_ParamsParser.GetParameters(this, m_Model, typeof(TParams),
                out IXDimension[] _, out string[] _, out IXBody[] _, out IXSelObject[] sels, out Enums.CustomFeatureOutdateState_e _);
        }

        public void SetParameters<TParams>(TParams param)
            where TParams : class, new()
        {
            //TODO: resolve model
            m_ParamsParser.SetParameters(m_Model, this, param, out Enums.CustomFeatureOutdateState_e _);
        }
    }
}
