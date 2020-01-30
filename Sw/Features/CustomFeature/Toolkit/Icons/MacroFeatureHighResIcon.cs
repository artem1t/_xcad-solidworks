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

namespace Xarial.XCad.Sw.Features.CustomFeature.Toolkit.Icons
{
    internal class MacroFeatureHighResIcon : MacroFeatureIcon
    {
        internal MacroFeatureHighResIcon(Image icon, string baseName) : base(icon, baseName)
        {
        }

        public override IEnumerable<IconSizeInfo> GetIconSizes()
        {
            yield return new IconSizeInfo(m_Icon, MacroFeatureIconInfo.SizeHighResSmall, m_BaseName);
            yield return new IconSizeInfo(m_Icon, MacroFeatureIconInfo.SizeHighResMedium, m_BaseName);
            yield return new IconSizeInfo(m_Icon, MacroFeatureIconInfo.SizeHighResLarge, m_BaseName);
        }
    }
}
