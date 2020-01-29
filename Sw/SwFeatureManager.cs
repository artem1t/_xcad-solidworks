//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections;
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
    public class SwFeatureManager : IXFeatureCollection
    {
        private readonly IFeatureManager m_FeatMgr;
        private readonly MacroFeatureParametersParser m_ParamsParser;
        private readonly SwDocument m_Model;

        public int Count => m_FeatMgr.GetFeatureCount(false);

        internal SwFeatureManager(SwDocument model, IFeatureManager featMgr, ISldWorks app) 
        {
            m_Model = model;
            m_ParamsParser = new MacroFeatureParametersParser(app);
            m_FeatMgr = featMgr;
        }
                
        public void AddRange(IEnumerable<IXFeature> feats)
        {
            if (feats == null) 
            {
                throw new ArgumentNullException(nameof(feats));
            }

            foreach (SwFeature feat in feats) 
            {
                feat.Create();
            }
        }

        public IXSketch2D New2DSketch()
        {
            return new SwSketch2D(m_Model.Model, null, false);
        }

        public IXSketch3D New3DSketch()
        {
            return new SwSketch3D(m_Model.Model, null, false);
        }

        public IEnumerator<IXFeature> GetEnumerator()
        {
            return new FeatureEnumerator(m_Model.Model);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IXCustomFeature<TParams> NewCustomFeature<TParams>() 
            where TParams : class, new()
        {
            return new SwMacroFeature<TParams>(m_Model, m_FeatMgr, null, m_ParamsParser, false);
        }
    }

    internal class FeatureEnumerator : IEnumerator<IXFeature>
    {
        public IXFeature Current => new SwFeature(m_Model, m_CurFeat, true);

        object IEnumerator.Current => Current;

        private readonly IModelDoc2 m_Model;
        private IFeature m_CurFeat;

        //TODO: implement proper handling of sub features

        internal FeatureEnumerator(IModelDoc2 model) 
        {
            m_Model = model;
            Reset();
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            m_CurFeat = m_CurFeat.IGetNextFeature();
            return m_CurFeat != null;
        }

        public void Reset()
        {
            m_CurFeat = m_Model.IFirstFeature();
        }
    }
}
