using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Wire_Behaviour : MonoBehaviour
{
    //[Range(1, 10)]
    //[SerializeField] private int totalRectangles = 1;
    //
    //[Range(0, 10f)]
    //[SerializeField] private float scaleRectangles = 1f;

    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector3[] normals;

    // -------------------------

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateMesh();
        UpdateMesh();
    }

    private void CreateMesh()
    {
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)
        };

        triangles = new int[]
        {
            0, 1, 2,
            1, 3, 2
        };

        normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
    }
}
