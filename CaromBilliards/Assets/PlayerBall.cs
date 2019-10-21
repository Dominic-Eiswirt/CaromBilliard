using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : Ball, IPlayerBall
{   
    [SerializeField] bool redCheck, yellowCheck;
    Rigidbody myBody;
    float rotationSpeed = 50f;    

    void Start()
    {
        myBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //If ball is stationary (we can make a move) then we reset the rotation, wait for input, and reset the collision bools
        if (!IsBallMoving())
        {
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0, -Time.deltaTime * rotationSpeed, 0, Space.Self);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, Time.deltaTime * rotationSpeed, 0, Space.Self);
            }
            ResetCollisionBools();
        }        
    }
   
    //Add all score related things here
    void AddScore()
    {
        Debug.Log("Adding Score");
        ResetCollisionBools();
    }

    void ResetCollisionBools()
    {
        redCheck = false;
        yellowCheck = false;
    }
    
    public void ApplyForce(Vector3 forward)
    {
        myBody.AddForce(forward);
    }
       

    public bool IsBallMoving()
    {
        if (myBody.velocity.z != 0 || myBody.velocity.x != 0)
        {
            return true;
        }
        return false;
    }

    //Collision
    void OnCollisionEnter(Collision collision)
    {
        if (IsBallMoving())
        {
            IBall collisionBall = collision.gameObject.GetComponent<IBall>();
            if (collisionBall != null)
            {
                if (collisionBall.GetBallType() == BallType.Red)
                {
                    redCheck = true;
                }
                if (collisionBall.GetBallType() == BallType.Yellow)
                {
                    yellowCheck = true;
                }

            }
            if (redCheck && yellowCheck)
            {
                AddScore();
            }
        }
    }
}
