using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class CameraYaser : MonoBehaviour {
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    public float speedH = 0.0f;
    public float speedV = 0.0f;

    private string info;
    private GameObject text;

    void Update()
    {
#if UNITY_EDITOR
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
#endif
        //InputTracking.Recenter();
        //transform.rotation = Quaternion.identity;
        //Vector3 rot = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles;
        //Quaternion qRot = Quaternion.Euler(-rot.x, -rot.y, -rot.z);
        //transform.localRotation = qRot;
    }
    // Use this for initialization
    void Start () {
        InputTracking.disablePositionalTracking = true;
	}
}
