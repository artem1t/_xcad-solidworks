//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Xarial.XCad.Sw
{
    public class SwTempBody : SwBody, IDisposable
    {
        public override IBody2 Body => m_TempBody;
        public override object Dispatch => m_TempBody;

        private IBody2 m_TempBody;

        internal SwTempBody(IBody2 body) : base(null)
        {
            //TODO: validate if temp body and/or convert
            m_TempBody = body;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (disposing) 
            {
                Marshal.ReleaseComObject(m_TempBody);
            }

            m_TempBody = null;
        }
    }
}
