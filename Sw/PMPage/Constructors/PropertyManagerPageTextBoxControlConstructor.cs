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
using Xarial.XCad.UI.PropertyPage.Attributes;

namespace Xarial.XCad.Sw.PMPage.Constructors
{
    [DefaultType(typeof(string))]
    internal class PropertyManagerPageTextBoxControlConstructor
        : PropertyManagerPageBaseControlConstructor<PropertyManagerPageTextBoxControl, IPropertyManagerPageTextbox>
    {
        public PropertyManagerPageTextBoxControlConstructor(ISldWorks app, IconsConverter iconsConv)
            : base(app, swPropertyManagerPageControlType_e.swControlType_Textbox, iconsConv)
        {
        }

        protected override PropertyManagerPageTextBoxControl CreateControl(
            IPropertyManagerPageTextbox swCtrl, IAttributeSet atts, PropertyManagerPageHandlerEx handler, short height)
        {
            if (height != -1)
            {
                swCtrl.Height = height;
            }

            if (atts.Has<TextBoxOptionsAttribute>())
            {
                var style = atts.Get<TextBoxOptionsAttribute>();
                
                if (style.Style != 0)
                {
                    swCtrl.Style = (int)style.Style;
                }
            }

            return new PropertyManagerPageTextBoxControl(atts.Id, atts.Tag, swCtrl, handler);
        }
    }
}
