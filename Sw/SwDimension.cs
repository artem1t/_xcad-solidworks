//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Xarial.XCad.Sw
{
    public class SwDimension : SwSelObject, IXDimension, IDisposable
    {
        public IDisplayDimension DisplayDimension { get; private set; }

        private IDimension m_Dimension;

        public IDimension Dimension => m_Dimension ?? (m_Dimension = DisplayDimension.GetDimension2(0));

        internal SwDimension(IDisplayDimension dispDim) : base(null, dispDim) 
        {
            DisplayDimension = dispDim;
        }

        public virtual double GetValue(string confName)
        {
            var dim = DisplayDimension.GetDimension2(0);
            var val = (dim.GetSystemValue3(
                (int)swInConfigurationOpts_e.swSpecifyConfiguration,
                new string[] { confName }) as double[])[0];

            return val;
        }

        public void Dispose()
        {
            Dispose(true);
            
            //NOTE: releasing the pointers as unreleased pointer might cause crash
            if (m_Dimension != null && Marshal.IsComObject(m_Dimension))
            {
                Marshal.ReleaseComObject(m_Dimension);
                m_Dimension = null;
            }

            if (DisplayDimension != null && Marshal.IsComObject(DisplayDimension))
            {
                Marshal.ReleaseComObject(DisplayDimension);
                DisplayDimension = null;
            }

            GC.Collect();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        protected virtual void Dispose(bool disposing) 
        {
        }

        public void SetValue(double val, string confName)
        {
            Dimension.SetSystemValue3(val,
                (int)swSetValueInConfiguration_e.swSetValue_InSpecificConfigurations,
                new string[] { confName });
        }
    }
}
