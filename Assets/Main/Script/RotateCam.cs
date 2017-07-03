using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCam : MonoBehaviour {

    // Use this for initialization
   
    private bool dragging = false;
    private GameObject cam;
    private GameObject model;
    public float speed = 5f;
    private Vector3 startPosition;
    private Vector3 diffDirection;

	void Awake () {
        cam = GameObject.Find("Modeler");
        model = GameObject.Find("EditableCube");
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (GvrController.AppButtonDown)
        {
            dragging = true;
            startPosition = GvrController.ArmModel.pointerPosition;
        }
        if (GvrController.AppButtonUp)
            dragging = false;

        if (dragging)
        {
            diffDirection = (GvrController.ArmModel.pointerPosition - startPosition);
            float diffX = diffDirection.x;
            float diffY = diffDirection.y;
            if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
                cam.transform.RotateAround(model.transform.position, Camera.main.transform.up, diffX * speed);
            else
                cam.transform.RotateAround(model.transform.position, Camera.main.transform.right, -diffY * speed);
        }
    }
}
