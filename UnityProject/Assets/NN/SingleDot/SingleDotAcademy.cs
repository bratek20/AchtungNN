using MLAgents;
using System.Collections;
using UnityEngine;

public class SingleDotAcademy : Academy {
    [SerializeField]
    private Board board;

    public override void InitializeAcademy()
    {
        Monitor.SetActive(true);
    }

    public override void AcademyReset()
    {
        Debug.Log("prevWait");
        StartCoroutine(Wait());
        Debug.Log("postWait");
        board.Init();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(2);
    }

    public override void AcademyStep()
    {
        if(board.CountAllive == 0)
        {
            Done();
        }
    }
}
