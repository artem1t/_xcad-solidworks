﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Xarial.XCad.Sw.Base;

namespace Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Icons
{
    internal class TitleIcon : IIcon
    {
        internal Image Icon { get; private set; }

        public Color TransparencyKey
        {
            get
            {
                return Color.White;
            }
        }

        internal TitleIcon(Image icon)
        {
            Icon = icon;
        }

        public IEnumerable<IconSizeInfo> GetIconSizes()
        {
            yield return new IconSizeInfo(Icon, new Size(22, 22));
        }
    }
}
