using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using DaydreamElements.SwipeMenu;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class TriMaker : MonoBehaviour
{

    public GameObject controller;
    private GameObject[] controllers;
    private Mesh mesh;
    private Vector3[] initialVerts;
    private Vector3[] vertices;
    private int[] triangles;
    private int[] initialTris;
    private Vector2 lasttouchPos;
    private Vector2 deltaTouch;
    private float zoomSpeed = 2f;
    private Vector3 offset;
    float delta = 4f;
    private Vector3 rot;
    private ActionSwipe action;

    // Use this for initialization
    void Start()
    {
        InputTracking.disablePositionalTracking = true;
        initialVerts = new Vector3[8];
        initialTris = new int[36];
        lasttouchPos = new Vector2(0, 0);
        delta = 4f;
        action = GameObject.Find("ActionSwipe").GetComponent<ActionSwipe>();
        makeCube();
        transform.position = new Vector3(0, 0, 4f);
        transform.localRotation = Quaternion.Euler(0, 45, 0);
    }

    // Update is called once per frame
    void Update()
    {
        getHandleChange();
        //transform.Rotate(Camera.main.transform.localRotation.eulerAngles, Space.Self);
        //transform.localRotation = Camera.main.transform.localRotation;
    }

    private void updateRotation()
    {
        if (GvrController.Recentered)
            rot = new Vector3(0,0,0);
        
        transform.Rotate(rot, Space.Self);
    }

    private void getHandleChange()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = controllers[initialTris[i]].transform.localPosition;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    private void makeCube()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "EditaableCube";
        controllers = new GameObject[8];

        controllers[0] = Instantiate(controller, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        controllers[0].transform.parent = transform;
        controllers[1] = Instantiate(controller, new Vector3(1, 0, 0), Quaternion.Euler(0, 0, 0));
        controllers[1].transform.parent = transform;
        controllers[2] = Instantiate(controller, new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
        controllers[2].transform.parent = transform;
        controllers[3] = Instantiate(controller, new Vector3(1, 1, 0), Quaternion.Euler(0, 0, 0));
        controllers[3].transform.parent = transform;

        controllers[4] = Instantiate(controller, new Vector3(0, 0, 1), Quaternion.Euler(0, 0, 0));
        controllers[4].transform.parent = transform;
        controllers[5] = Instantiate(controller, new Vector3(1, 0, 1), Quaternion.Euler(0, 0, 0));
        controllers[5].transform.parent = transform;
        controllers[6] = Instantiate(controller, new Vector3(0, 1, 1), Quaternion.Euler(0, 0, 0));
        controllers[6].transform.parent = transform;
        controllers[7] = Instantiate(controller, new Vector3(1, 1, 1), Quaternion.Euler(0, 0, 0));
        controllers[7].transform.parent = transform;

        for (int i = 0; i < initialVerts.Length; i++)
        {
            initialVerts[i] = controllers[i].transform.localPosition;
        }

        initialTris[0] = 0;
        initialTris[1] = 2;
        initialTris[2] = 1;

        initialTris[3] = 2;
        initialTris[4] = 3;
        initialTris[5] = 1;

        initialTris[6] = 1;
        initialTris[7] = 3;
        initialTris[8] = 5;

        initialTris[9] = 3;
        initialTris[10] = 7;
        initialTris[11] = 5;

        initialTris[12] = 2;
        initialTris[13] = 6;
        initialTris[14] = 7;

        initialTris[15] = 6;
        initialTris[16] = 2;
        initialTris[17] = 0;

        initialTris[18] = 0;
        initialTris[19] = 4;
        initialTris[20] = 6;

        initialTris[21] = 7;
        initialTris[22] = 6;
        initialTris[23] = 4;

        initialTris[24] = 4;
        initialTris[25] = 5;
        initialTris[26] = 7;

        initialTris[27] = 2;
        initialTris[28] = 7;
        initialTris[29] = 3;

        initialTris[30] = 0;
        initialTris[31] = 1;
        initialTris[32] = 5;

        initialTris[33] = 0;
        initialTris[34] = 5;
        initialTris[35] = 4;

        //mesh.vertices = verts;
        //mesh.triangles = tris;

        triangles = new int[initialTris.Length];
        vertices = new Vector3[triangles.Length];
        for (int i = 0; i < triangles.Length; i++)
        {
            vertices[i] = initialVerts[triangles[i]];
            triangles[i] = i;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
