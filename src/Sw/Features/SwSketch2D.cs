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
    public class SwSketch2D : SwSketchBase, IXSketch2D
    {
        public SwSketch2D(IModelDoc2 model, IFeature feat, bool created) : base(model, feat, created)
        {
        }

        protected override ISketch CreateSketch()
        {
            //TODO: select the plane or face
            m_Model.InsertSketch2(true);
            return m_Model.SketchManager.ActiveSketch;
        }

        protected override void ToggleEditSketch()
        {
            m_Model.InsertSketch2(true);
        }
    }
}
