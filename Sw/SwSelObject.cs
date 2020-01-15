//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Xarial.XCad.Sw
{
    public class SwSelObject : IXSelObject
    {
        public object Dispatch { get; }

        private readonly IModelDoc2 m_Model;

        internal SwSelObject(IModelDoc2 model, object disp) 
        {
            m_Model = model;
            Dispatch = disp;
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
