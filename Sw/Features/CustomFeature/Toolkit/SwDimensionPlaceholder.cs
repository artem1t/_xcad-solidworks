﻿//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad/blob/master/LICENSE
//*********************************************************************

using System;
using System.Collections.Generic;
using System.Text;
using Xarial.XCad.Sw.Annotations;

namespace Xarial.XCad.Sw.Features.CustomFeature.Toolkit
{
    /// <summary>
    /// This is a mock implementation of display SOLIDWORKS dimension
    /// It is used in <see cref="Services.IParameterConverter.ConvertDisplayDimensions(IXDocument, IXCustomFeature, IXDimension[])"/>
    /// for supporting the backward compatibility of macro feature parameters
    /// </summary>
    internal class SwDimensionPlaceholder : SwDimension
    {
        internal SwDimensionPlaceholder() : base(null) 
        {
        }

        public override double GetValue(string confName = "")
        {
            return double.NaN;
        }
    }
}
