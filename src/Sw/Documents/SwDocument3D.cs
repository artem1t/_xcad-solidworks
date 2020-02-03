//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Text;
using Xarial.XCad.Documents;
using Xarial.XCad.Geometry.Structures;
using Xarial.XCad.Utils.Diagnostics;

namespace Xarial.XCad.Sw.Documents
{
    public abstract class SwDocument3D : SwDocument, IXDocument3D
    {
        private readonly IMathUtility m_MathUtils;

        internal SwDocument3D(IModelDoc2 model, ISldWorks app, ILogger logger) : base(model, app, logger)
        {
            m_MathUtils = app.IGetMathUtility();
        }

        public IXView ActiveView => new SwModelView(Model, Model.IActiveView, m_MathUtils);

        public abstract Box3D CalculateBoundingBox();
    }
}
