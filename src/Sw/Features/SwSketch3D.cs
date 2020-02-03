//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using Xarial.XCad.Features;

namespace Xarial.XCad.Sw.Features
{
    public class SwSketch3D : SwSketchBase, IXSketch3D
    {
        public SwSketch3D(IModelDoc2 model, IFeature feat, bool created) : base(model, feat, created)
        {
        }

        protected override ISketch CreateSketch()
        {
            //TODO: try to use API only selection
            m_Model.ClearSelection2(true);
            m_Model.Insert3DSketch2(true);
            return m_Model.SketchManager.ActiveSketch;
        }

        protected override void ToggleEditSketch()
        {
            m_Model.Insert3DSketch2(true);
        }
    }
}
