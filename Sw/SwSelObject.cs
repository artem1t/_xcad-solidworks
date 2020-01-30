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
    public class SwSelObject : IXSelObject
    {
        public static SwSelObject FromDispatch(object disp, IModelDoc2 model = null) 
        {
            switch (disp)
            {
                //TODO: make this automatic
                case IEdge edge:
                    return new SwEdge(edge);
                case IFeature feat:
                    return new SwFeature(model, feat, true);
                case IBody2 body:
                    return new SwBody(body);
                case IDisplayDimension dispDim:
                    return new SwDimension(dispDim);
                default:
                    return new SwSelObject(model, disp);
            }
        }

        public virtual object Dispatch { get; }

        protected readonly IModelDoc2 m_Model;

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

        public virtual bool IsSame(IXObject other)
        {
            if (other is SwSelObject)
            {
                return Dispatch == (other as SwSelObject).Dispatch;
            }
            else 
            {
                return false;
            }
        }
    }
}
