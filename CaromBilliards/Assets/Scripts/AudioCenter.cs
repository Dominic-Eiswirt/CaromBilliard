using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Requesting clips goes through here, because it can help with wanting more variation in sounds.
//The functions that return the clips can go through an array of different ones and choose at random in a dedicated place
public class AudioCenter : MonoBehaviour, IAudioCenter
{
    static AudioCenter instance;
    public AudioClip ballCollision;
    public AudioClip ballCueHit;
    public AudioClip woodImpact;
    float playerVelocityVolumeRegulator;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if (this != instance)
        {
            Destroy(this.gameObject);            
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
    public AudioClip RequestWoodImpact()
    {
        return woodImpact;
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

