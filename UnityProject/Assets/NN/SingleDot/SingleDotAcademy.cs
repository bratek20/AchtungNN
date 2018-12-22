using MLAgents;
using UnityEngine;

public class SingleDotAcademy : Academy {
    [SerializeField]
    private Board board;

    public override void AcademyReset()
    {
        board.Init();
    }

    public override void AcademyStep()
    {
        if(board.AllKilled)
        {
            Done();
        }
    }
}
