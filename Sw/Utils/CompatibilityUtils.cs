//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xarial.XCad.Sw.Utils
{
    internal static class CompatibilityUtils
    {
        internal enum HighResIconsScope_e
        {
            CommandManager,
            TaskPane
        }

        internal enum SolidWorksRevisions_e
        {
            Sw2016 = 24,
            Sw2017 = 25
        }

        internal static bool SupportsHighResIcons(this ISldWorks app, HighResIconsScope_e scope)
        {
            var majorRev = int.Parse(app.RevisionNumber().Split('.')[0]);

            switch (scope)
            {
                case HighResIconsScope_e.CommandManager:
                    return majorRev >= (int)SolidWorksRevisions_e.Sw2016;

                case HighResIconsScope_e.TaskPane:
                    return majorRev >= (int)SolidWorksRevisions_e.Sw2017;

                default:
                    //Debug.Assert(false, "Not supported scope");
                    return false;
            }
        }
    }
}
