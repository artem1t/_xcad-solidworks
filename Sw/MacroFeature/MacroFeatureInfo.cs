//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Xarial.XCad.Attributes;
using Xarial.XCad.Utils.Reflection;

namespace Xarial.XCad.Sw.MacroFeature
{
    internal static class MacroFeatureInfo
    {
        internal static string GetBaseName<TMacroFeature>()
            where TMacroFeature : SwMacroFeatureDefinition
        {
            return GetBaseName(typeof(TMacroFeature));
        }

        internal static string GetBaseName(Type macroFeatType)
        {
            if (!typeof(SwMacroFeatureDefinition).IsAssignableFrom(macroFeatType))
            {
                throw new InvalidCastException(
                    $"{macroFeatType.FullName} must inherit {typeof(SwMacroFeatureDefinition).FullName}");
            }

            string baseName = "";

            macroFeatType.TryGetAttribute<CustomFeatureOptionsAttribute>(a =>
            {
                baseName = a.BaseName;
            });

            if (string.IsNullOrEmpty(baseName))
            {
                macroFeatType.TryGetAttribute<DisplayNameAttribute>(a =>
                {
                    baseName = a.DisplayName;
                });
            }

            if (string.IsNullOrEmpty(baseName))
            {
                baseName = macroFeatType.Name;
            }

            return baseName;
        }

        internal static string GetProgId<TMacroFeature>()
            where TMacroFeature : SwMacroFeatureDefinition
        {
            return GetProgId(typeof(TMacroFeature));
        }

        internal static string GetProgId(Type macroFeatType)
        {
            if (!typeof(SwMacroFeatureDefinition).IsAssignableFrom(macroFeatType))
            {
                throw new InvalidCastException(
                    $"{macroFeatType.FullName} must inherit {typeof(SwMacroFeatureDefinition).FullName}");
            }

            string progId = "";

            if (!macroFeatType.TryGetAttribute<ProgIdAttribute>(a => progId = a.Value))
            {
                progId = macroFeatType.FullName;
            }

            return progId;
        }
    }
}
