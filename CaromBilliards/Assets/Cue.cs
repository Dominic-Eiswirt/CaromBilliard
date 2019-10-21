using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cue : MonoBehaviour, ICue
{
    public GameObject target;
    public Camera cam;
    public Vector3 cameraOffset;       
    
    Vector3 drawCuePos;
    bool firing;
    float speed = 30f;        


    void Update()
    {
         if (!firing)
         {
            this.transform.localPosition = new Vector3(cameraOffset.x + target.transform.position.x, target.transform.position.y + cameraOffset.y, cameraOffset.z + target.transform.position.z);
            
        }
       
    }

    public void AdjustCue(float value)
    {
        //Draw back cue based on time pressed * backward
        firing = true;
        drawCuePos = target.transform.position + cameraOffset + value * -transform.forward;
        this.transform.position = Vector3.Lerp(this.transform.position, drawCuePos, Time.deltaTime * speed);
    }

    public void Release()
    {
        //Set bool off so we can track in update()
        firing = false;    
    }
}
