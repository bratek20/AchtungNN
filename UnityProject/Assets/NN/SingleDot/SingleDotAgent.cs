using MLAgents;
using UnityEngine;
using System.Linq;

public class SingleDotAgent : Agent {
    [SerializeField]
    private int distSamples = 10;
    [SerializeField]
    private float angleStep = 5f;
    [SerializeField]
    private DotHead dot;

    public override void CollectObservations()
    {
        var dists = dot.GetDistsToObstacles(distSamples, angleStep);
        float max = dists.Max();
        dists = dists.Select(val => val / max).ToList();
        AddVectorObs(dists);
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
