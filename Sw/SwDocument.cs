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
    public abstract class SwDocument : IXDocument
    {
        public IModelDoc2 Model { get; }

        internal SwDocument(IModelDoc2 model) 
        {
            Model = model;
        }
    }

    public class SwPart : SwDocument, IXPart
    {
        public IPartDoc Part { get; }

        internal SwPart(IPartDoc part) : base((IModelDoc2)part) 
        {
            Part = part;
        }
    }

    public class SwAssembly : SwDocument, IXPart
    {
        public IAssemblyDoc Assembly { get; }

        internal SwAssembly(IAssemblyDoc assembly) : base((IModelDoc2)assembly)
        {
            Assembly = assembly;
        }
    }

    public class SwDrawing : SwDocument, IXDrawing
    {
        public IDrawingDoc Drawing { get; }

        internal SwDrawing(IDrawingDoc drawing) : base((IModelDoc2)drawing)
        {
            Drawing = drawing;
        }
    }
}
