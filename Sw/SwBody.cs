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
        
        public bool Visible 
        {
            get => Body.Visible;
            set 
            {
                Body.HideBody(!value);
            }
        }

        internal SwBody(IBody2 body) : base(null, body)
        {
            Body = body;
        }
    }
}
