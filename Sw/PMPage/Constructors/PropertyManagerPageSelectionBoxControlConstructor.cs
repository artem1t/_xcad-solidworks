//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using Xarial.XCad.Sw.PMPage.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolidWorks.Interop.swconst;
using System.Drawing;
using SolidWorks.Interop.sldworks;
using System.Collections;
using Xarial.XCad.Utils.Diagnostics;
using Xarial.XCad.Sw.Utils;
using Xarial.XCad.Attributes;
using Xarial.XCad.Utils.PageBuilder.Base;
using Xarial.XCad.Services;

namespace Xarial.XCad.Sw.PMPage.Constructors
{
    internal class PropertyManagerPageSelectionBoxControlConstructor
        : PropertyManagerPageBaseControlConstructor<PropertyManagerPageSelectionBoxControl, IPropertyManagerPageSelectionbox>, ISelectionBoxConstructor
    {
        private readonly ILogger m_Logger;

        public PropertyManagerPageSelectionBoxControlConstructor(ISldWorks app, IconsConverter iconsConv, ILogger logger) 
            : base(app, swPropertyManagerPageControlType_e.swControlType_Selectionbox, iconsConv)
        {
            m_Logger = logger;
        }

        protected override PropertyManagerPageSelectionBoxControl CreateControl(
            IPropertyManagerPageSelectionbox swCtrl, IAttributeSet atts, PropertyManagerPageHandlerEx handler, short height)
        {
            var selAtt = atts.Get<SelectionBoxAttribute>();
            swCtrl.SetSelectionFilters(selAtt.Filters);
            swCtrl.Mark = selAtt.SelectionMark;

            swCtrl.SingleEntityOnly = !(typeof(IList).IsAssignableFrom(atts.BoundType));

            ISelectionCustomFilter customFilter = null;

            if (selAtt.CustomFilter != null)
            {
                customFilter = Activator.CreateInstance(selAtt.CustomFilter) as ISelectionCustomFilter;

                if (customFilter == null)
                {
                    throw new InvalidCastException(
                        $"Specified custom filter of type {selAtt.CustomFilter.FullName} cannot be cast to {typeof(ISelectionCustomFilter).FullName}");
                }
            }

            if (height == -1)
            {
                height = 20;
            }

            swCtrl.Height = height;

            if (atts.Has<SelectionBoxOptionsAttribute>())
            {
                var style = atts.Get<SelectionBoxOptionsAttribute>();

                if (style.Style != 0)
                {
                    swCtrl.Style = (int)style.Style;
                }

                if (style.SelectionColor != 0)
                {
                    swCtrl.SetSelectionColor(true, ConvertColor(style.SelectionColor));
                }
            }

            return new PropertyManagerPageSelectionBoxControl(m_App, atts.Id, atts.Tag,
                swCtrl, handler, atts.BoundType, customFilter);
        }

        public override void PostProcessControls(IEnumerable<IPropertyManagerPageControlEx> ctrls)
        {
            var selBoxes = ctrls.OfType<PropertyManagerPageSelectionBoxControl>().ToArray();

            var autoAssignSelMarksCtrls = selBoxes
                .Where(s => s.SelectionBox.Mark == -1).ToList();

            var assignedMarks = ctrls.OfType<PropertyManagerPageSelectionBoxControl>()
                .Except(autoAssignSelMarksCtrls).Select(c => c.SelectionBox.Mark).ToList();

            ValidateMarks(assignedMarks);

            if (selBoxes.Length == 1)
            {
                autoAssignSelMarksCtrls[0].SelectionBox.Mark = 0;
            }
            else
            {
                int index = 0;

                autoAssignSelMarksCtrls.ForEach(c =>
                {
                    int mark;
                    do
                    {
                        mark = (int)Math.Pow(2, index);
                        index++;
                    } while (assignedMarks.Contains(mark));

                    c.SelectionBox.Mark = mark;
                });
            }

            m_Logger.Log($"Assigned selection box marks: {string.Join(", ", selBoxes.Select(s => s.SelectionBox.Mark).ToArray())}");
        }

        private void ValidateMarks(List<int> assignedMarks)
        {
            if (assignedMarks.Count > 1)
            {
                var dups = assignedMarks.GroupBy(m => m).Where(g => g.Count() > 1).Select(g => g.Key);

                if (dups.Any())
                {
                    m_Logger.Log($"Potential issue for selection boxes as there are duplicate selection marks: {string.Join(", ", dups.ToArray())}");
                }

                var joinedMarks = assignedMarks.Where(m => m != 0 && !IsPowerOfTwo(m));

                if (joinedMarks.Any())
                {
                    m_Logger.Log($"Potential issue for selection boxes as not all marks are power of 2: {string.Join(", ", joinedMarks.ToArray())}");
                }

                if (assignedMarks.Any(m => m == 0))
                {
                    m_Logger.Log($"Potential issue for selection boxes as some of the marks is 0 which means that all selections allowed");
                }
            }
        }

        private bool IsPowerOfTwo(int mark)
        {
            return (mark != 0) && ((mark & (mark - 1)) == 0);
        }
    }
}
