using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotHead : MonoBehaviour
{
    private const int TURN_LEFT_SHIFT = 1;
    private const int TURN_RIGHT_SHIFT = -1;

    [SerializeField]
    private float turnForce = 1f;
    [SerializeField]
    private float turnApplyTime = 0f;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private DotTail tail;

    private float curTurnTime = 0f;
    private int turnShift = 0;
    private Vector2 direction = Vector2.up;

    public void TurnLeft()
    {
        turnShift = TURN_LEFT_SHIFT;    
    }
    
    public void TurnRight()
    {
        turnShift = TURN_RIGHT_SHIFT;
    }
	
	void Update ()
    {
        curTurnTime += Time.deltaTime;
        if(curTurnTime > turnApplyTime)
        {
            ApplyTurn(curTurnTime);
            curTurnTime = 0f;
        }
	}

    private void ApplyTurn(float dt)
    {
        if(turnShift != 0)
        {
            Vector2 perpVec = turnShift * Vector2.Perpendicular(direction);
            direction = Vector2.Lerp(direction, perpVec, turnForce * speed * dt).normalized;
        }

        tail.AddPoint(transform.position);
        transform.position += (Vector3)(direction * speed * dt);
        turnShift = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
