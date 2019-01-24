using MLAgents;
using UnityEngine;
using System.Linq;

public class VisualAgent : Agent
{
    [SerializeField]
    private DotHead dot;
    [SerializeField]
    private Board board;
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

    private float CalcDistReward()
    {
        var dists = dot.GetDistsToObstacles(5, 33);
        float maxDist = board.MaxDist;
        float normSum = dists.Select(d => d / maxDist).Sum();
        return normSum / dists.Count;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float reward = CalcDistReward();
        Monitor.Log("Reward", reward.ToString());
        AddReward(reward);
         
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
