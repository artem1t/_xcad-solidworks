//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Xarial.XCad.Attributes;
using Xarial.XCad.Enums;
using Xarial.XCad.Services;
using Xarial.XCad.Utils.PageBuilder.PageElements;

namespace Xarial.XCad.Sw.PMPage.Controls
{
    internal class PropertyManagerPageSelectionBoxControl : PropertyManagerPageBaseControl<object, IPropertyManagerPageSelectionbox>
    {
        protected override event ControlValueChangedDelegate<object> ValueChanged;

        private ISldWorks m_App;

        private Type m_ObjType;

        private ISelectionCustomFilter m_CustomFilter;

        public PropertyManagerPageSelectionBoxControl(ISldWorks app, int id, object tag,
            IPropertyManagerPageSelectionbox selBox,
            PropertyManagerPageHandlerEx handler, Type objType, ISelectionCustomFilter customFilter = null)
            : base(selBox, id, tag, handler)
        {
            m_App = app;
            m_ObjType = objType;
            m_CustomFilter = customFilter;

            m_Handler.SelectionChanged += OnSelectionChanged;

            if (m_CustomFilter != null)
            {
                m_Handler.SubmitSelection += OnSubmitSelection;
            }
        }

        internal IPropertyManagerPageSelectionbox SelectionBox
        {
            get
            {
                return SwSpecificControl;
            }
        }

        private void OnSubmitSelection(int Id, object Selection, int SelType, ref string ItemText, ref bool res)
        {
            if (Id == this.Id)
            {
                Debug.Assert(m_CustomFilter != null, "This event must not be attached if custom filter is not specified");

                //TODO: implement conversion
                res = m_CustomFilter.Filter(this, new SwSelObject(m_App.IActiveDoc2, Selection),
                    (SelectType_e)SelType, ref ItemText);
            }
        }

        private void OnSelectionChanged(int id, int count)
        {
            if (Id == id)
            {
                ValueChanged?.Invoke(this, GetSpecificValue());
            }
        }

        protected override object GetSpecificValue()
        {
            var selMgr = m_App.IActiveDoc2.ISelectionManager;

            if (SupportsMultiEntities)
            {
                var list = Activator.CreateInstance(m_ObjType) as IList;

                for (int i = 0; i < SwSpecificControl.ItemCount; i++)
                {
                    var selIndex = SwSpecificControl.SelectionIndex[i];
                    var obj = selMgr.GetSelectedObject6(selIndex, -1);
                    list.Add(obj);
                }

                return list;
            }
            else
            {
                Debug.Assert(SwSpecificControl.ItemCount <= 1, "Single entity only");

                if (SwSpecificControl.ItemCount == 1)
                {
                    var selIndex = SwSpecificControl.SelectionIndex[0];
                    var obj = selMgr.GetSelectedObject6(selIndex, -1);
                    return obj;
                }
                else
                {
                    return null;
                }
            }
        }

        protected override void SetSpecificValue(object value)
        {
            SwSpecificControl.SetSelectionFocus();

            if (value != null)
            {
                var disps = new List<DispatchWrapper>();

                if (SupportsMultiEntities)
                {
                    foreach (var item in value as IList)
                    {
                        disps.Add(new DispatchWrapper(item));
                    }
                }
                else
                {
                    disps.Add(new DispatchWrapper(value));
                }

                var selMgr = m_App.IActiveDoc2.ISelectionManager;
                
                var selData = selMgr.CreateSelectData();
                selData.Mark = SwSpecificControl.Mark;

                m_App.IActiveDoc2.Extension.MultiSelect2(disps.ToArray(), true, selData);
            }
        }
        
        private bool SupportsMultiEntities
        {
            get
            {
                return typeof(IList).IsAssignableFrom(m_ObjType);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_Handler.SelectionChanged -= OnSelectionChanged;
                m_Handler.SubmitSelection -= OnSubmitSelection;
            }
        }
    }
}
