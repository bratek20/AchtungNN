using MLAgents;
using UnityEngine;

public class DistSingleDotAcademy : Academy {
    [SerializeField]
    private Board board;

    public override void AcademyReset()
    {
        board.Init();
    }

    public override void AcademyStep()
    {
        if(board.CountAllive == 0)
        {
            Done();
        }
    }
}
