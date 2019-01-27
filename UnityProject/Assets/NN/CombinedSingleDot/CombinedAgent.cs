using MLAgents;
using UnityEngine;
using System.Linq;

public class CombinedAgent : Agent
{
    [SerializeField]
    private int distSamples = 10;
    [SerializeField]
    private float angleStep = 5f;

    [SerializeField]
    private DotHead dot;
    [SerializeField]
    private Board board;
    [SerializeField]
    private Transform[] cameras;

    public override void CollectObservations()
    {
        var dists = dot.GetDistsToObstacles(distSamples, angleStep);
        float max = dists.Max();
        dists = dists.Select(val => val > 1 ? 1 : val).ToList()
            .Concat(dists.Select(val => val < 1 ? 1 : 1 / val).ToList()).ToList();
        AddVectorObs(dists);

        Vector2 dir = dot.Direction;
        float angle = Vector2.SignedAngle(dir, Vector2.up);
        foreach(var cam in  cameras)
        {
            cam.localEulerAngles = new Vector3(0, 0, -angle);
        }
    }

    private float CalcDistReward()
    {
        var dists = dot.GetDistsToObstacles(5, 33);
        float maxDist = board.MaxDist;
        float normSum = dists.Select(d => d / maxDist).Sum();
        return normSum / dists.Count;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        AddReward(0.01f);


        if (brain.brainParameters.vectorActionSpaceType == SpaceType.continuous)
        {
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
        }
        else
        {
            var action = Mathf.FloorToInt(vectorAction[0]);
            switch (action)
            {
                case 1:
                    dot.TurnLeft();
                    break;
                case 2:
                    dot.TurnRight();
                    break;
            }
        }

        if (dot.Killed)
        {
            Done();
            SetReward(-1.0f);
        }
    }
}
