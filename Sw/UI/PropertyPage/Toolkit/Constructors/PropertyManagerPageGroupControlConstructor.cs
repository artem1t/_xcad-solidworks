//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolidWorks.Interop.swconst;
using Xarial.XCad.Utils.PageBuilder.Attributes;
using Xarial.XCad.Utils.PageBuilder.Core;
using Xarial.XCad.Utils.PageBuilder.Constructors;
using Xarial.XCad.Utils.PageBuilder.Base;
using SolidWorks.Interop.sldworks;
using Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Controls;

namespace Xarial.XCad.Sw.UI.PropertyPage.Toolkit.Constructors
{
    [DefaultType(typeof(SpecialTypes.ComplexType))]
    internal class PropertyManagerPageGroupControlConstructor
        : GroupConstructor<PropertyManagerPageGroupBase, PropertyManagerPagePage>, 
        IPropertyManagerPageElementConstructor
    {
        public Type ControlType
        {
            get
            {
                return typeof(PropertyManagerPageGroupControl);
            }
        }

        public void PostProcessControls(IEnumerable<IPropertyManagerPageControlEx> ctrls)
        {
            //TODO: not used
        }

        protected override PropertyManagerPageGroupBase Create(
            PropertyManagerPageGroupBase group, IAttributeSet atts)
        {
            if (group is PropertyManagerPageTabControl)
            {
                var grp = (group as PropertyManagerPageTabControl).Tab.AddGroupBox(atts.Id, atts.Name,
                    (int)(swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded
                    | swAddGroupBoxOptions_e.swGroupBoxOptions_Visible)) as IPropertyManagerPageGroup;

                return new PropertyManagerPageGroupControl(atts.Id, atts.Tag,
                    group.Handler, grp, group.App, group.ParentPage);
            }
            //NOTE: nested groups are not supported in SOLIDWORKS, creating the group in page instead
            else if (group is PropertyManagerPageGroupControl)
            {
                return Create(group.ParentPage, atts);
            }
            else
            {
                throw new NullReferenceException("Not supported group type");
            }
        }

        protected override PropertyManagerPageGroupBase Create(PropertyManagerPagePage page, IAttributeSet atts)
        {
            var grp = page.Page.AddGroupBox(atts.Id, atts.Name,
                (int)(swAddGroupBoxOptions_e.swGroupBoxOptions_Expanded
                | swAddGroupBoxOptions_e.swGroupBoxOptions_Visible)) as IPropertyManagerPageGroup;

            return new PropertyManagerPageGroupControl(atts.Id, atts.Tag,
                page.Handler, grp, page.App, page);
        }
    }
}
