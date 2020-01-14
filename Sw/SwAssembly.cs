//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using Xarial.XCad.Diagnostics;

namespace Xarial.XCad.Sw
{
    public class SwAssembly : SwDocument, IXPart
    {
        public IAssemblyDoc Assembly { get; }

        internal SwAssembly(IAssemblyDoc assembly, ISldWorks app, ILogger logger)
            : base((IModelDoc2)assembly, app, logger)
        {
            Assembly = assembly;
        }
    }
}
