using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    public enum BallType { White, Red, Yellow }
    [SerializeField] protected BallType myBallType;
    [SerializeField] protected GameObject AudioCenter;
    protected AudioSource mySource;
    protected IAudioCenter audioCenter;
   
    void Start()
    {
        audioCenter = AudioCenter.GetComponent<IAudioCenter>();
        mySource = GetComponent<AudioSource>();
    }
    public BallType GetBallType()
    {
        return myBallType;
    }    

    void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.GetComponent<IBall>() != null)
        {
            mySource.clip = audioCenter.RequestCollisionClip();            
            mySource.volume = mySource.volume = collision.rigidbody.velocity.magnitude / 20f;
            if (!mySource.isPlaying)
            {
                mySource.Play();
            }
        }
    }
}
