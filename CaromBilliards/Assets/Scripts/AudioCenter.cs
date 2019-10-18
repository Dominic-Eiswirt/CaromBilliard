using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCenter : MonoBehaviour
{
    AudioCenter instance;
    
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
}

