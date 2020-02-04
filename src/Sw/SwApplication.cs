//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://xcad.xarial.com/license/
//*********************************************************************

using SolidWorks.Interop.sldworks;
using Xarial.XCad.Documents;
using Xarial.XCad.Geometry;
using Xarial.XCad.SolidWorks.Documents;
using Xarial.XCad.SolidWorks.Geometry;
using Xarial.XCad.Utils.Diagnostics;

namespace Xarial.XCad.SolidWorks
{
    public class SwApplication : IXApplication
    {
        public static SwApplication FromPointer(ISldWorks app)
        {
            return new SwApplication(app, new TraceLogger(""));
        }

        public ISldWorks Application { get; }

        public IXDocumentCollection Documents => SwDocuments;

        internal SwDocumentCollection SwDocuments { get; }

        public IXGeometryBuilder GeometryBuilder { get; }

        internal SwApplication(ISldWorks app, ILogger logger)
        {
            Application = app;
            SwDocuments = new SwDocumentCollection(app, logger);
            GeometryBuilder = new SwGeometryBuilder(app.IGetMathUtility(), app.IGetModeler());
        }
    }
}