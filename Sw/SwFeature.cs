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
    public class SwFeature : SwSelObject, IXFeature
    {
        public IFeature Feature { get; }

        internal SwFeature(IModelDoc2 model, IFeature feat) : base(model, feat)
        {
            Feature = feat;
        }
    }
}
