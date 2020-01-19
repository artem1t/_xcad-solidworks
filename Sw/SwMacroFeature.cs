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
using Xarial.XCad.Structures;
using Xarial.XCad.Sw.MacroFeature;

namespace Xarial.XCad.Sw
{
    public class SwMacroFeature : SwFeature, IXCustomFeature
    {   
        protected readonly SwDocument m_Model;

        private IMacroFeatureData m_FeatData;

        public IMacroFeatureData FeatureData => m_FeatData ?? (m_FeatData = Feature.GetDefinition() as IMacroFeatureData);

        internal SwMacroFeature(SwDocument model, IFeature feat) 
            : base(model.Model, feat)
        {
            m_Model = model;
        }

        //TODO: check constant context disconnection exception
        public IXConfiguration Configuration => new SwConfiguration((Feature.GetDefinition() as IMacroFeatureData).CurrentConfiguration);
        
        public SwMacroFeature<TParams> ToParameters<TParams>()
            where TParams : class, new()
        {
            return ToParameters<TParams>(new MacroFeatureParametersParser());
        }

        internal SwMacroFeature<TParams> ToParameters<TParams>(MacroFeatureParametersParser paramsParser)
            where TParams : class, new()
        {
            return new SwMacroFeature<TParams>(m_Model, Feature, paramsParser);
        }
    }

    public class SwMacroFeature<TParams> : SwMacroFeature, IXCustomFeature<TParams>
        where TParams : class, new()
    {
        private readonly MacroFeatureParametersParser m_ParamsParser;

        internal SwMacroFeature(SwDocument model, IFeature feat, MacroFeatureParametersParser paramsParser) 
            : base(model, feat)
        {
            m_ParamsParser = paramsParser;
        }

        public TParams GetParameters()
        {
            return (TParams)m_ParamsParser.GetParameters(this, m_Model, typeof(TParams),
                out IXDimension[] _, out string[] _, out IXBody[] _, out IXSelObject[] sels, out Enums.CustomFeatureOutdateState_e _);
        }

        public void SetParameters(TParams param)
        {
            m_ParamsParser.SetParameters(m_Model, this, param, out Enums.CustomFeatureOutdateState_e _);
            //TODO: call modify deinition
        }
    }
}
