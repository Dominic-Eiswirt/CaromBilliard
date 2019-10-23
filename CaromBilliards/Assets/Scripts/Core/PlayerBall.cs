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
    List<BallType> exitBallType = new List<BallType>();
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
        if(exitBallType.Count>0)
        {
            foreach(BallType t in exitBallType)
            {
                if(t == BallType.Red)
                {
                    redCheck = false;
                }
                if(t == BallType.Yellow)
                {
                    yellowCheck = false;
                }
            }
        }        
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

    //Collision:
    //We have to sync the exit and enter collisions up, because during very short distances the collisions sometimes don't register.
    //Basically we don't reset the boolchecks unless we get a confirmation that unity registered a collider leaving. 
    //If a collider doesn't leave, and we reset the checks by ourselves, we will never get an OnCollisionEnter event.    
    void OnCollisionEnter(Collision collision)
    {
        if (IsBallMoving())
        {
            //If getcomponent works we know it's a ball
            IBall collisionBall = collision.gameObject.GetComponent<IBall>();
            if (collisionBall != null)
            {
                //Check each of the color of the balls, and remove them from the exit list if we collide (we don't want the collision checks to reset to falls while we are still colliding)
                if (collisionBall.GetBallType() == BallType.Red)
                {
                    redCheck = true;
                    if(exitBallType.Contains(collisionBall.GetBallType()))
                    {
                        exitBallType.Remove(collisionBall.GetBallType());
                    }
                }
                if (collisionBall.GetBallType() == BallType.Yellow)
                {
                    yellowCheck = true;
                    if (exitBallType.Contains(collisionBall.GetBallType()))
                    {
                        exitBallType.Remove(collisionBall.GetBallType());
                    }
                }

            }
            //Give player a point if applicable
            if (redCheck && yellowCheck)
            {
                PlayerScoredEvent();                
                ResetCollisionBools();
            }
        }
    }
    
    void OnCollisionExit(Collision collision)
    {
        //Add to the list so that we can remove the bool checks later when the ball is stationary. 
        //Doing it now would mess up the collision checks and scoring system
        IBall collisionBall = collision.transform.GetComponent<IBall>();
        if(collisionBall != null)
        {
            exitBallType.Add(collisionBall.GetBallType());
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
