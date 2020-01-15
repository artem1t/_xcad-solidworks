//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xarial.XCad.Utils.PageBuilder.PageElements;

namespace Xarial.XCad.Sw.PMPage.Controls
{
    internal abstract class PropertyManagerPageGroupBase : Group, IPropertyManagerPageElementEx
    {
        public ISldWorks App { get; private set; }
        internal PropertyManagerPageHandlerEx Handler { get; private set; }

        internal PropertyManagerPagePage ParentPage { get; private set; }

        public abstract bool Enabled { get; set; }
        public abstract bool Visible { get; set; }

        internal PropertyManagerPageGroupBase(int id, object tag, PropertyManagerPageHandlerEx handler,
            ISldWorks app, PropertyManagerPagePage parentPage) : base(id, tag)
        {
            Handler = handler;
            App = app;
            ParentPage = parentPage;
        }
    }
}
