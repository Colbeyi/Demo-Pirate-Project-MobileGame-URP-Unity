using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{   
   public new Rigidbody rigidbody;
   public Material seaShader;
   public float depthBeforeSubmerged = 1f;
   public float displacementValue = 3f;
   public int floaterCount = 1;
   public float waterDrag = 0.99f;
   public float waterAngularDrag = 0.5f;

   private void FixedUpdate() {

        //float waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        //float waveHeight = seaShader.GetFloat("_DisplacementStrenght");
        rigidbody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);

        if(transform.position.y < 0f){

            float displacementMultiplier = Mathf.Clamp01((SeaManager.instantiate.WaterHeightAtPosition(transform.position) - transform.position.y) / depthBeforeSubmerged) * displacementValue;
            rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
            rigidbody.AddForce(displacementMultiplier * -rigidbody.velocity * waterDrag, ForceMode.VelocityChange);
            rigidbody.AddTorque(displacementMultiplier * -rigidbody.angularVelocity * waterAngularDrag, ForceMode.VelocityChange);

        }
   }
}
