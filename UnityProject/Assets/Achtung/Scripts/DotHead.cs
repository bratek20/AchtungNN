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
    private float gapApplyTime = 3f;
    [SerializeField]
    private float gapRandomOffset = 2f;
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private DotConfig config;
    [SerializeField]
    private SpriteRenderer headSprite;
    [SerializeField]
    private DotTail tail;

    private float curTurnTime = 0f;
    private float makeGapTime = 0f;
    private int turnShift = 0;
    private Vector2 direction = Vector3.up;

    public bool Killed { private set; get; }

    public void Init(Vector2 initPos)
    {
        transform.position = new Vector3(initPos.x, initPos.y, -1);
        curTurnTime = 0f;
        turnShift = 0;
        direction = Random.insideUnitCircle.normalized;
        Killed = false;
        headSprite.color = config.color;
        tail.Init(config.color);
        CalculateGapTime();
    }

    public void TurnLeft()
    {
        turnShift = TURN_LEFT_SHIFT;
    }

    public void TurnRight()
    {
        turnShift = TURN_RIGHT_SHIFT;
    }

    private float RaycastDist(Vector2 dir)
    {
        var hit = Physics2D.Raycast(transform.position, dir);
        float dist = Vector2.Distance(hit.point, transform.position);
        //Debug.Log(dist);
        return dist;
    }

    public List<float> GetDistsToObstacles(int samples, float angleStep)
    {
        List<float> dists = new List<float>();
        dists.Add(RaycastDist(direction));
        for (int i = 1; i <= samples; i++)
        {
            float angle = angleStep * i;
            dists.Add(RaycastDist(RotateVector(direction, angle)));
            dists.Add(RaycastDist(RotateVector(direction, -angle)));
        }
        return dists;
    }

    void FixedUpdate()
    {
        if (Killed)
        {
            return;
        }

        curTurnTime += Time.fixedDeltaTime;
        if (curTurnTime > turnApplyTime)
        {
            ApplyTurn(curTurnTime);
            curTurnTime = 0f;
        }

        makeGapTime -= Time.fixedDeltaTime;
        if (makeGapTime < 0)
        {
            tail.MakeGap();
            CalculateGapTime();
        }
    }

    private void CalculateGapTime()
    {
        makeGapTime = gapApplyTime + Random.Range(0, gapRandomOffset);
    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }

    private void ApplyTurn(float dt)
    {
        if (turnShift != 0)
        {
            direction = RotateVector(direction, turnShift * turnForce * speed * dt).normalized;
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
