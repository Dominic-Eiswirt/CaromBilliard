using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PlayerScored();
public interface IPlayerBall
{
    bool IsBallMoving();    
    void ApplyForce(Vector3 forward);
    event PlayerScored PlayerScoredEvent;
}
