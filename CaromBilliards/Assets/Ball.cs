using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IBall
{
    Rigidbody myBody;
    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }
    public void ApplyForce(Vector3 forward)
    {
        myBody.AddForce(forward);
    }

    public void OnBallCollision()
    {
        throw new System.NotImplementedException();
    }    
}
