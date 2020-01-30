//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xarial.XCad.Utils.PageBuilder.PageElements;

namespace Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Controls
{
    internal class PropertyManagerPageTextBoxControl : PropertyManagerPageBaseControl<string, IPropertyManagerPageTextbox>
    {
        protected override event ControlValueChangedDelegate<string> ValueChanged;
        
        internal PropertyManagerPageTextBoxControl(int id, object tag,
            IPropertyManagerPageTextbox textBox,
            PropertyManagerPageHandlerEx handler) : base(textBox, id, tag, handler)
        {
            m_Handler.TextChanged += OnTextChanged;
        }

        private void OnTextChanged(int id, string text)
        {
            if (Id == id)
            {
                ValueChanged?.Invoke(this, text);
            }
        }

        protected override string GetSpecificValue()
        {
            return SwSpecificControl.Text;
        }

        protected override void SetSpecificValue(string value)
        {
            SwSpecificControl.Text = value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_Handler.TextChanged -= OnTextChanged;
            }
        }
    }
}
