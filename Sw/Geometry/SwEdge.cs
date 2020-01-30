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
using Xarial.XCad.Geometry;

namespace Xarial.XCad.Sw.Geometry
{
    public class SwEdge : SwEntity, IXEdge
    {
        public IEdge Edge { get; }

        public override IXBody Body => new SwBody(Edge.GetBody());

        internal SwEdge(IEdge edge) : base(edge as IEntity)
        {
            Edge = edge;
        }
    }
}
