﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Text;
using Xarial.XCad.Sw.UI.Commands.Toolkit.Enums;
using Xarial.XCad.UI.Commands;
using Xarial.XCad.UI.Commands.Delegates;
using Xarial.XCad.UI.Commands.Structures;

namespace Xarial.XCad.Sw.UI.Commands
{
    public class SwCommandBar : IXCommandBar
    {
        public event CommandClickDelegate CommandClick;
        public event CommandStateDelegate CommandStateResolve;

        private readonly SwApplication m_App;
        public CommandGroup CommandGroup { get; }
        public CommandBarSpec Spec { get; }

        internal SwCommandBar(SwApplication app, CommandBarSpec spec, CommandGroup cmdGroup)
        {
            Spec = spec;
            CommandGroup = cmdGroup;
            m_App = app;
        }

        internal void RaiseCommandClick(CommandSpec spec)
        {
            CommandClick?.Invoke(spec);
        }

        internal CommandItemEnableState_e RaiseCommandEnable(CommandSpec spec)
        {
            var state = new CommandState();
            state.ResolveState(spec.SupportedWorkspace, m_App);

            CommandStateResolve?.Invoke(spec, ref state);

            if (state.Enabled)
            {
                if (state.Checked)
                {
                    return CommandItemEnableState_e.SelectEnable;
                }
                else
                {
                    return CommandItemEnableState_e.DeselectEnable;
                }
            }
            else
            {
                if (state.Checked)
                {
                    return CommandItemEnableState_e.SelectDisable;
                }
                else
                {
                    return CommandItemEnableState_e.DeselectDisable;
                }
            }
        }
    }
}
