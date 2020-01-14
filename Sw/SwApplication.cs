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
    public class SwApplication : IXApplication 
    {
        public IXDocument ActiveDocument
        {
            get
            {
                return null;
            }
        }

        public ISldWorks Application { get; }

        internal SwApplication(ISldWorks app)
        {
            Application = app;
        }
    }
}
