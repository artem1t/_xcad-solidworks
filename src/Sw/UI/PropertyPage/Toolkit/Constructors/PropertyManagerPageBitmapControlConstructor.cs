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
using Xarial.XCad.UI.PropertyPage.Attributes;
using Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Controls;

namespace Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Constructors
{
    [DefaultType(typeof(Image))]
    internal class PropertyManagerPageBitmapControlConstructor
        : PropertyManagerPageBaseControlConstructor<PropertyManagerPageBitmapControl, IPropertyManagerPageBitmap>
    {
        private readonly IconsConverter m_IconsConv;

        public PropertyManagerPageBitmapControlConstructor(ISldWorks app, IconsConverter iconsConv) 
            : base(app, swPropertyManagerPageControlType_e.swControlType_Bitmap, iconsConv)
        {
            m_IconsConv = iconsConv;
        }

        protected override PropertyManagerPageBitmapControl CreateControl(
            IPropertyManagerPageBitmap swCtrl, IAttributeSet atts, SwPropertyManagerPageHandler handler, short height)
        {
            Size? size = null;

            if (atts.Has<BitmapOptionsAttribute>())
            {
                var opts = atts.Get<BitmapOptionsAttribute>();
                size = opts.Size;
            }

            return new PropertyManagerPageBitmapControl(m_IconsConv, atts.Id, atts.Tag, size, swCtrl, handler);
        }
    }
}
