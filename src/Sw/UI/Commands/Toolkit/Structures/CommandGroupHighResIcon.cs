﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://xcad.xarial.com/license/
//*********************************************************************

using System.Collections.Generic;
using System.Drawing;
using Xarial.XCad.SolidWorks.Base;

namespace Xarial.XCad.SolidWorks.UI.Commands.Toolkit.Structures
{
    internal class CommandGroupHighResIcon : CommandGroupIcon
    {
        internal CommandGroupHighResIcon(Image icon) : base(icon)
        {
        }

        public override IEnumerable<IconSizeInfo> GetIconSizes()
        {
            yield return new IconSizeInfo(m_Icon, new Size(20, 20));
            yield return new IconSizeInfo(m_Icon, new Size(32, 32));
            yield return new IconSizeInfo(m_Icon, new Size(40, 40));
            yield return new IconSizeInfo(m_Icon, new Size(64, 64));
            yield return new IconSizeInfo(m_Icon, new Size(96, 96));
            yield return new IconSizeInfo(m_Icon, new Size(128, 128));
        }
    }
}