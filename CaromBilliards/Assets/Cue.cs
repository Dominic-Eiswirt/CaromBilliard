using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cue : MonoBehaviour, ICue
{
    public Camera cam;
    public Vector3 cameraOffset;
    Vector3 newCameraOffset;
    float targetDistanceToBall = -2.06f;
    Vector3 originalPos;    
    bool firing;
        
    void Update()
    {
        newCameraOffset = new Vector3(0, cam.transform.position.y + cameraOffset.y, cam.transform.position.z + cameraOffset.z);
        originalPos = newCameraOffset;
        if (!firing)
        {
            this.transform.position = originalPos;
            this.transform.rotation = new Quaternion(0, cam.transform.rotation.y, 0, cam.transform.rotation.w);
        }
    }
    public void AdjustCue(float value)
    {
        firing = true;
        Vector3 newPos = new Vector3(0, 0, value * -transform.forward.z);
        this.transform.position = originalPos + newPos;
    }
    public void Release()
    {
        firing = false;        
    }
}
