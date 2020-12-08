
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    public class DelGen
    {

        int top = -1;
        IList<Vector2> verts;

        List<int> indices;
        List<TVert> triangles;

        public DelGen()
        {
            triangles = new List<TVert>();
            indices = new List<int>();
        }


        public DelTri Triangulate(IList<Vector2> verts)
        {
            DelTri result = null;
            Triangulate(verts, ref result);

            return result;
        }

        public void Triangulate(IList<Vector2> verts, ref DelTri result)
        {
            if (verts == null)
            {
                throw new ArgumentNullException("");
            }
            if (verts.Count < 3)
            {
                throw new ArgumentException("");
            }

            triangles.Clear();
            this.verts = verts;

            top = 0;

            for (int i = 0; i < verts.Count; i++)
            {
                if (Higher(top, i))
                {
                    top = i;
                }
            }

            triangles.Add(new TVert(-2, -1, top));

        for (int i = 0; i < verts.Count; i++)
        {
           
            var pi = i;

            if (pi == top) continue;

        
            var ti = FindTVert(pi);

            var t = triangles[ti];

            
            var p0 = t.P0;
            var p1 = t.P1;
            var p2 = t.P2;

         
            var nti0 = triangles.Count;
            var nti1 = nti0 + 1;
            var nti2 = nti0 + 2;

            var nt0 = new TVert(pi, p0, p1);
            var nt1 = new TVert(pi, p1, p2);
            var nt2 = new TVert(pi, p2, p0);


            nt0.A0 = t.A2;
            nt1.A0 = t.A0;
            nt2.A0 = t.A1;

            nt0.A1 = nti1;
            nt1.A1 = nti2;
            nt2.A1 = nti0;

            nt0.A2 = nti2;
            nt1.A2 = nti0;
            nt2.A2 = nti1;

          
            t.C0 = nti0;
            t.C1 = nti1;
            t.C2 = nti2;

            triangles[ti] = t;

            triangles.Add(nt0);
            triangles.Add(nt1);
            triangles.Add(nt2);

            if (nt0.A0 != -1) EdgeFix(nti0, nt0.A0, pi, p0, p1);
            if (nt1.A0 != -1) EdgeFix(nti1, nt1.A0, pi, p1, p2);
            if (nt2.A0 != -1) EdgeFix(nti2, nt2.A0, pi, p2, p0);
        }
        if (result == null)
        {
            result = new DelTri();
        }

        result.Clear();

        for (int i = 0; i < verts.Count; i++)
        {
            result.Vertices.Add(verts[i]);
        }

        for (int i = 1; i < triangles.Count; i++)
        {
            var t = triangles[i];

            if (t.IsLeaf && t.IsInner)
            {
                result.Triangles.Add(t.P0);
                result.Triangles.Add(t.P1);
                result.Triangles.Add(t.P2);
            }
        }

        this.verts = null;
        }

        bool Higher(int pi0, int pi1)
        {
            if (pi0 == -2)
            {
                return false;
            }
            else if (pi0 == -1)
            {
                return true;
            }
            else if (pi1 == -2)
            {
                return true;
            }
            else if (pi1 == -1)
            {
                return false;
            }
            else
            {
                var p0 = verts[pi0];
                var p1 = verts[pi1];

                if (p0.y < p1.y)
                {
                    return true;
                }
                else if (p0.y > p1.y)
                {
                    return false;
                }
                else
                {
                    return p0.x < p1.x;
                }
            }
        }






        void EdgeFix(int ti0, int ti1, int pi, int li0, int li1)
        {


            Debug.Assert(triangles[ti1].HasEdge(li0, li1));

            while (!triangles[ti1].IsLeaf)
            {
                var t = triangles[ti1];

                if (t.C0 != -1 && triangles[t.C0].HasEdge(li0, li1))
                {
                ti1 = t.C0;
                }
                else if (t.C1 != -1 && triangles[t.C1].HasEdge(li0, li1))
                {
                ti1 = t.C1;
                }
                else if (t.C2 != -1 && triangles[t.C2].HasEdge(li0, li1))
                {
                ti1 = t.C2;
                }
                else
                {
                    Debug.Assert(false);
                    throw new System.Exception("");
                }
            }




        var t0 = triangles[ti0];
            var t1 = triangles[ti1];
            var qi = t1.OtherPoint(li0, li1);

            Debug.Assert(t0.HasEdge(li0, li1));
            Debug.Assert(t1.HasEdge(li0, li1));
            Debug.Assert(t0.IsLeaf);
            Debug.Assert(t1.IsLeaf);
            Debug.Assert(t0.P0 == pi || t0.P1 == pi || t0.P2 == pi);
            Debug.Assert(t1.P0 == qi || t1.P1 == qi || t1.P2 == qi);





        bool girsinmi;


        Debug.Assert(pi != top && pi >= 0);

        var lMagic = qi < 0;
        var iMagic = li0 < 0;
        var jMagic = li1 < 0;

        Debug.Assert(!(iMagic && jMagic));

        if (lMagic)
        {
            girsinmi =  true;
        }
        else if (iMagic)
        {
            Debug.Assert(!jMagic);

            var p = verts[qi];
            var l0 = verts[pi];
            var l1 = verts[li1];

            girsinmi =  GeomFuncs.ToTheLeft(p, l0, l1);
        }
        else if (jMagic)
        {
            Debug.Assert(!iMagic);

            var p = verts[qi];
            var l0 = verts[pi];
            var l1 = verts[li0];

            girsinmi = !GeomFuncs.ToTheLeft(p, l0, l1);
        }
        else
        {
            Debug.Assert(pi >= 0 && qi >= 0 && li0 >= 0 && li1 >= 0);

            var p = verts[qi];
            var c0 = verts[pi];
            var c1 = verts[li0];
            var c2 = verts[li1];

            Debug.Assert(GeomFuncs.ToTheLeft(c2, c0, c1));
            Debug.Assert(GeomFuncs.ToTheLeft(c2, c1, p));


            var ax = c0.x - p.x;
            var ay = c0.y - p.y;
            var bx = c1.x - p.x;
            var by = c1.y - p.y;
            var cx = c2.x - p.x;
            var cy = c2.y - p.y;

            girsinmi = (
                    (ax * ax + ay * ay) * (bx * cy - cx * by) -
                    (bx * bx + by * by) * (ax * cy - cx * ay) +
                    (cx * cx + cy * cy) * (ax * by - bx * ay)
            ) <= 0.000001f;




        }



        if (!girsinmi)
            {
                var ti2 = triangles.Count;
                var ti3 = ti2 + 1;

                var t2 = new TVert(pi, li0, qi);
                var t3 = new TVert(pi, qi, li1);

                t2.A0 = t1.Opposite(li1);
                t2.A1 = ti3;
                t2.A2 = t0.Opposite(li1);

                t3.A0 = t1.Opposite(li0);
                t3.A1 = t0.Opposite(li0);
                t3.A2 = ti2;

                triangles.Add(t2);
                triangles.Add(t3);

                var nt0 = triangles[ti0];
                var nt1 = triangles[ti1];

                nt0.C0 = ti2;
                nt0.C1 = ti3;

                nt1.C0 = ti2;
                nt1.C1 = ti3;

                triangles[ti0] = nt0;
                triangles[ti1] = nt1;

                if (t2.A0 != -1) EdgeFix(ti2, t2.A0, pi, li0, qi);
                if (t3.A0 != -1) EdgeFix(ti3, t3.A0, pi, qi, li1);
            }
        }

        int FindTVert(int pi)
        {
            var curr = 0;

            while (!triangles[curr].IsLeaf)
            {
                var t = triangles[curr];

                if (t.C0 >= 0 && PointInTriangle(pi, t.C0))
                {
                    curr = t.C0;
                }
                else if (t.C1 >= 0 && PointInTriangle(pi, t.C1))
                {
                    curr = t.C1;
                }
                else
                {
                    curr = t.C2;
                }
            }

            return curr;
        }

        bool PointInTriangle(int pi, int ti)
        {
            var t = triangles[ti];
            return ToTheLeft(pi, t.P0, t.P1)
                && ToTheLeft(pi, t.P1, t.P2)
                && ToTheLeft(pi, t.P2, t.P0);
        }

        bool ToTheLeft(int pi, int li0, int li1)
        {
            if (li0 == -2)
            {
                return Higher(li1, pi);
            }
            else if (li0 == -1)
            {
                return Higher(pi, li1);
            }
            else if (li1 == -2)
            {
                return Higher(pi, li0);
            }
            else if (li1 == -1)
            {
                return Higher(li0, pi);
            }
            else
            {
                Debug.Assert(li0 >= 0);
                Debug.Assert(li1 >= 0);

                return GeomFuncs.ToTheLeft(verts[pi], verts[li0], verts[li1]);
            }
        }

        struct TVert
        {
   
            public int P0;
            public int P1;
            public int P2;

            public int C0;
            public int C1;
            public int C2;

            public int A0;
            public int A1;
            public int A2;


            public bool IsLeaf
            {
                get
                {
                    return C0 < 0 && C1 < 0 && C2 < 0;
                }
            }

            public bool IsInner
            {
                get
                {
                    return P0 >= 0 && P1 >= 0 && P2 >= 0;
                }
            }

            public TVert(int P0, int P1, int P2)
            {
                this.P0 = P0;
                this.P1 = P1;
                this.P2 = P2;

                this.C0 = -1;
                this.C1 = -1;
                this.C2 = -1;

                this.A0 = -1;
                this.A1 = -1;
                this.A2 = -1;
            }

            public bool HasEdge(int e0, int e1)
            {
                if (e0 == P0)
                {
                    return e1 == P1 || e1 == P2;
                }
                else if (e0 == P1)
                {
                    return e1 == P0 || e1 == P2;
                }
                else if (e0 == P2)
                {
                    return e1 == P0 || e1 == P1;
                }

                return false;
            }


            public int OtherPoint(int p0, int p1)
            {
                if (p0 == P0)
                {
                    if (p1 == P1) return P2;
                    if (p1 == P2) return P1;
                    throw new ArgumentException("");
                }
                if (p0 == P1)
                {
                    if (p1 == P0) return P2;
                    if (p1 == P2) return P0;
                    throw new ArgumentException("");
                }
                if (p0 == P2)
                {
                    if (p1 == P0) return P1;
                    if (p1 == P1) return P0;
                    throw new ArgumentException("");
                }

                throw new ArgumentException("");
            }


            public int Opposite(int p)
            {
                if (p == P0) return A0;
                if (p == P1) return A1;
                if (p == P2) return A2;
                throw new ArgumentException("");
            }

 
            public override string ToString()
            {
                if (IsLeaf)
                {
                    return string.Format("", P0, P1, P2);
                }
                else
                {
                    return string.Format("", P0, P1, P2, C0, C1, C2);
                }
            }
        }
    }







    public class DelTri
    {

        public readonly List<Vector2> Vertices;


        public readonly List<int> Triangles;

        internal DelTri()
        {
            Vertices = new List<Vector2>();
            Triangles = new List<int>();
        }

        internal void Clear()
        {
            Vertices.Clear();
            Triangles.Clear();
        }

        public bool Verify()
        {
            try
            {
                for (int i = 0; i < Triangles.Count; i += 3)
                {
                    var c0 = Vertices[Triangles[i]];
                    var c1 = Vertices[Triangles[i + 1]];
                    var c2 = Vertices[Triangles[i + 2]];

                    for (int j = 0; j < Vertices.Count; j++)
                    {
                        var p = Vertices[j];
                        var ax = c0.x - p.x;
                        var ay = c0.y - p.y;
                        var bx = c1.x - p.x;
                        var by = c1.y - p.y;
                        var cx = c2.x - p.x;
                        var cy = c2.y - p.y;
                    if ((
                                (ax * ax + ay * ay) * (bx * cy - cx * by) -
                                (bx * bx + by * by) * (ax * cy - cx * ay) +
                                (cx * cx + cy * cy) * (ax * by - bx * ay)
                        ) > 0.000001f)
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }






    public class GeomFuncs: MonoBehaviour
    {



        public static bool ToTheLeft(Vector2 p, Vector2 l0, Vector2 l1)
        {
            return ((l1.x - l0.x) * (p.y - l0.y) - (l1.y - l0.y) * (p.x - l0.x)) >= 0;
        }

        public static bool ToTheRight(Vector2 p, Vector2 l0, Vector2 l1)
        {
            return !ToTheLeft(p, l0, l1);
        }


        public static bool PointInTriangle(Vector2 p, Vector2 c0, Vector2 c1, Vector2 c2)
        {
            return ToTheLeft(p, c0, c1)
                && ToTheLeft(p, c1, c2)
                && ToTheLeft(p, c2, c0);
        }
       
        public static Vector2 RotateRightAngle(Vector2 v)
        {
            var x = v.x;
            v.x = -v.y;
            v.y = x;

            return v;
        }

        public static bool LineLineIntersection(Vector2 p0, Vector2 v0, Vector2 p1, Vector2 v1, out float m0, out float m1)
        {
            var det = (v0.x * v1.y - v0.y * v1.x);

            if (Mathf.Abs(det) < 0.001f)
            {
                m0 = float.NaN;
                m1 = float.NaN;

                return false;
            }
            else
            {
                m0 = ((p0.y - p1.y) * v1.x - (p0.x - p1.x) * v1.y) / det;

                if (Mathf.Abs(v1.x) >= 0.001f)
                {
                    m1 = (p0.x + m0 * v0.x - p1.x) / v1.x;
                }
                else
                {
                    m1 = (p0.y + m0 * v0.y - p1.y) / v1.y;
                }

                return true;
            }
        }

        public static Vector2 LineLineIntersection(Vector2 p0, Vector2 v0, Vector2 p1, Vector2 v1)
        {
            float m0, m1;

            if (LineLineIntersection(p0, v0, p1, v1, out m0, out m1))
            {
                return p0 + m0 * v0;
            }
            else
            {
                return new Vector2(float.NaN, float.NaN);
            }
        }


        public static float Area(IList<Vector2> polygon)
        {
            var area = 0.0f;

            var count = polygon.Count;

            for (int i = 0; i < count; i++)
            {
                var j = (i == count - 1) ? 0 : (i + 1);

                var p0 = polygon[i];
                var p1 = polygon[j];

                area += p0.x * p1.y - p1.y * p1.x;
            }

            return 0.5f * area;
        }
    }





    public class VoronoiDiagram
    {

        public readonly DelTri Triangulation;


        public readonly List<Vector2> Sites;


        public readonly List<Vector2> Vertices;


        public readonly List<Edge> Edges;

        public readonly List<int> FirstEdgeBySite;

        internal VoronoiDiagram()
        {
            Triangulation = new DelTri();
            Sites = Triangulation.Vertices;
            Vertices = new List<Vector2>();
            Edges = new List<Edge>();
            FirstEdgeBySite = new List<int>();
        }

        internal void Clear()
        {
            Triangulation.Clear();
            Sites.Clear();
            Vertices.Clear();
            Edges.Clear();
            FirstEdgeBySite.Clear();
        }


        public enum EdgeType
        {
            Line,
            RayCCW,
            RayCW,
            Segment
        }

        public struct Edge
        {


            readonly public EdgeType Type;


            readonly public int Site;


            readonly public int Vert0;


            readonly public int Vert1;

            public Vector2 Direction;

            public Edge(EdgeType type, int site, int vert0, int vert1, Vector2 direction)
            {
                this.Type = type;
                this.Site = site;
                this.Vert0 = vert0;
                this.Vert1 = vert1;
                this.Direction = direction;
            }


            public override string ToString()
            {
                if (Type == EdgeType.Segment)
                {
                    return string.Format("VoronoiEdge(Segment, {0}, {1}, {2})",
                            Site, Vert0, Vert1);
                }
                else if (Type == EdgeType.Segment)
                {
                    return string.Format("VoronoiEdge(Line, {0}, {1}, {2})",
                            Site, Vert0, Direction);
                }
                else
                {
                    return string.Format("VoronoiEdge(Ray, {0}, {1}, ({2}, {3}))",
                            Site, Vert0, Direction.x, Direction.y);
                }
            }
        }
    }


    public class BreakFlat : MonoBehaviour
    {

        public MeshFilter Filter { get; private set; }
        public MeshRenderer Renderer { get; private set; }
        public MeshCollider Collider { get; private set; }
        public Rigidbody Rigidbody { get; private set; }

        public List<Vector2> Polygon;
        public float Thickness = 1.0f;
        public float MinBreakArea = 0.01f;
        public float MinImpactToBreak = 50.0f;

        float _Area = -1.0f;

        int age;

        public float Area
        {
            get
            {
                if (_Area < 0.0f)
                {
                    _Area = GeomFuncs.Area(Polygon);
                }

                return _Area;
            }
        }

        void Start()
        {
        Debug.Log("Basladi");
        age = 0;

            Reload();
        }

        public void Reload()
        {
            var pos = transform.position;

            if (Filter == null) Filter = GetComponent<MeshFilter>();
            if (Renderer == null) Renderer = GetComponent<MeshRenderer>();
            if (Collider == null) Collider = GetComponent<MeshCollider>();
            if (Rigidbody == null) Rigidbody = GetComponent<Rigidbody>();

            if (Polygon.Count == 0)
            {
               
                var scale = 0.5f * transform.localScale;

                Polygon.Add(new Vector2(-scale.x, -scale.y));
                Polygon.Add(new Vector2(scale.x, -scale.y));
                Polygon.Add(new Vector2(scale.x, scale.y));
                Polygon.Add(new Vector2(-scale.x, scale.y));

                Thickness = 2.0f * scale.z;

                transform.localScale = Vector3.one;
            }

            var mesh = MeshFromPolygon(Polygon, Thickness);

            Filter.sharedMesh = mesh;
            Collider.sharedMesh = mesh;
        }

        void FixedUpdate()
        {
            var pos = transform.position;

            age++;
            if (pos.magnitude > 1000.0f)
            {
                DestroyImmediate(gameObject);
            }
        }

        void OnCollisionEnter(Collision coll)
        {
            Debug.Log("coll.impactForceSum.magnitude");
            Debug.Log(coll.impactForceSum.magnitude);
            if (age > 5 && coll.impactForceSum.magnitude > MinImpactToBreak)
                {
                    Debug.Log("GIRdis");
                    var pnt = coll.contacts[0].point;
                    Break((Vector2)transform.InverseTransformPoint(pnt));
                }
        }

        static float NormalizedRandom(float mean, float stddev)
        {
            var u1 = UnityEngine.Random.value;
            var u2 = UnityEngine.Random.value;

            var randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                Mathf.Sin(2.0f * Mathf.PI * u2);

            return mean + stddev * randStdNormal;
        }

        public void Break(Vector2 position)
        {
            var area = Area;
            if (area > MinBreakArea)
            {
                DelGen delCalc = new DelGen();
                PCmp cmp = new PCmp();
                List<PT> pts = new List<PT>(); ;
                       

                //var clip = new VoronoiClipper();

                var sites = new Vector2[10];

                for (int i = 0; i < sites.Length; i++)
                {
                    var dist = Mathf.Abs(NormalizedRandom(0.5f, 1.0f / 2.0f));
                    var angle = 2.0f * Mathf.PI * UnityEngine.Random.value;

                    sites[i] = position + new Vector2(
                            dist * Mathf.Cos(angle),
                            dist * Mathf.Sin(angle));
                }


                VoronoiDiagram diagram = null;


                if (diagram == null)
                {
                diagram = new VoronoiDiagram();
                }

                var trig = diagram.Triangulation;

                diagram.Clear();

         
                delCalc.Triangulate(sites, ref trig);
  

                pts.Clear();

                var verts = trig.Vertices;
                var tris = trig.Triangles;
                var centers = diagram.Vertices;
                var edges = diagram.Edges;


                if (tris.Count > pts.Capacity) { pts.Capacity = tris.Count; }
                if (tris.Count > edges.Capacity) { edges.Capacity = tris.Count; }


                for (int ti = 0; ti < tris.Count; ti += 3)
                {
                    var p0 = verts[tris[ti]];
                    var p1 = verts[tris[ti + 1]];
                    var p2 = verts[tris[ti + 2]];

               
                    Debug.Assert(GeomFuncs.ToTheLeft(p2, p0, p1));

                    

                                             


                    var mp0 = 0.5f * (p0 + p1);
                    var mp1 = 0.5f * (p1 + p2);
                    var v0 = (p0 - p1);
                    var v1 = (p1 - p2);
                    var x = v0.x;
                    v0.x = -v0.y;
                    v0.y = x;

                    var x1 = v1.x;
                    v1.x = -v1.y;
                    v1.y = x1;


                    float m0, m1;

                    GeomFuncs.LineLineIntersection(mp0, v0, mp1, v1, out m0, out m1);

                    var center =  mp0 + m0 * v0;
                    centers.Add(center);

            }


                for (int ti = 0; ti < tris.Count; ti += 3)
                {
                    pts.Add(new PT(tris[ti], ti));
                    pts.Add(new PT(tris[ti + 1], ti));
                    pts.Add(new PT(tris[ti + 2], ti));
                }

                cmp.tris = tris;
                cmp.verts = verts;

            
                pts.Sort(cmp);
                


                cmp.tris = null;
                cmp.verts = null;

                for (int i = 0; i < pts.Count; i++)
                {
                diagram.FirstEdgeBySite.Add(edges.Count);

                    var start = i;
                    var end = -1;

                    for (int j = i + 1; j < pts.Count; j++)
                    {
                        if (pts[i].Point != pts[j].Point)
                        {
                            end = j - 1;
                            break;
                        }
                    }

                    if (end == -1)
                    {
                        end = pts.Count - 1;
                    }

                    i = end;

                    var count = end - start;

                    Debug.Assert(count >= 0);

                    for (int ptiCurr = start; ptiCurr <= end; ptiCurr++)
                    {
                        bool isEdge;

                        var ptiNext = ptiCurr + 1;

                        if (ptiNext > end) ptiNext = start;

                        var ptCurr = pts[ptiCurr];
                        var ptNext = pts[ptiNext];

                        var tiCurr = ptCurr.Triangle;
                        var tiNext = ptNext.Triangle;

                        var p0 = verts[ptCurr.Point];

                        var v2nan = new Vector2(float.NaN, float.NaN);

                        if (count == 0)
                        {
                            isEdge = true;
                        }
                        else if (count == 1)
                        {


                        var cCurr = (1.0f / 3.0f) * (verts[tris[tiCurr]] + verts[tris[tiCurr + 1]] + verts[tris[tiCurr + 2]]);
                            var cNext = (1.0f / 3.0f) * (verts[tris[tiNext]] + verts[tris[tiNext + 1]] + verts[tris[tiNext + 2]]);

                            isEdge = GeomFuncs.ToTheLeft(cCurr, p0, cNext);
                        }
                        else
                        {
                            var x0 = tris[tiCurr];
                            var x1 = tris[tiCurr + 1];
                            var x2 = tris[tiCurr + 2];

                            var y0 = tris[tiNext];
                            var y1 = tris[tiNext + 1];
                            var y2 = tris[tiNext + 2];

                            var n = 0;

                            if (x0 == y0 || x0 == y1 || x0 == y2) n++;
                            if (x1 == y0 || x1 == y1 || x1 == y2) n++;
                            if (x2 == y0 || x2 == y1 || x2 == y2) n++;

                            Debug.Assert(n != 3);

                            isEdge = n < 2;



                        }

                        if (isEdge)
                        {
                            Vector2 v0, v1;

                            if (ptCurr.Point == tris[tiCurr])
                            {
                                v0 = verts[tris[tiCurr + 2]] - verts[tris[tiCurr + 0]];
                            }
                            else if (ptCurr.Point == tris[tiCurr + 1])
                            {
                                v0 = verts[tris[tiCurr + 0]] - verts[tris[tiCurr + 1]];
                            }
                            else
                            {
                                Debug.Assert(ptCurr.Point == tris[tiCurr + 2]);
                                v0 = verts[tris[tiCurr + 1]] - verts[tris[tiCurr + 2]];
                            }

                            if (ptNext.Point == tris[tiNext])
                            {
                                v1 = verts[tris[tiNext + 0]] - verts[tris[tiNext + 1]];
                            }
                            else if (ptNext.Point == tris[tiNext + 1])
                            {
                                v1 = verts[tris[tiNext + 1]] - verts[tris[tiNext + 2]];
                            }
                            else
                            {
                                Debug.Assert(ptNext.Point == tris[tiNext + 2]);
                                v1 = verts[tris[tiNext + 2]] - verts[tris[tiNext + 0]];
                            }




                            edges.Add(new VoronoiDiagram.Edge(
                                VoronoiDiagram.EdgeType.RayCCW,
                                ptCurr.Point,
                                tiCurr / 3,
                                -1,
                                GeomFuncs.RotateRightAngle(v0)
                            ));

                            edges.Add(new VoronoiDiagram.Edge(
                                VoronoiDiagram.EdgeType.RayCW,
                                ptCurr.Point,
                                tiNext / 3,
                                -1,
                                GeomFuncs.RotateRightAngle(v1)
                            ));
                        }
                        else
                        {

                            if ((centers[tiCurr / 3] - centers[tiNext / 3]).magnitude >= 0.000001f)
                            {
                                edges.Add(new VoronoiDiagram.Edge(
                                    VoronoiDiagram.EdgeType.Segment,
                                    ptCurr.Point,
                                    tiCurr / 3,
                                    tiNext / 3,
                                    v2nan
                                ));
                            }
                        }
                    }
                }
            


            var clipped = new List<Vector2>();
            List<Vector2> inVertices = new List<Vector2>();
            List<Vector2> outVertices = new List<Vector2>();
            for (int i = 0; i < sites.Length; i++)
                {
                    int site = i;

                    inVertices.Clear();

                    inVertices.AddRange(Polygon);

                    int firstEdge, lastEdge;

                    if (site == diagram.Sites.Count - 1)
                    {
                        firstEdge = diagram.FirstEdgeBySite[site];
                        lastEdge = diagram.Edges.Count - 1;
                    }
                    else
                    {
                        firstEdge = diagram.FirstEdgeBySite[site];
                        lastEdge = diagram.FirstEdgeBySite[site + 1] - 1;
                    }

                    for (int ei = firstEdge; ei <= lastEdge; ei++)
                    {
                        outVertices.Clear();

                        var edge = diagram.Edges[ei];

                        Vector2 lp, ld;

                        if (edge.Type == VoronoiDiagram.EdgeType.RayCCW || edge.Type == VoronoiDiagram.EdgeType.RayCW)
                        {
                            lp = diagram.Vertices[edge.Vert0];
                            ld = edge.Direction;

                            if (edge.Type == VoronoiDiagram.EdgeType.RayCW)
                            {
                                ld *= -1;
                            }
                        }
                        else if (edge.Type == VoronoiDiagram.EdgeType.Segment)
                        {
                            var lp0 = diagram.Vertices[edge.Vert0];
                            var lp1 = diagram.Vertices[edge.Vert1];

                            lp = lp0;
                            ld = lp1 - lp0;
                        }
                        else if (edge.Type == VoronoiDiagram.EdgeType.Line)
                        {
                            throw new NotSupportedException("");
                        }
                        else
                        {
                            Debug.Assert(false);
                            return;
                        }

                        for (int pi0 = 0; pi0 < inVertices.Count; pi0++)
                        {
                            var pi1 = pi0 == inVertices.Count - 1 ? 0 : pi0 + 1;

                            var p0 = inVertices[pi0];
                            var p1 = inVertices[pi1];

                            var p0Inside = GeomFuncs.ToTheLeft(p0, lp, lp + ld);
                            var p1Inside = GeomFuncs.ToTheLeft(p1, lp, lp + ld);

                            if (p0Inside && p1Inside)
                            {
                                outVertices.Add(p1);
                            }
                            else if (!p0Inside && !p1Inside)
                            {
                   
                            }
                            else
                            {
                                var intersection = GeomFuncs.LineLineIntersection(lp, ld.normalized, p0, (p1 - p0).normalized);

                                if (p0Inside)
                                {
                                    outVertices.Add(intersection);
                                }
                                else if (p1Inside)
                                {
                                    outVertices.Add(intersection);
                                    outVertices.Add(p1);
                                }
                                else
                                {
                                    Debug.Assert(false);
                                }
                            }
                        }

                        var tmp = inVertices;
                        inVertices = outVertices;
                        outVertices = tmp;
                    }

                    if (clipped == null)
                    {
                        clipped = new List<Vector2>();
                    }
                    else
                    {
                        clipped.Clear();
                    }

                    clipped.AddRange(inVertices);
            
            if (clipped.Count > 0)
                    {
                        var newGo = Instantiate(gameObject, transform.parent);

                        newGo.transform.localPosition = transform.localPosition;
                        newGo.transform.localRotation = transform.localRotation;

                        var bs = newGo.GetComponent<BreakFlat>();

                        bs.Thickness = Thickness;
                        bs.Polygon.Clear();
                        bs.Polygon.AddRange(clipped);

                        var childArea = bs.Area;

                        var rb = bs.GetComponent<Rigidbody>();

                        rb.mass = Rigidbody.mass * (childArea / area);
                    }
                }

                gameObject.active = false;
                Destroy(gameObject);
            }
        }

        static Mesh MeshFromPolygon(List<Vector2> polygon, float thickness)
        {
            var count = polygon.Count;
            
            var verts = new Vector3[6 * count];
            var norms = new Vector3[6 * count];
            var tris = new int[3 * (4 * count - 4)];
            

            var vi = 0;
            var ni = 0;
            var ti = 0;

            var ext = 0.5f * thickness;

            
            for (int i = 0; i < count; i++)
            {
                verts[vi++] = new Vector3(polygon[i].x, polygon[i].y, ext);
                norms[ni++] = Vector3.forward;
            }

           
            for (int i = 0; i < count; i++)
            {
                verts[vi++] = new Vector3(polygon[i].x, polygon[i].y, -ext);
                norms[ni++] = Vector3.back;
            }

           
            for (int i = 0; i < count; i++)
            {
                var iNext = i == count - 1 ? 0 : i + 1;

                verts[vi++] = new Vector3(polygon[i].x, polygon[i].y, ext);
                verts[vi++] = new Vector3(polygon[i].x, polygon[i].y, -ext);
                verts[vi++] = new Vector3(polygon[iNext].x, polygon[iNext].y, -ext);
                verts[vi++] = new Vector3(polygon[iNext].x, polygon[iNext].y, ext);

                var norm = Vector3.Cross(polygon[iNext] - polygon[i], Vector3.forward).normalized;

                norms[ni++] = norm;
                norms[ni++] = norm;
                norms[ni++] = norm;
                norms[ni++] = norm;
            }


            for (int vert = 2; vert < count; vert++)
            {
                tris[ti++] = 0;
                tris[ti++] = vert - 1;
                tris[ti++] = vert;
            }

            for (int vert = 2; vert < count; vert++)
            {
                tris[ti++] = count;
                tris[ti++] = count + vert;
                tris[ti++] = count + vert - 1;
            }

            for (int vert = 0; vert < count; vert++)
            {
                var si = 2 * count + 4 * vert;

                tris[ti++] = si;
                tris[ti++] = si + 1;
                tris[ti++] = si + 2;

                tris[ti++] = si;
                tris[ti++] = si + 2;
                tris[ti++] = si + 3;
            }

            Debug.Assert(ti == tris.Length);
            Debug.Assert(vi == verts.Length);

            var mesh = new Mesh();


            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.normals = norms;

            return mesh;
        }




    struct PT
    {
        public readonly int Point;
        public readonly int Triangle;

        public PT(int point, int triangle)
        {
            this.Point = point;
            this.Triangle = triangle;
        }


    }


    class PCmp : IComparer<PT>
    {
        public List<Vector2> verts;
        public List<int> tris;

        public int Compare(PT pt0, PT pt1)
        {
            if (pt0.Point < pt1.Point)
            {
                return -1;
            }
            else if (pt0.Point > pt1.Point)
            {
                return 1;
            }
            else if (pt0.Triangle == pt1.Triangle)
            {
                Debug.Assert(pt0.Point == pt1.Point);
                return 0;
            }
            else
            {
                Debug.Assert(pt0.Point == pt1.Point);


                var rp = verts[pt0.Point];


                var p0 = (1.0f / 3.0f) * (verts[tris[pt0.Triangle]]+ verts[tris[pt0.Triangle + 1]]+ verts[tris[pt0.Triangle + 2]]) - rp;
                var p1 = (1.0f / 3.0f) * (verts[tris[pt1.Triangle]]+ verts[tris[pt1.Triangle + 1]]+ verts[tris[pt1.Triangle + 2]]) - rp;


                var q0 = ((p0.y < 0) || ((p0.y == 0) && (p0.x < 0)));
                var q1 = ((p1.y < 0) || ((p1.y == 0) && (p1.y < 0)));

                if (q0 == q1)
                {

                    var cp = p0.x * p1.y - p0.y * p1.x;

                    if (cp > 0)
                    {
                        return -1;
                    }
                    else if (cp < 0)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {

                    return q1 ? -1 : 1;
                }
            }
        }


    }








}
