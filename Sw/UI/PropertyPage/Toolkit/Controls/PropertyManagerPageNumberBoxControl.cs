﻿//*********************************************************************
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
    internal class PropertyManagerPageNumberBoxControl : PropertyManagerPageBaseControl<double, IPropertyManagerPageNumberbox>
    {
        protected override event ControlValueChangedDelegate<double> ValueChanged;

        public PropertyManagerPageNumberBoxControl(int id, object tag,
            IPropertyManagerPageNumberbox numberBox, 
            PropertyManagerPageHandlerEx handler) : base(numberBox, id, tag, handler)
        {
            m_Handler.NumberChanged += OnNumberChanged;
        }

        private void OnNumberChanged(int id, double value)
        {
            if (Id == id)
            {
                ValueChanged?.Invoke(this, value);
            }
        }
        
        protected override double GetSpecificValue()
        {
            return SwSpecificControl.Value;
        }

        protected override void SetSpecificValue(double value)
        {
            SwSpecificControl.Value = value;
        }

        protected override void Dispose(bool disposing) 
        {
            if (disposing) 
            {
                m_Handler.NumberChanged -= OnNumberChanged;
            }
        }
    }
}
