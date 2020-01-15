﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using Xarial.XCad.Utils.PageBuilder.PageElements;

namespace Xarial.XCad.Sw.PMPage.Controls
{
    internal class PropertyManagerPageCheckBoxControl : PropertyManagerPageBaseControl<bool, IPropertyManagerPageCheckbox>
    {
        protected override event ControlValueChangedDelegate<bool> ValueChanged;
        
        public PropertyManagerPageCheckBoxControl(int id, object tag,
            IPropertyManagerPageCheckbox checkBox,
            PropertyManagerPageHandlerEx handler) : base(checkBox, id, tag, handler)
        {
            m_Handler.CheckChanged += OnCheckChanged;
        }

        private void OnCheckChanged(int id, bool isChecked)
        {
            if (Id == id)
            {
                ValueChanged?.Invoke(this, isChecked);
            }
        }

        protected override bool GetSpecificValue()
        {
            return SwSpecificControl.Checked;
        }

        protected override void SetSpecificValue(bool value)
        {
            SwSpecificControl.Checked = value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_Handler.CheckChanged -= OnCheckChanged;
            }
        }
    }
}
