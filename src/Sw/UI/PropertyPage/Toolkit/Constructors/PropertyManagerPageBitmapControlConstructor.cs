﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://xcad.xarial.com/license/
//*********************************************************************

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Drawing;
using Xarial.XCad.SolidWorks.UI.PropertyPage.Toolkit.Controls;
using Xarial.XCad.SolidWorks.Utils;
using Xarial.XCad.UI.PropertyPage.Attributes;
using Xarial.XCad.Utils.PageBuilder.Attributes;
using Xarial.XCad.Utils.PageBuilder.Base;

namespace Xarial.XCad.SolidWorks.UI.PropertyPage.Toolkit.Constructors
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