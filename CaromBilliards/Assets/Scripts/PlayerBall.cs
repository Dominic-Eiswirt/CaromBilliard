using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBall : Ball, IPlayerBall
{
    bool redCheck, yellowCheck;
    Rigidbody myBody;    
    LineRenderer lineRenderer;
    public event PlayerScored PlayerScoredEvent;
#pragma warning disable CS0649
    [SerializeField] GameObject GhostBall;
#pragma warning restore

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
    }    
    void Update()
    {
        //If ball is stationary (we can make a move) then we reset the rotation, wait for input, and reset the collision bools
        if (!IsBallMoving())
        {            
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
            DrawRaycastToDirection();
       
            ResetCollisionBools();
        }                
        else
        {
            GhostBall.SetActive(false);
            lineRenderer.enabled = false;
        }
    }       

    void ResetCollisionBools()
    {
        redCheck = false;
        yellowCheck = false;
    }
    
    public void ApplyForce(Vector3 forward)
    {
        mySource.clip = audioCenter.RequestCueHitClip();
        if (!mySource.isPlaying)
        {
            mySource.volume = 1f;
            mySource.Play();
        }
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
                PlayerScoredEvent();
                ResetCollisionBools();
            }
        }
    }

    //Raycast
    void DrawRaycastToDirection()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.collider)
            {
                GhostBall.SetActive(true);
                GhostBall.transform.position = hit.point;
                lineRenderer.SetPosition(1, hit.point);
            }

        }
        else
        {            
            lineRenderer.SetPosition(1, transform.forward * 5000);
        }
    }
}
