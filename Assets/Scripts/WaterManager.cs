using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    private MeshFilter meshFilter;
    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3[] verticals = meshFilter.mesh.vertices;
        for (int i = 0; i < verticals.Length; i++)
        {
            verticals[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + verticals[i].x);

        }
        meshFilter.mesh.vertices = verticals;
        meshFilter.mesh.RecalculateNormals();
    }
}
