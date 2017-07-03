using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {
    public Mesh mesh;
    public Material mat;
    public int maxDepth;
    public float childScale;

    private int depth;

	// Use this for initialization
	void Start () {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = mat;

        if (depth < maxDepth)
        {
            new GameObject("FractalChild").
                AddComponent<Fractal>().initialize(this);
        }
	}

    private void initialize(Fractal parent)
    {
        mesh = parent.mesh;
        mat = parent.mat;
        childScale = parent.childScale;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        transform.localScale = Vector3.up * childScale;
        transform.localPosition = Vector3.up * (0.5f + 0.5f * childScale);
        transform.parent = parent.transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
