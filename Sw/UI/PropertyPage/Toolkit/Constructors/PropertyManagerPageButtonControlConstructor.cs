//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolidWorks.Interop.swconst;
using System.Drawing;
using SolidWorks.Interop.sldworks;
using Xarial.XCad.Utils.PageBuilder.Attributes;
using Xarial.XCad.Sw.Utils;
using Xarial.XCad.Utils.PageBuilder.Base;
using Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Controls;

namespace Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Constructors
{
    [DefaultType(typeof(Action))]
    internal class PropertyManagerPageButtonControlConstructor
        : PropertyManagerPageBaseControlConstructor<PropertyManagerPageButtonControl, IPropertyManagerPageButton>
    {
        public PropertyManagerPageButtonControlConstructor(ISldWorks app, IconsConverter iconsConv) 
            : base(app, swPropertyManagerPageControlType_e.swControlType_Button, iconsConv)
        {
        }

        protected override PropertyManagerPageButtonControl CreateControl(
            IPropertyManagerPageButton swCtrl, IAttributeSet atts, SwPropertyManagerPageHandler handler, short height)
        {
            swCtrl.Caption = atts.Name;

            return new PropertyManagerPageButtonControl(atts.Id, atts.Tag, swCtrl, handler);
        }
    }
}
