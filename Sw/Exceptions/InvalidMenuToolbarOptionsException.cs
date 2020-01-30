//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using System;
using Xarial.XCad.UI.Commands.Structures;

namespace Xarial.XCad.Sw.Exceptions
{
    /// <summary>
    /// Indicates that the command doesn't have either menu or toolbar option set
    /// </summary>
    public class InvalidMenuToolbarOptionsException : InvalidOperationException
    {
        internal InvalidMenuToolbarOptionsException(CommandSpec cmd) 
            : base($"Neither toolbar nor menu option is specified for {cmd.Title} ({cmd.UserId}) command. Use")
        {
        }
    }
}
