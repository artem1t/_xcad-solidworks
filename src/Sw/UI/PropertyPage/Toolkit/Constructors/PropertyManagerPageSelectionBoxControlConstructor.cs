﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://xcad.xarial.com/license/
//*********************************************************************

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xarial.XCad.SolidWorks.UI.PropertyPage.Toolkit.Controls;
using Xarial.XCad.SolidWorks.Utils;
using Xarial.XCad.UI.PropertyPage;
using Xarial.XCad.UI.PropertyPage.Attributes;
using Xarial.XCad.Utils.Diagnostics;
using Xarial.XCad.Utils.PageBuilder.Attributes;
using Xarial.XCad.Utils.PageBuilder.Base;

namespace Xarial.XCad.SolidWorks.UI.PropertyPage.Toolkit.Constructors
{
    [DefaultType(typeof(IXSelObject))]
    [DefaultType(typeof(IEnumerable<IXSelObject>))]
    internal class PropertyManagerPageSelectionBoxControlConstructor
        : PropertyManagerPageBaseControlConstructor<PropertyManagerPageSelectionBoxControl, IPropertyManagerPageSelectionbox>
    {
        private readonly ILogger m_Logger;

        public PropertyManagerPageSelectionBoxControlConstructor(ISldWorks app, IconsConverter iconsConv, ILogger logger)
            : base(app, swPropertyManagerPageControlType_e.swControlType_Selectionbox, iconsConv)
        {
            m_Logger = logger;
        }

        protected override PropertyManagerPageSelectionBoxControl CreateControl(
            IPropertyManagerPageSelectionbox swCtrl, IAttributeSet atts, SwPropertyManagerPageHandler handler, short height)
        {
            swCtrl.SingleEntityOnly = !(typeof(IList).IsAssignableFrom(atts.BoundType));

            if (height == -1)
            {
                height = 20;
            }

            swCtrl.Height = height;

            ISelectionCustomFilter customFilter = null;

            //TODO: identify filters based on type

            if (atts.Has<SelectionBoxOptionsAttribute>())
            {
                var selAtt = atts.Get<SelectionBoxOptionsAttribute>();

                if (selAtt.Style != 0)
                {
                    swCtrl.Style = (int)selAtt.Style;
                }

                if (selAtt.SelectionColor != 0)
                {
                    swCtrl.SetSelectionColor(true, ConvertColor(selAtt.SelectionColor));
                }

                swCtrl.SetSelectionFilters(selAtt.Filters);
                swCtrl.Mark = selAtt.SelectionMark;

                if (selAtt.CustomFilter != null)
                {
                    customFilter = Activator.CreateInstance(selAtt.CustomFilter) as ISelectionCustomFilter;

                    if (customFilter == null)
                    {
                        throw new InvalidCastException(
                            $"Specified custom filter of type {selAtt.CustomFilter.FullName} cannot be cast to {typeof(ISelectionCustomFilter).FullName}");
                    }
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