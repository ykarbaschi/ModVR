using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DaydreamElements.SwipeMenu
{
    public enum ActionType
    {
        Select = 3,
        Move = 1,
        Zoom = 0,
        Rotate = 2,
    }

    [RequireComponent(typeof(GvrAudioSource))]
    public class ActionSwipe : MonoBehaviour
    {
        public GameObject swipeMenu;

        private Gizmo gizmo;

        private const float MENU_OFFSET = 0.2f;
        private const float MIN_TIMEOUT = 0.36f;
        private float time;
        private ActionType currentType;
        private GvrAudioSource audioSource;
        public ActionType type;
        private GvrLaserPointerImpl laserPointerImpl;
        // Use this for initialization
        void Start()
        {
            gizmo = GameObject.Find("Gizmo").GetComponent<Gizmo>();
            audioSource = GetComponent<GvrAudioSource>();
            type = ActionType.Select;
            swipeMenu.GetComponent<SwipeMenu>().OnSwipeSelect += OnSwipeSelect;
        }

        // Update is called once per frame
        void Update()
        {
            //if (type == ActionType.Select || type == ActionType.Zoom || type == ActionType.Rotate)
            //    gizmo.Hide();
            if (gizmo.SelectedObjects.ToArray().Length == 0)
                gizmo.Hide();

            if (gizmo.SelectedObjects.ToArray().Length > 0)
                gizmo.Show();

            if (GvrController.ClickButtonDown && (type == ActionType.Move || type == ActionType.Select))
            {
                laserPointerImpl = (GvrLaserPointerImpl)GvrPointerManager.Pointer;
                if (!laserPointerImpl.IsPointerIntersecting)
                    gizmo.ClearSelection();
            }
        }

        private void OnSwipeSelect(int ix)
        {
            type = (ActionType)ix;
            //ColorUtil.Colorize(type, gameObject);
        }
    }
}
