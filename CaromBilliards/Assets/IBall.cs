using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IBall
{
    void ApplyForce(Vector3 forward);
    void OnBallCollision();
}
