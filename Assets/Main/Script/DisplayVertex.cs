using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayVertex : MonoBehaviour {
    public Transform Vertex;
    Vector3[] vertices;
    Matrix4x4 matrix;
    // Use this for initialization
    void Start () {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        matrix = transform.localToWorldMatrix;
        vertices = mesh.vertices;
        foreach (Vector3 v in vertices)
        {
            Instantiate(Vertex, matrix.MultiplyPoint3x4(v), transform.rotation);
        }
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}
