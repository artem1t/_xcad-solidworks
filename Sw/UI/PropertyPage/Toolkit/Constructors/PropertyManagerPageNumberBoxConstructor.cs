//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Controls;
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

namespace Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Constructors
{
    [DefaultType(typeof(int))]
    [DefaultType(typeof(double))]
    internal class PropertyManagerPageNumberBoxConstructor
        : PropertyManagerPageBaseControlConstructor<PropertyManagerPageNumberBoxControl, IPropertyManagerPageNumberbox>
    {
        public PropertyManagerPageNumberBoxConstructor(ISldWorks app, IconsConverter iconsConv) 
            : base(app, swPropertyManagerPageControlType_e.swControlType_Numberbox, iconsConv)
        {
        }

        protected override PropertyManagerPageNumberBoxControl CreateControl(
            IPropertyManagerPageNumberbox swCtrl, IAttributeSet atts, PropertyManagerPageHandlerEx handler, short height)
        {
            if (height != -1)
            {
                swCtrl.Height = height;
            }

            if (atts.Has<NumberBoxOptionsAttribute>())
            {
                var style = atts.Get<NumberBoxOptionsAttribute>();
                
                if (style.Style != 0)
                {
                    swCtrl.Style = (int)style.Style;
                }

                if (style.Units != 0)
                {
                    swCtrl.SetRange2((int)style.Units, style.Minimum, style.Maximum,
                        style.Inclusive, style.Increment, style.FastIncrement, style.SlowIncrement);
                }
            }

            return new PropertyManagerPageNumberBoxControl(atts.Id, atts.Tag, swCtrl, handler);
        }
    }
}
