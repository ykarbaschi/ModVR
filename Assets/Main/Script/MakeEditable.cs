using UnityEngine;
using System.Collections;

public class MakeEditable : MonoBehaviour
{
    public GameObject controller;
    private Mesh mesh;
    private GameObject[] controllers;
    private Vector3[] vertices;
    private int[] initialTris;
    // Use this for initialization
    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        makeControllers();
        //transform.rotation = Quaternion.Euler(-90, 0 ,0);
        //transform.localScale = new Vector3(100, 100, 100);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        getHandleChange();
    }

    private void getHandleChange()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = controllers[i].transform.position;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    private void makeControllers()
    {
        vertices = mesh.vertices;
        //initialTris = mesh.triangles;
        controllers = new GameObject[vertices.Length];
        for (int i = 0; i < controllers.Length; i++)
        {
            controllers[i] = Instantiate(controller, vertices[i], Quaternion.Euler(0, 0, 0));
            controllers[i].transform.parent = transform;
        }
    }
}
