using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
public class MeshBaker : MonoBehaviour
{
    public TrailRenderer[] trailRenderer;
    public string exportFileName;
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            var combine = new CombineInstance[trailRenderer.Length];
 
            for (int i = 0; i < trailRenderer.Length; i++)
            {
                var mesh = new Mesh();
                trailRenderer[i].BakeMesh(mesh, true);
                combine[i].mesh = mesh;
            }
 
            var combinedMesh = new Mesh();
            combinedMesh.CombineMeshes(combine, true, false, false);
 
            var savePath = "Assets/"+ exportFileName + ".asset";
            Debug.Log("Saved Mesh to:" + savePath);
            AssetDatabase.CreateAsset(combinedMesh, savePath);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Cleaned trail renderer");
            for (int i = 0; i < trailRenderer.Length; i++)
            {
                trailRenderer[i].Clear();
            }
        }
    }
 }
