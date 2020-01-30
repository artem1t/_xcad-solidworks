//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using Xarial.XCad.Sw.PMPage.Controls;
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

namespace Xarial.XCad.Sw.PMPage.Constructors
{
    [DefaultType(typeof(bool))]
    internal class PropertyManagerPageCheckBoxControlConstructor
        : PropertyManagerPageBaseControlConstructor<PropertyManagerPageCheckBoxControl, IPropertyManagerPageCheckbox>
    {
        public PropertyManagerPageCheckBoxControlConstructor(ISldWorks app, IconsConverter iconsConv) 
            : base(app, swPropertyManagerPageControlType_e.swControlType_Checkbox, iconsConv)
        {
        }

        protected override PropertyManagerPageCheckBoxControl CreateControl(
            IPropertyManagerPageCheckbox swCtrl, IAttributeSet atts, PropertyManagerPageHandlerEx handler, short height)
        {
            swCtrl.Caption = atts.Name;

            return new PropertyManagerPageCheckBoxControl(atts.Id, atts.Tag, swCtrl, handler);
        }
    }
}
