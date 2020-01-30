using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Text;
using Xarial.XCad.Sw.Annotations;
using Xarial.XCad.Sw.Features;
using Xarial.XCad.Sw.Geometry;

namespace Xarial.XCad.Sw
{
    public class SwObject : IXObject
    {
        public static SwObject FromDispatch(object disp, IModelDoc2 model = null)
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
                    return new SwObject(disp);
            }
        }

        public virtual object Dispatch { get; }

        internal SwObject(object dispatch) 
        {
            Dispatch = dispatch;
        }

        public virtual bool IsSame(IXObject other)
        {
            if (other is SwObject)
            {
                return Dispatch == (other as SwObject).Dispatch;
            }
            else
            {
                return false;
            }
        }
    }
}
