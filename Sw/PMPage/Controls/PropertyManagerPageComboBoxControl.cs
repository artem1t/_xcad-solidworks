﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xarial.XCad.Utils.PageBuilder.PageElements;

namespace Xarial.XCad.Sw.PMPage.Controls
{
    internal class PropertyManagerPageComboBoxControl : PropertyManagerPageBaseControl<Enum, IPropertyManagerPageCombobox>
    {
        protected override event ControlValueChangedDelegate<Enum> ValueChanged;
        
        private ReadOnlyCollection<Enum> m_Values;

        public PropertyManagerPageComboBoxControl(int id, object tag,
            IPropertyManagerPageCombobox comboBox, ReadOnlyCollection<Enum> values,
            PropertyManagerPageHandlerEx handler) : base(comboBox, id, tag, handler)
        {
            m_Values = values;
            m_Handler.ComboBoxChanged += OnComboBoxChanged;
        }

        private void OnComboBoxChanged(int id, int selIndex)
        {
            if (Id == id)
            {
                ValueChanged?.Invoke(this, m_Values[selIndex]);
            }
        }

        protected override Enum GetSpecificValue()
        {
            var curSelIndex = SwSpecificControl.CurrentSelection;

            if (curSelIndex >= 0 && curSelIndex < m_Values.Count)
            {
                return m_Values[curSelIndex];
            }
            else
            {
                return null;
            }
        }

        protected override void SetSpecificValue(Enum value)
        {
            SwSpecificControl.CurrentSelection = (short)m_Values.IndexOf(value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_Handler.ComboBoxChanged -= OnComboBoxChanged;
            }
        }
    }
}
