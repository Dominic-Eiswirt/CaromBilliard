using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCenter : MonoBehaviour, IAudioCenter
{
    AudioCenter instance;
    public AudioClip ballCollision;
    public AudioClip ballCueHit;
    float playerVelocityVolumeRegulator;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
    }

    public AudioClip RequestCueHitClip()
    {
        return ballCueHit;
    }

    public AudioClip RequestCollisionClip()
    {
        return ballCollision;
    }

    public float GetVelocityVolume()
    {
        return playerVelocityVolumeRegulator;
    }

    public void SetVelocityVolume(float velocity)
    {
        playerVelocityVolumeRegulator = velocity;
    }
}

