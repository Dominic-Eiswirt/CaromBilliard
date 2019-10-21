using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    public enum BallType { White, Red, Yellow }
    [SerializeField] protected BallType myBallType;    
    
   
    public BallType GetBallType()
    {
        return myBallType;
    }    
}
