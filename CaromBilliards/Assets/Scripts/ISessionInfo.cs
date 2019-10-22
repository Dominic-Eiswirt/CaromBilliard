using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISessionInfo
{
    void AddMoveToInput();
    void AddScore();
    void RegisterBallPositionsEndOfTurn(GameObject ball);
    void RegisterPower(float power, float strength);    
    bool IsSimulating { get; }
}
