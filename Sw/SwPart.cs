//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using Xarial.XCad.Utils.Diagnostics;

namespace Xarial.XCad.Sw
{
    public class SwPart : SwDocument, IXPart
    {
        public IPartDoc Part { get; }

        internal SwPart(IPartDoc part, ISldWorks app, ILogger logger) 
            : base((IModelDoc2)part, app, logger) 
        {
            Part = part;
        }
    }
}
