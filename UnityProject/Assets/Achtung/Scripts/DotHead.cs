using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
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
    private DotConfig config;
    [SerializeField]
    private DotTail tail;

    private float curTurnTime = 0f;
    private int turnShift = 0;
    private Vector2 direction = Vector3.up;

    public bool Killed { private set; get; }

    public void Init(Vector2 initPos)
    {
        transform.position = new Vector3(initPos.x ,initPos.y, -1);
        curTurnTime = 0f;
        turnShift = 0;
        direction = Random.insideUnitCircle.normalized;
        Killed = false;
        tail.Init(config.color);
    }

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
        if(Killed)
        {
            return;
        }

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
        Killed = true;
    }
}
