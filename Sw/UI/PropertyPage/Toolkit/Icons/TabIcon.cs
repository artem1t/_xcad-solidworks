//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using System.Collections.Generic;
using System.Drawing;
using Xarial.XCad.Sw.Base;
using Xarial.XCad.Sw.Core;

namespace Xarial.XCad.Sw.PMPage.Icons
{
    internal class TabIcon : IIcon
    {
        internal Image Icon { get; private set; }

        public Color TransparencyKey
        {
            get
            {
                return Color.White;
            }
        }

        internal TabIcon(Image icon)
        {
            Icon = icon;
        }

        public IEnumerable<IconSizeInfo> GetIconSizes()
        {
            yield return new IconSizeInfo(Icon, new Size(16, 18));
        }
    }
}
