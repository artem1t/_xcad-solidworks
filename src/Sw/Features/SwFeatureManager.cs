﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://xcad.xarial.com/license/
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections;
using System.Collections.Generic;
using Xarial.XCad.Features;
using Xarial.XCad.Features.CustomFeature;
using Xarial.XCad.SolidWorks.Documents;
using Xarial.XCad.SolidWorks.Features.CustomFeature;
using Xarial.XCad.SolidWorks.Features.CustomFeature.Toolkit;

namespace Xarial.XCad.SolidWorks.Features
{
    public class SwFeatureManager : IXFeatureRepository
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

        public void RemoveRange(IEnumerable<IXFeature> ents)
        {
            //TODO: implement deletion
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