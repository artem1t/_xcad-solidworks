//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xarial.XCad.Sw.Base;

namespace Xarial.XCad.Sw.Core
{
    internal class CommandGroupIcon : IIcon
    {
        protected readonly Image m_Icon;

        private static readonly Color m_CommandTransparencyKey
                    = Color.FromArgb(192, 192, 192);
        public virtual Color TransparencyKey
        {
            get
            {
                return m_CommandTransparencyKey;
            }
        }

        internal CommandGroupIcon(Image icon)
        {
            m_Icon = icon;
        }

        public virtual IEnumerable<IconSizeInfo> GetIconSizes() 
        {
            yield return new IconSizeInfo(m_Icon, new Size(16, 16));
            yield return new IconSizeInfo(m_Icon, new Size(24, 24));
        }
    }
}
