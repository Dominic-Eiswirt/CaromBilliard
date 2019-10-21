using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioCenter
{
    AudioClip RequestCueHitClip();
    AudioClip RequestCollisionClip();
    float GetVelocityVolume();
    void SetVelocityVolume(float velocity);
}

