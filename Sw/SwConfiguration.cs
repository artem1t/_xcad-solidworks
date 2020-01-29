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
using Xarial.XCad.Documents;

namespace Xarial.XCad.Sw
{
    public class SwConfiguration : IXConfiguration
    {
        private readonly IConfiguration m_Conf;

        public string Name => m_Conf.Name;

        internal SwConfiguration(IConfiguration conf) 
        {
            m_Conf = conf;
        }
    }
}
