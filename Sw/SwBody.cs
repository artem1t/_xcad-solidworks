//*********************************************************************
//xCAD
//Copyright(C) 2020 Xarial Pty Limited
//Product URL: https://www.xcad.net
//License: https://github.com/xarial/xcad-solidworks/blob/master/LICENSE
//*********************************************************************

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xarial.XCad.Sw
{
    public class SwBody : SwSelObject, IXBody
    {
        public virtual IBody2 Body { get; }
        
        public bool Visible 
        {
            get => Body.Visible;
            set 
            {
                Body.HideBody(!value);
            }
        }

        internal SwBody(IBody2 body) : base(null, body)
        {
            Body = body;
        }

        public IXBody Add(IXBody other)
        {
            return PerformOperation(other, swBodyOperationType_e.SWBODYADD).FirstOrDefault();
        }

        public IXBody[] Substract(IXBody other)
        {
            return PerformOperation(other, swBodyOperationType_e.SWBODYCUT);
        }

        public IXBody[] Common(IXBody other)
        {
            return PerformOperation(other, swBodyOperationType_e.SWBODYINTERSECT);
        }

        private IXBody[] PerformOperation(IXBody other, swBodyOperationType_e op) 
        {
            if (other is SwBody)
            {
                var thisBody = Body;
                var otherBody = (other as SwBody).Body;

                if (!thisBody.IsTemporaryBody()) 
                {
                    thisBody = thisBody.ICopy();
                }

                if (!otherBody.IsTemporaryBody())
                {
                    otherBody = otherBody.ICopy();
                }

                int errs;
                var res = thisBody.Operations2((int)op, otherBody, out errs) as object[];

                if (errs != (int)swBodyOperationError_e.swBodyOperationNoError) 
                {
                    throw new Exception($"Body boolean operation failed: {(swBodyOperationError_e)errs}");
                }

                if (res?.Any() == true)
                {
                    return res.Select(b => new SwTempBody(b as IBody2)).ToArray();
                }
                else 
                {
                    return new IXBody[0];
                }
            }
            else 
            {
                throw new InvalidCastException();
            }
        }
    }
}
