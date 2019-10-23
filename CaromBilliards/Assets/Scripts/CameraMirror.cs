using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMirror : MonoBehaviour
{
    public Vector3 offset;
    void Update()
    {
        this.transform.position = Camera.main.transform.position + offset;
    }
}
