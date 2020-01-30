//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Xarial.XCad.Sw.Base;
using Xarial.XCad.Sw.Core;

namespace Xarial.XCad.Sw.MacroFeature.Icons
{
    internal class MacroFeatureIcon : IIcon
    {
        protected readonly string m_BaseName;
        protected readonly Image m_Icon;

        public Color TransparencyKey => Color.White;

        internal MacroFeatureIcon(Image icon, string baseName)
        {
            m_BaseName = baseName;
            m_Icon = icon;
        }

        public virtual IEnumerable<IconSizeInfo> GetIconSizes()
        {
            yield return new IconSizeInfo(m_Icon, MacroFeatureIconInfo.Size, m_BaseName);
        }
    }
}
