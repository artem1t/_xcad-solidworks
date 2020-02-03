//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xarial.XCad.Sw.Annotations;
using Xarial.XCad.Sw.Features;
using Xarial.XCad.Sw.Geometry;

namespace Xarial.XCad.Sw
{
    public class SwSelObject : SwObject, IXSelObject
    {
        protected readonly IModelDoc2 m_Model;

        internal SwSelObject(IModelDoc2 model, object disp) : base(disp)
        {
            m_Model = model;
        }

        public virtual void Select(bool append)
        {
            if (m_Model.Extension.MultiSelect2(new DispatchWrapper[] { new DispatchWrapper(Dispatch) }, append, null) != 1) 
            {
                throw new Exception("Failed to select");
            }
        }
    }
}
