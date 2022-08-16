using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class CameraController : MonoBehaviour, IPointerDownHandler, IPointerExitHandler
 {
    public float TouchSensitivity_x = 10f;
    public float TouchSensitivity_y = 10f;
 
    private int touchID;
    private bool onPad;
 
    private void Start()
    {
        CinemachineCore.GetInputAxis = HandleAxisInputDelegate;
    }
 
    float HandleAxisInputDelegate(string axisName)
    {
        if (onPad)
        {
             switch (axisName)
             {
                 case "Mouse X":
                     if (Input.touchCount > 0)
                     {
                         return Input.touches[touchID].deltaPosition.x / TouchSensitivity_x;
                     }
                     else
                     {
                         return Input.GetAxis(axisName);
                     }
 
                 case "Mouse Y":
                     if (Input.touchCount > 0)
                     {
                         return Input.touches[touchID].deltaPosition.y / TouchSensitivity_y;
                     }
                     else
                     {
                         return Input.GetAxis(axisName);
                     }
 
                 default:
                     Debug.LogError("Input <" + axisName + "> not recognyzed.", this);
                     break;
             }
         }
         return 0f;
     }
 
     public void OnPointerExit(PointerEventData eventData)
     {
        onPad = false;

     }
     public void OnPointerDown(PointerEventData eventData)
     {
         if (Input.touchCount > 0)
         {
             foreach (Touch touch in Input.touches)
             {
                 int id = touch.fingerId;
                 if (touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(id))
                 {
                    onPad = true;
                    touchID = id;
 
                    break;
                 }
             }
         }
     }
     

}
