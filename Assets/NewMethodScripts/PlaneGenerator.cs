using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class PlaneGenerator : MonoBehaviour
{
    public int Size = 20;
    public float scale = 1.0f;

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uvs;
    private int _verticiesLength = 0;

    [SerializeField] private Transform _debugSphere;
    [SerializeField] private bool isUpdatingOnCPU = false;

    void Start()
    {
        _mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = _mesh;
        _verticiesLength = (Size + 1) * (Size + 1);

        UpdatePlaneVerticies();
        UpdateMesh();
    }

    private void FixedUpdate() {

        if (isUpdatingOnCPU)
        {
            UpdatePlaneVerticies();
            UpdateMesh();
        }

        if (_debugSphere != null) 
        {
            Vector3 newPos = _debugSphere.position;
            newPos.y = WaterController.current.GetHeightAtPosition(newPos) + transform.position.y;
            _debugSphere.position = newPos;
        }
    }

    void UpdatePlaneVerticies()
    {
        _vertices = new Vector3[_verticiesLength];
        _uvs = new Vector2[_vertices.Length];

        float halfSizeX = (scale * Size) / 2;
        float halfSizeZ = (scale * Size) / 2;

		int i = 0;
		for (int z = 0; z <= Size; z++) 
        {
			for (int x = 0; x <= Size; x++) 
            {
                float xPos = (x * scale) - halfSizeX;
                float zPos = (z * scale) - halfSizeZ;
                float yPos = 0;

				_vertices[i] = new Vector3(xPos, yPos, zPos);
                
                if (isUpdatingOnCPU)
                    _vertices[i] += WaterController.current.GetWaveAddition(_vertices[i] + transform.position, Time.timeSinceLevelLoad);
				
                _uvs[i] = new Vector2(_vertices[i].x, _vertices[i].z);
				i++;
			}
		}

        _triangles = new int[Size * Size * 6];

		int vert = 0;
        int tris = 0;

		for (int z = 0; z < Size; z++) 
        {
			for (int x = 0; x < Size; x++) 
            {
				_triangles[tris + 0] = vert + 0;
				_triangles[tris + 1] = vert + Size + 1;
				_triangles[tris + 2] = vert + 1;
				_triangles[tris + 3] = vert + 1;
				_triangles[tris + 4] = vert + Size + 1;
				_triangles[tris + 5] = vert + Size + 2;

				vert++;
				tris += 6;
			}
			vert++;
		}
    }

    void UpdateMesh()
    {
        _mesh.Clear();
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs;
        _mesh.RecalculateNormals();
    }

}
