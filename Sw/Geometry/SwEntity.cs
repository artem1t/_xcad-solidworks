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
using Xarial.XCad.Geometry;

namespace Xarial.XCad.Sw
{
    public abstract class SwEntity : SwSelObject, IXEntity
    {
        public IEntity Entity { get; }

        public abstract IXBody Body { get; }

        internal SwEntity(IEntity entity) : base(null, entity)
        {
            Entity = entity;
        }
    }
}
