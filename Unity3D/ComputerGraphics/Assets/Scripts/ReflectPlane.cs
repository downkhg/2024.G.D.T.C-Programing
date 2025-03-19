using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectPlane : MonoBehaviour
{
    MeshInfo meshInfo; 
    // Start is called before the first frame update
    void Start()
    {
        meshInfo = GetComponent<MeshInfo>();
    }

    public Vector3 GetNomal()
    {
        return meshInfo.m_plane.normal;
    }
}
