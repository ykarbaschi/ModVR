using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DaydreamElements.SwipeMenu;
public class RotateZoom : MonoBehaviour
{

    // Use this for initialization

    private bool dragging = false;
    private GameObject cam;
    private GameObject model;
    public float speed = 5f;
    private Vector3 startPosition;
    private Vector3 diffDirection;
    private ActionSwipe action;
    public float MaxDistanceToCamera = 3f;
    public float MinDistanceToCamera = 1f;

    void Awake()
    {
        cam = GameObject.Find("Modeler");
        model = GameObject.Find("EditableCube");
    }

    private void Start()
    {
        action = GameObject.Find("ActionSwipe").GetComponent<ActionSwipe>();
    }

    // Update is called once per frame
    void Update()
    {
        if (action.type == ActionType.Rotate)
        {
            if (GvrController.ClickButtonDown)
            {
                dragging = true;
                startPosition = GvrController.ArmModel.pointerPosition;
            }
            if (GvrController.ClickButtonUp)
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

        if (action.type == ActionType.Zoom)
        {
            if (GvrController.ClickButtonDown)
            {
                dragging = true;
                startPosition = GvrController.ArmModel.pointerPosition;
            }

            if (GvrController.ClickButtonUp)
                dragging = false;

            if (GvrController.ClickButton)
            {
                diffDirection = (GvrController.ArmModel.pointerPosition - startPosition);
                Vector3 dir = (model.transform.position - cam.transform.position);
                float mag = diffDirection.magnitude;
                Mathf.Clamp(mag, 3, 6);

                cam.transform.Translate(dir.normalized * mag * Mathf.Sign(diffDirection.y), Space.World);
                //deltaTouch = GvrController.TouchPos - lasttouchPos;
                //delta += deltaTouch.y * zoomSpeed;
                //lasttouchPos = GvrController.TouchPos;
                //delta = Mathf.Clamp(delta, 2.5f, 5);
            }
            //offset = Camera.main.transform.forward * delta;
            //transform.position = Camera.main.transform.position + offset;
        }
    }
}
