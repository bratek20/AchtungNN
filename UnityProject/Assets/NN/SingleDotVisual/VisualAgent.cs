using MLAgents;
using UnityEngine;
using System.Linq;

public class VisualAgent : Agent
{
    [SerializeField]
    private DotHead dot;
    [SerializeField]
    private Transform[] cameras;

    public override void CollectObservations()
    {
        Vector2 dir = dot.Direction;
        float angle = Vector2.SignedAngle(dir, Vector2.up);
        foreach(var cam in  cameras)
        {
            cam.localEulerAngles = new Vector3(0, 0, -angle);
        }
    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        AddReward(0.01f);

        const float TURN_THRESHOLD = 0.33f;
        float turnValue = vectorAction[0];
        if (turnValue < -TURN_THRESHOLD)
        {
            dot.TurnLeft();
        }
        else if (turnValue > TURN_THRESHOLD)
        {
            dot.TurnRight();
        }

        if (dot.Killed)
        {
            Done();
            SetReward(-1.0f);
        }
    }
}
