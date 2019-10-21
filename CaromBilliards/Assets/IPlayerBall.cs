using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerBall
{
    bool IsBallMoving();    
    void ApplyForce(Vector3 forward);
 
}
