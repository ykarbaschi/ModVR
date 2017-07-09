using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using DaydreamElements.SwipeMenu;

public class GizmoSelect : MonoBehaviour, IPointerEnterHandler
{
    private Gizmo gizmoControl;
    private bool shiftDown;
    private ActionSwipe swipeType;

    private bool selected = false;
    public Material activeMat;
    public Material inActiveMat;

    // Use this for initialization
    void Start()
    {
        gizmoControl = GameObject.Find("Gizmo").GetComponent<Gizmo>();
        swipeType = GameObject.Find("ActionSwipe").GetComponent<ActionSwipe>();
    }

    void OnMouseDown()
    {
        if (gizmoControl != null)
        {
            if (!shiftDown)
            {
                gizmoControl.ClearSelection();
            }
            gizmoControl.Show();
            gizmoControl.SelectObject(transform);
            gameObject.layer = 2;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
    //    {
    //        shiftDown = true;
    //    }
    //    else
    //    {
    //        shiftDown = false;
    //    }
    //}

    public void Unselect()
    {
        gameObject.GetComponent<MeshRenderer>().material = inActiveMat;
        gameObject.layer = 0;
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{

    //    if (swipeType.type == ActionType.Move)
    //    {
    //        selected = true;
    //        if (gizmoControl != null)
    //        {
    //            gizmoControl.ClearSelection();
    //            gameObject.GetComponent<MeshRenderer>().material = activeMat;
    //            gizmoControl.Show();
    //            gizmoControl.SelectObject(transform);
    //            gameObject.layer = 2;
    //        }
    //    }

    //    //if (swipeType.type == ActionType.Select)
    //    //{
    //    //    selected = true;
    //    //    if (gizmoControl != null)
    //    //    {

    //    //        gizmoControl.ClearSelection();
    //    //        //gizmoControl.Show();
    //    //        gizmoControl.SelectObject(transform);
    //    //        gameObject.layer = 2;
    //    //    }
    //    //}
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!GvrController.ClickButton)
        {
            selected = !selected;
            if (gizmoControl != null)
            {
                if (selected)
                {
                    gameObject.GetComponent<MeshRenderer>().material = activeMat;
                    gizmoControl.SelectObject(transform);
                    gizmoControl.UpdateCenter();
                    gameObject.layer = 2;
                }
                else
                {
                    gameObject.GetComponent<MeshRenderer>().material = inActiveMat;
                    Unselect();
                    gizmoControl.SelectedObjects.Remove(transform);
                    gizmoControl.UpdateCenter();
                }
            }
        }
    }
}
