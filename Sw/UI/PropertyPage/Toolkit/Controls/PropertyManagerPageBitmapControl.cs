﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System.Drawing;
using Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Icons;
using Xarial.XCad.Sw.Utils;
using Xarial.XCad.Utils.PageBuilder.PageElements;

namespace Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Controls
{
    internal class PropertyManagerPageBitmapControl : PropertyManagerPageBaseControl<Image, IPropertyManagerPageBitmap>
    {
#pragma warning disable CS0067
        protected override event ControlValueChangedDelegate<Image> ValueChanged;
#pragma warning restore CS0067
        
        private readonly IconsConverter m_IconsConv;

        private Image m_Image;
        private readonly Size m_Size;

        public PropertyManagerPageBitmapControl(IconsConverter iconsConv,
            int id, object tag, Size? size,
            IPropertyManagerPageBitmap bitmap,
            PropertyManagerPageHandlerEx handler) : base(bitmap, id, tag, handler)
        {
            m_Size = size.HasValue ? size.Value : new Size(18, 18);
            m_IconsConv = iconsConv;
        }
        
        protected override Image GetSpecificValue()
        {
            return m_Image;
        }

        protected override void SetSpecificValue(Image value)
        {
            if (value == null)
            {
                //TODO: use default
                //value = Resources.DefaultBitmap;
            }
            
            var icons = m_IconsConv.ConvertIcon(new ControlIcon(value, m_Size));
            SwSpecificControl.SetBitmapByName(icons[0], icons[1]);

            m_Image = value;
        }
    }
}
