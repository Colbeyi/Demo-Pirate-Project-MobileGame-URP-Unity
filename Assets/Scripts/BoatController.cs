using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{   
    public static BoatController boatController;
    [SerializeField] private ParticleSystem splash;
	[SerializeField] private float turningSpeed = 60f;
    
    private Rigidbody _rigidbody;
    private ConstantForce _constantForce;
    private void Awake() {

        boatController = this;

        _rigidbody = GetComponent<Rigidbody>(); 
        _constantForce = GetComponent<ConstantForce>();
    }
    void Start()
    {
       
    }

    
    void Update()
    {
        
    }

    public void MoveForward(float forwardForce)
    {
        _constantForce.relativeForce = new Vector3(0f, 0f, forwardForce);
        splash.Play();
        
    }

    public void RotateShip(float wheelAngle){

        Vector3 torque = new Vector3(0f, wheelAngle * turningSpeed * Time.deltaTime, 0f);
        _rigidbody.AddTorque(torque);
    }

}
