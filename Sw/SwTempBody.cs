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
    public class SwTempBody : SwBody
    {
        internal SwTempBody(IBody2 body) : base(null, body)
        {
            //TODO: validate if temp body and/or convert
        }
    }
}
