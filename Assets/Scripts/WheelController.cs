using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class WheelController : MonoBehaviour
{
    public Graphic UI_Element;
    public float maximumSteeringAngle = 250f;
    public float wheelReleasedSpeed = 250f;
    RectTransform _rectT;
    Vector2 _centerPoint;
    float _wheelAngle = 0f;
    float _wheelPrevAngle = 0f;
    bool _wheelBeingHeld = false;

     void Start()
    {
        _rectT = UI_Element.rectTransform;
        InitEventsSystem();
    }
    void Update()
    {
        // If the wheel is released, reset the rotation
        // to initial (zero) rotation by wheelReleasedSpeed degrees per second
        if(!_wheelBeingHeld && !Mathf.Approximately( 0f, _wheelAngle ))
        {
            float deltaAngle = wheelReleasedSpeed * Time.deltaTime;

            if(Mathf.Abs( deltaAngle ) > Mathf.Abs( _wheelAngle ))

                _wheelAngle = 0f;

            else if(_wheelAngle > 0f)

                _wheelAngle -= deltaAngle;

            else
                _wheelAngle += deltaAngle;
        }
 
        // Rotate the wheel image
        _rectT.localEulerAngles = Vector3.back * _wheelAngle;
    }
    public float GetClampedValue()
    {
        // returns a value in range [-1,1] similar to GetAxis("Horizontal")
        return _wheelAngle / maximumSteeringAngle;
    }
 
    public float GetAngle()
    {
        // returns the wheel angle itself without clamp operation
        return _wheelAngle;
    }
 
    void InitEventsSystem()
    {
        
        EventTrigger events = UI_Element.gameObject.GetComponent<EventTrigger>();
 
        if( events == null )
            events = UI_Element.gameObject.AddComponent<EventTrigger>();
 
        if( events.triggers == null )
            events.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();
 
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
        UnityAction<BaseEventData> functionCall = new UnityAction<BaseEventData>( PressEvent );
        callback.AddListener( functionCall );
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback = callback;
 
        events.triggers.Add( entry );
 
        entry = new EventTrigger.Entry();
        callback = new EventTrigger.TriggerEvent();
        functionCall = new UnityAction<BaseEventData>( DragEvent );
        callback.AddListener( functionCall );
        entry.eventID = EventTriggerType.Drag;
        entry.callback = callback;
 
        events.triggers.Add( entry );
 
        entry = new EventTrigger.Entry();
        callback = new EventTrigger.TriggerEvent();
        functionCall = new UnityAction<BaseEventData>( ReleaseEvent );
        callback.AddListener( functionCall );
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback = callback;
 
        events.triggers.Add( entry );
    }
 
    public void PressEvent( BaseEventData eventData )
    {
        // Executed when mouse/finger starts touching the steering wheel
        Vector2 pointerPos = ( (PointerEventData) eventData ).position;
 
        _wheelBeingHeld = true;
        _centerPoint = RectTransformUtility.WorldToScreenPoint( ( (PointerEventData) eventData ).pressEventCamera, _rectT.position );
        _wheelPrevAngle = Vector2.Angle( Vector2.up, pointerPos - _centerPoint );
    }
 
    public void DragEvent( BaseEventData eventData )
    {
        // Executed when mouse/finger is dragged over the steering wheel
        Vector2 pointerPos = ( (PointerEventData) eventData ).position;
 
        float wheelNewAngle = Vector2.Angle( Vector2.up, pointerPos - _centerPoint );
        
        // Do nothing if the pointer is too close to the center of the wheel
        if( Vector2.Distance( pointerPos, _centerPoint ) > 20f )
        {
            if( pointerPos.x > _centerPoint.x ){
                _wheelAngle += wheelNewAngle - _wheelPrevAngle;
                BoatController.boatController.RotateShip(-_wheelAngle);
                }
            else
                _wheelAngle -= wheelNewAngle - _wheelPrevAngle;
                BoatController.boatController.RotateShip(_wheelAngle);
        }
        // Make sure wheel angle never exceeds maximumSteeringAngle
        _wheelAngle = Mathf.Clamp( _wheelAngle, -maximumSteeringAngle, maximumSteeringAngle );
        _wheelPrevAngle = wheelNewAngle;
    }
 
    public void ReleaseEvent( BaseEventData eventData )
    {
        // Executed when mouse/finger stops touching the steering wheel
        // Performs one last DragEvent, just in case
        DragEvent( eventData );
 
        _wheelBeingHeld = false;
    }
}
