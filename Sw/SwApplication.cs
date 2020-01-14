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
using Xarial.XCad.Utils.Diagnostics;

namespace Xarial.XCad.Sw
{
    public class SwApplication : IXApplication
    {
        public ISldWorks Application { get; }

        public IXDocumentCollection Documents { get; }

        internal SwApplication(ISldWorks app, ILogger logger)
        {
            Application = app;
            Documents = new SwDocumentCollection(app, logger);
        }
    }
}
