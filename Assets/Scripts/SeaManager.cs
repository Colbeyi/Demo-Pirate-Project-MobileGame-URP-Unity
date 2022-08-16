using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaManager : MonoBehaviour
{   
    public static SeaManager instantiate;
    public float waveHeight = 0.5f;
    public float waveFrequency = 0.5f;
    public float waveSpeed = 0.02f;
    public Transform ocean;

    Material oceanMaterial;
    Texture2D wavesDisplacement;

    
    void Awake()
    {
        if(!instantiate){

            instantiate = this;
        }else{
            Destroy(gameObject);
        }
        
        SetVariables();
    }

    void SetVariables(){

        oceanMaterial = ocean.GetComponent<Renderer>().sharedMaterial;
        wavesDisplacement = (Texture2D)oceanMaterial.GetTexture("_Displacement");
    }

    public float WaterHeightAtPosition(Vector3 position){

        return ocean.position.y + wavesDisplacement.GetPixelBilinear(position.x * waveFrequency, position.z * waveFrequency + Time.time * waveSpeed).r * waveHeight * ocean.localScale.x;
    }

    void OnValidate() {

        if(!oceanMaterial)
            SetVariables();
        
        UpdateMaterial();
    }
   
    void UpdateMaterial(){
        //oceanMaterial.SetFloat("", waveFrequency);
        oceanMaterial.SetFloat("_PanSpeed", waveSpeed);
        oceanMaterial.SetFloat("_WaveHeight", waveHeight);
    }
}
