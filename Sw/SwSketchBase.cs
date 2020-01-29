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
using System.Text;
using Xarial.XCad.Services;
using Xarial.XCad.Structures;

namespace Xarial.XCad.Sw
{
    public class SwSketchEntityCollection : IXSketchEntityCollection
    {
        public int Count => m_Sketch.IsCreated ? 0 : m_Cache.Count;

        private readonly SwSketchBase m_Sketch;

        private readonly List<IXSketchEntity> m_Cache;

        private readonly IModelDoc2 m_Model;
        private readonly ISketchManager m_SkMgr;

        public SwSketchEntityCollection(IModelDoc2 model, SwSketchBase sketch, ISketchManager skMgr)
        {
            m_Model = model;
            m_Sketch = sketch;
            m_SkMgr = skMgr;
            m_Cache = new List<IXSketchEntity>();
        }

        public void AddRange(IEnumerable<IXSketchEntity> segments)
        {
            if (m_Sketch.IsCreated)
            {
                CreateSegments(segments, m_Sketch.Sketch);
            }
            else 
            {
                m_Cache.AddRange(segments);
            }
        }

        internal void CommitCache(ISketch sketch) 
        {
            CreateSegments(m_Cache, sketch);

            m_Cache.Clear();
        }

        private void CreateSegments(IEnumerable<IXSketchEntity> segments, ISketch sketch)
        {
            var addToDbOrig = m_SkMgr.AddToDB;

            m_Sketch.SetEditMode(sketch, true);

            m_SkMgr.AddToDB = true;

            foreach (SwSketchEntity seg in segments)
            {
                seg.Create();
            }

            m_SkMgr.AddToDB = addToDbOrig;

            m_Sketch.SetEditMode(sketch, false);
        }

        public IEnumerator<IXSketchEntity> GetEnumerator()
        {
            if (m_Sketch.IsCreated)
            {
                throw new NotImplementedException();
            }
            else 
            {
                return m_Cache.GetEnumerator();
            }
        }

        public IXSketchLine NewLine()
        {
            return new SwSketchLine(m_Model, null, false);
        }

        public IXSketchPoint NewPoint()
        {
            return new SwSketchPoint(m_Model, null, false);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public abstract class SwSketchEntity : SwSelObject, IXSketchEntity
    {
        internal abstract void Create();

        internal SwSketchEntity(IModelDoc2 model, object ent) : base(model, ent) 
        {
        }
    }

    public abstract class SwSketchEntity<TEnt> : SwSketchEntity
    {
        protected readonly ElementCreator<TEnt> m_Creator;

        protected readonly ISketchManager m_SketchMgr;

        public TEnt Element
        {
            get
            {
                return m_Creator.Element;
            }
        }

        internal SwSketchEntity(IModelDoc2 model, TEnt ent, bool created) : base(model, ent)
        {
            m_SketchMgr = model.SketchManager;
            m_Creator = new ElementCreator<TEnt>(CreateSketchEntity, ent, created);
        }

        internal override void Create()
        {
            m_Creator.Create();
        }

        protected abstract TEnt CreateSketchEntity();
    }

    public class SwSketchLine : SwSketchEntity<ISketchLine>, IXSketchLine
    {
        private readonly SwSketchPoint m_StartPoint;
        private readonly SwSketchPoint m_EndPoint;

        public IXSketchPoint StartPoint => m_StartPoint;
        public IXSketchPoint EndPoint => m_EndPoint;

        public SwSketchLine(IModelDoc2 model, ISketchLine ent, bool created) : base(model, ent, created)
        {
            m_StartPoint = new SwSketchPoint(model, ent?.IGetStartPoint2(), created);
            m_EndPoint = new SwSketchPoint(model, ent?.IGetEndPoint2(), created);
        }

        protected override ISketchLine CreateSketchEntity()
        {
            var line = (ISketchLine)m_SketchMgr.CreateLine(
                StartPoint.Coordinate.X,
                StartPoint.Coordinate.Y,
                StartPoint.Coordinate.Z,
                EndPoint.Coordinate.X,
                EndPoint.Coordinate.Y,
                EndPoint.Coordinate.Z);

            m_StartPoint.SetLinePoint(line.IGetStartPoint2());
            m_EndPoint.SetLinePoint(line.IGetEndPoint2());

            return line;
        }
    }

    public class SwSketchPoint : SwSketchEntity<ISketchPoint>, IXSketchPoint
    {
        private Point m_CachedCoordinate;
        private ISketchPoint m_LinePoint;

        public SwSketchPoint(IModelDoc2 model, ISketchPoint ent, bool created) : base(model, ent, created)
        {
        }

        public Point Coordinate 
        {
            get 
            {
                if (m_Creator.IsCreated)
                {
                    return new Point(Element.X, Element.Y, Element.Z);
                }
                else 
                {
                    return m_CachedCoordinate ?? (m_CachedCoordinate = new Point(0, 0, 0));
                }
            }
            set 
            {
                if (m_Creator.IsCreated)
                {
                    if (m_SketchMgr.ActiveSketch != Element.GetSketch())
                    {
                        throw new Exception("You must set the sketch into editing mode in order to modify the cooridinate");
                    }

                    Element.SetCoords(value.X, value.Y, value.Z);
                }
                else 
                {
                    m_CachedCoordinate = value;
                }
            }
        }

        protected override ISketchPoint CreateSketchEntity()
        {
            if (m_LinePoint == null)
            {
                return m_SketchMgr.CreatePoint(Coordinate.X, Coordinate.Y, Coordinate.Z);
            }
            else 
            {
                return m_LinePoint;
            }
        }

        internal void SetLinePoint(ISketchPoint pt) 
        {
            m_LinePoint = pt;
            Create();
        }
    }

    public abstract class SwSketchBase : SwFeature, IXSketchBase
    {
        private readonly SwSketchEntityCollection m_SwEntsColl;
        
        public ISketch Sketch => Feature?.GetSpecificFeature2() as ISketch;

        internal SwSketchBase(IModelDoc2 model, IFeature feat, bool created) : base(model, feat, created)
        {
            m_SwEntsColl = new SwSketchEntityCollection(model, this, model.SketchManager);
        }

        public IXSketchEntityCollection Entities => m_SwEntsColl;

        public bool IsEditing
        {
            get
            {
                if (IsCreated)
                {
                    return GetEditMode(Sketch);
                }
                else 
                {
                    throw new Exception("This option is only valid for the committed sketch");
                }
            }
            set 
            {
                if (IsCreated)
                {
                    SetEditMode(Sketch, value);
                }
                else
                {
                    throw new Exception("This option is only valid for the committed sketch");
                }
            }
        }

        internal bool GetEditMode(ISketch sketch) 
        {
            return m_Model.SketchManager.ActiveSketch == sketch;
        }

        internal void SetEditMode(ISketch sketch, bool isEditing)
        {
            if (isEditing)
            {
                if (!GetEditMode(sketch))
                {
                    //TODO: use API only selection
                    (sketch as IFeature).Select2(false, 0);
                    ToggleEditSketch();
                }
            }
            else
            {
                if (GetEditMode(sketch))
                {
                    ToggleEditSketch();
                }
            }
        }

        protected abstract void ToggleEditSketch();

        protected override IFeature CreateFeature()
        {
            var sketch = CreateSketch();

            m_SwEntsColl.CommitCache(sketch);

            return (IFeature)sketch;
        }

        protected abstract ISketch CreateSketch();
    }

    public class SwSketch2D : SwSketchBase, IXSketch2D
    {
        public SwSketch2D(IModelDoc2 model, IFeature feat, bool created) : base(model, feat, created)
        {
        }

        protected override ISketch CreateSketch()
        {
            //TODO: select the plane or face
            m_Model.InsertSketch2(true);
            return m_Model.SketchManager.ActiveSketch;
        }

        protected override void ToggleEditSketch()
        {
            m_Model.InsertSketch2(true);
        }
    }

    public class SwSketch3D : SwSketchBase, IXSketch3D
    {
        public SwSketch3D(IModelDoc2 model, IFeature feat, bool created) : base(model, feat, created)
        {
        }

        protected override ISketch CreateSketch()
        {
            //TODO: try to use API only selection
            m_Model.ClearSelection2(true);
            m_Model.Insert3DSketch2(true);
            return m_Model.SketchManager.ActiveSketch;
        }

        protected override void ToggleEditSketch()
        {
            m_Model.Insert3DSketch2(true);
        }
    }
}
