using UnityEngine;

public class BuoyancyObjects : MonoBehaviour
{   
    public Transform[] floaters;
    public float underWaterDrag = 3f;
    public float underWaterAngularDrag = 1f;
    public float airDrag = 0f;
    public float airAngularDrag = 0.05f;
    public float floatingPower = 15f;

    Rigidbody _rigidBody;
    int _floatersUnderWater;
    bool _isUnderWater;

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {   
        _floatersUnderWater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float difference = floaters[i].position.y - SeaManager.instantiate.WaterHeightAtPosition(floaters[i].position);

            if(difference < 0){

            _rigidBody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floaters[i].position, ForceMode.Force);
            _floatersUnderWater++;
                if(!_isUnderWater){

                _isUnderWater = true;
                SwitchState(true);

                }

            
            }
        }
        
        if(_isUnderWater && _floatersUnderWater == 0){
            _isUnderWater = false;
            SwitchState(false);
        }
        
    }

    private void SwitchState(bool isUnderWater){
        
        if(_isUnderWater){

            _rigidBody.drag = underWaterDrag;
            _rigidBody.angularDrag = underWaterAngularDrag;

        }else{

            _rigidBody.drag = airDrag;
            _rigidBody.angularDrag = airAngularDrag;
        }

    }
}
