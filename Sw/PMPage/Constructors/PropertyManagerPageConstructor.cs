﻿//*********************************************************************
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
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using Xarial.XCad.Sw.Utils;
using Xarial.XCad.Utils.PageBuilder.Constructors;
using Xarial.XCad.Utils.PageBuilder.Base;
using Xarial.XCad.Utils.Reflection;
using Xarial.XCad.Sw.PMPage.Icons;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.UI.PropertyPage.Attributes;
using Xarial.XCad.UI.PropertyPage.Enums;

namespace Xarial.XCad.Sw.PMPage.Constructors
{
    internal class PropertyManagerPageConstructor : PageConstructor<PropertyManagerPagePage>
    {
        private readonly ISldWorks m_App;
        private readonly IconsConverter m_IconsConv;
        private readonly PropertyManagerPageHandlerEx m_Handler;

        internal PropertyManagerPageConstructor(ISldWorks app, IconsConverter iconsConv, PropertyManagerPageHandlerEx handler)
        {
            m_App = app;
            m_IconsConv = iconsConv;

            m_Handler = handler;
            handler.Init(m_App);
        }

        protected override PropertyManagerPagePage Create(IAttributeSet atts)
        {
            int err = -1;

            swPropertyManagerPageOptions_e opts;

            TitleIcon titleIcon = null;

            IconAttribute commIconAtt;
            if (atts.BoundType.TryGetAttribute(out commIconAtt))
            {
                if (commIconAtt.Icon != null)
                {
                    titleIcon = new TitleIcon(commIconAtt.Icon);
                }
            }

            if (atts.Has<PageOptionsAttribute>())
            {
                var optsAtt = atts.Get<PageOptionsAttribute>();

                //TODO: implement conversion
                opts = (swPropertyManagerPageOptions_e)optsAtt.Options;

                //if (optsAtt.Icon != null)
                //{
                //    titleIcon = optsAtt.Icon;
                //}
            }
            else
            {
                //TODO: implement conversion
                opts = (swPropertyManagerPageOptions_e)(PageOptions_e.OkayButton | PageOptions_e.CancelButton);
            }

            var helpLink = "";
            var whatsNewLink = "";

            if (atts.Has<HelpAttribute>())
            {
                var helpAtt = atts.Get<HelpAttribute>();

                if (!string.IsNullOrEmpty(helpAtt.WhatsNewLink))
                {
                    if (!opts.HasFlag(swPropertyManagerPageOptions_e.swPropertyManagerOptions_WhatsNew))
                    {
                        opts |= swPropertyManagerPageOptions_e.swPropertyManagerOptions_WhatsNew;
                    }
                }

                helpLink = helpAtt.HelpLink;
                whatsNewLink = helpAtt.WhatsNewLink;
            }

            var page = m_App.CreatePropertyManagerPage(atts.Name,
                (int)opts,
                m_Handler, ref err) as IPropertyManagerPage2;

            if (titleIcon != null)
            {
                var iconPath = m_IconsConv.ConvertIcon(titleIcon).First();
                page.SetTitleBitmap2(iconPath);
            }

            if (atts.Has<MessageAttribute>())
            {
                var msgAtt = atts.Get<MessageAttribute>();
                page.SetMessage3(msgAtt.Text, (int)msgAtt.Visibility,
                    (int)msgAtt.Expanded, msgAtt.Caption);
            }
            else if (!string.IsNullOrEmpty(atts.Description))
            {
                page.SetMessage3(atts.Description, (int)swPropertyManagerPageMessageVisibility.swMessageBoxVisible,
                    (int)swPropertyManagerPageMessageExpanded.swMessageBoxExpand, "");
            }

            return new PropertyManagerPagePage(page, m_Handler, m_App, helpLink, whatsNewLink);
        }
    }
}
