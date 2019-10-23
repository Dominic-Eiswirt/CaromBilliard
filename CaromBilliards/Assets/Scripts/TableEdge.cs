using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableEdge : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] GameObject AudioCenter;
#pragma warning restore
    IAudioCenter audioCenter;
    AudioSource mySource;
    void Start()
    {
        mySource = GetComponent<AudioSource>();
        audioCenter = AudioCenter.GetComponent<IAudioCenter>();
        mySource.clip = audioCenter.RequestWoodImpact();
    }

    void OnCollisionEnter(Collision collision)
    {   
        if(collision.gameObject.GetComponent<IBall>() != null)
        {   
            mySource.volume = collision.rigidbody.velocity.magnitude / 50f;
            mySource.Play();
        }
    }
}
