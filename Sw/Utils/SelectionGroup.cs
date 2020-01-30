//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Xarial.XCad.Sw.MacroFeature
{
    internal class SelectionGroup : IDisposable
    {
        private ISelectionMgr m_SelMgr;

        internal SelectionGroup(ISelectionMgr selMgr)
        {
            if (selMgr == null)
            {
                throw new ArgumentNullException(nameof(selMgr));
            }

            m_SelMgr = selMgr;

            m_SelMgr.SuspendSelectionList();
        }

        /// <summary>
        /// Add object to current selection list
        /// </summary>
        /// <param name="disp">Pointer to dispatch</param>
        /// <param name="selData">Optional selection data</param>
        /// <returns>Result of selection</returns>
        internal bool Add(object disp, ISelectData selData = null)
        {
            if (disp == null)
            {
                throw new ArgumentNullException(nameof(disp));
            }

            return m_SelMgr.AddSelectionListObject(new DispatchWrapper(disp), selData);
        }

        /// <summary>
        /// Adds multiple objects into selection list
        /// </summary>
        /// <param name="disps">Array of dispatches to select</param>
        /// <param name="selData">Optional selection data</param>
        /// <returns>Result of the selection</returns>
        internal bool AddRange(object[] disps, ISelectData selData = null)
        {
            if (disps == null)
            {
                throw new ArgumentNullException(nameof(disps));
            }

            var dispWrappers = disps.Select(d => new DispatchWrapper(d)).ToArray();

            return m_SelMgr.AddSelectionListObjects(dispWrappers, selData) == disps.Length;
        }

        public void Dispose()
        {
            m_SelMgr.ResumeSelectionList();
        }
    }
}
