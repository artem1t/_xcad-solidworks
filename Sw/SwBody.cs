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

namespace Xarial.XCad.Sw
{
    public class SwBody : SwSelObject, IXBody
    {
        public IBody2 Body { get; }

        internal SwBody(IModelDoc2 model, IBody2 body) : base(model, body)
        {
            Body = body;
        }
    }
}
