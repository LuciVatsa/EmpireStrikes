using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakComplexObject : MonoBehaviour
{
    private bool isFirst = false;
    private Vector3 firstVert = Vector3.zero;
    private Plane firstPlane = new Plane();

    public int numSplit = 1;
    public float breakForce = 0;
    int age;
    // Start is called before the first frame update
    void Start()
    {
        age = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        age++;
    }

    private void Break(Vector3 pnt)
    {
        var mainMesh = GetComponent<MeshFilter>().mesh;
        mainMesh.RecalculateBounds();
        var pieces = new List<Mesh>();

        
        pieces.Add(mainMesh);
        for (var i = 0; i < numSplit; i++)
        {
            //var center = mainMesh.bounds.center;
            
            var plane = new Plane(UnityEngine.Random.onUnitSphere, pnt);

            var mesh1 = new Mesh();
            var mesh2 = new Mesh();
            Split(pieces[i], mesh1, plane, true);
            Split(pieces[i], mesh2, plane, false);

            pieces.Add(mesh1);
            pieces.Add(mesh2);

        }

        for (var i = numSplit; i < pieces.Count; i++)
        {

            var piece = Instantiate(gameObject);
            piece.transform.position = transform.position;
            piece.transform.rotation = transform.rotation;
            piece.transform.localScale = transform.localScale;

 

            var renderer = piece.GetComponent<MeshRenderer>();
            renderer.materials = this.GetComponent<MeshRenderer>().materials;

            var filter = piece.GetComponent<MeshFilter>();
            filter.mesh = pieces[i];

            Destroy(piece.GetComponent<Collider>());

            var collider = piece.AddComponent<MeshCollider>();
            collider.convex = true;

            Destroy(piece.GetComponent<BreakComplexObject>());

        
            piece.GetComponent<Rigidbody>().AddForceAtPosition(mainMesh.bounds.center * breakForce, transform.position);
        }

        Destroy(gameObject);
    }



    void OnCollisionEnter(Collision coll)
    {
        if (age > 5 && coll.impactForceSum.magnitude > 0)
        {
            var localPoint = transform.InverseTransformPoint(coll.contacts[0].point);
            Break(localPoint);
        }
    }

    private void Split(Mesh mainMesh, Mesh newMesh, Plane plane, bool left)
    {
        
        List<int> newTriangles = new List<int>();
        List<Vector3> newVerts = new List<Vector3>();
        List<Vector3> newNormals = new List<Vector3>();
        List<Vector2> newUV = new List<Vector2>();


        
        var mainVerts = mainMesh.vertices;
        var mainTriangles = mainMesh.triangles;
        var mainNormals = mainMesh.normals;
        var mainUV = mainMesh.uv;



        isFirst = false;

        for (var j = 0; j < mainTriangles.Length; j = j + 3)
        {
            var leftA = (plane.GetSide(mainVerts[mainTriangles[j]]) == left) ? 1 : 0;
            var leftB = (plane.GetSide(mainVerts[mainTriangles[j + 1]]) == left) ? 1 : 0;
            var leftC = (plane.GetSide(mainVerts[mainTriangles[j + 2]]) == left) ? 1 : 0;

            var leftCount = leftA + leftB + leftC;


            if (leftCount == 0)
                continue;
         
            if (leftCount == 3){

                newTriangles.Add(newVerts.Count);
                newTriangles.Add(newVerts.Count + 1);
                newTriangles.Add(newVerts.Count + 2);

                newVerts.Add(mainVerts[mainTriangles[j]]);
                newVerts.Add(mainVerts[mainTriangles[j + 1]]);
                newVerts.Add(mainVerts[mainTriangles[j + 2]]);

                continue;
            }

            var singleIndex = leftB == leftC ? 0 : leftA == leftC ? 1 : 2;
            var single1 = mainVerts[mainTriangles[j + singleIndex]];
            var multi1 = mainVerts[mainTriangles[j + ((singleIndex + 1) % 3)]];
            var multi2 = mainVerts[mainTriangles[j + ((singleIndex + 2) % 3)]];

            Ray ray1 = new Ray(single1, multi1 - single1);
            Ray ray2 = new Ray(single1, multi2 - single1);

            plane.Raycast(ray1, out var intersect1);
            plane.Raycast(ray2, out var intersect2);


            var intvert1 = ray1.origin + ray1.direction.normalized * intersect1;
            var intvert2 = ray2.origin + ray2.direction.normalized * intersect2;

            if (!isFirst)
            {
                isFirst = true;
                firstVert = intvert1;
            }
            else
            {
                firstPlane.Set3Points(firstVert, intvert1, intvert2);

                var normal = left ? plane.normal * -1f : plane.normal;
                newTriangles.Add(newVerts.Count);
                newTriangles.Add(newVerts.Count + 1);
                newTriangles.Add(newVerts.Count + 2);

                newVerts.Add(firstVert);
                newVerts.Add(firstPlane.GetSide(firstVert + normal) ? intvert1 : intvert2);
                newVerts.Add(firstPlane.GetSide(firstVert + normal) ? intvert2 : intvert1);
          
            }


            if (leftCount == 1)
            {

                newTriangles.Add(newVerts.Count);
                newTriangles.Add(newVerts.Count + 1);
                newTriangles.Add(newVerts.Count + 2);

                newVerts.Add(single1);
                newVerts.Add(intvert1);
                newVerts.Add(intvert2);
                                                                           
                continue;
            }

            if (leftCount == 2)
            {

                newTriangles.Add(newVerts.Count);
                newTriangles.Add(newVerts.Count + 1);
                newTriangles.Add(newVerts.Count + 2);
                newTriangles.Add(newVerts.Count + 3);
                newTriangles.Add(newVerts.Count + 4);
                newTriangles.Add(newVerts.Count + 5);

                newVerts.Add(intvert1);
                newVerts.Add(multi1);
                newVerts.Add(multi2);
                newVerts.Add(intvert1);
                newVerts.Add(multi2);
                newVerts.Add(intvert2);


                                                                                 
                continue;
            }
        }
        newMesh.vertices = newVerts.ToArray();
        newMesh.SetTriangles(newTriangles, 0, true);
        newMesh.RecalculateNormals();
    }



}