//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Text;
using Xarial.XCad.Features;
using Xarial.XCad.Services;

namespace Xarial.XCad.Sw.Features
{
    public class SwFeature : SwSelObject, IXFeature
    {
        private readonly ElementCreator<IFeature> m_Creator;

        public IFeature Feature 
        {
            get 
            {
                return m_Creator.Element;
            }
        }

        internal bool IsCreated 
        {
            get 
            {
                return m_Creator.IsCreated;
            }
        }

        internal SwFeature(IModelDoc2 model, IFeature feat, bool created) : base(model, feat)
        {
            m_Creator = new ElementCreator<IFeature>(CreateFeature, feat, created);
        }

        internal void Create()
        {
            m_Creator.Create();
        }

        protected virtual IFeature CreateFeature()
        {
            throw new NotSupportedException("Creation of this feature is not supported");
        }

        public override void Select(bool append)
        {
            if (!Feature.Select2(append, 0)) 
            {
                throw new Exception("Faile to select feature");
            }
        }
    }
}
