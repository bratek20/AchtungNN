using MLAgents;
using System;
using System.Collections;
using UnityEngine;

public class BuilderDotAcademy : Academy
{
    public enum SymmetryType
    {
        NONE,
        USE,
        RANDOM
    }

    [Serializable]
    public class BoardOption
    {
        public BoardBuilder BuilderPrefab;
        public int StepsK;
        public SymmetryType SymType = SymmetryType.NONE;

        public bool CheckMakeSymmetry()
        {
            switch (SymType)
            {
                case SymmetryType.NONE:
                    return false;
                case SymmetryType.USE:
                    return true;
                case SymmetryType.RANDOM:
                    return UnityEngine.Random.Range(0, 2) == 0;
            }
            return false;
        }
    }

    [SerializeField]
    private Board board;
    [SerializeField]
    private int steps = 0;
    [SerializeField]
    private BoardOption[] options;
    [SerializeField]
    private BoardBuilder emptyPrefab;

    private int optionStepsSum = 0;
    private int optionsIdx = 0;

    public override void InitializeAcademy()
    {
        Monitor.SetActive(true);
    }

    private BoardOption GetCurrentOption()
    {
        while(optionsIdx < options.Length && options[optionsIdx].StepsK * 1000 + optionStepsSum <= steps)
        {
            optionStepsSum += options[optionsIdx].StepsK * 1000;
            optionsIdx++;
        }
        return optionsIdx < options.Length ? options[optionsIdx] : null;
    }
    public override void AcademyReset()
    {
        StartCoroutine(Wait());

        BoardOption option = GetCurrentOption();
        BoardBuilder builderPrefab = option != null ? option.BuilderPrefab : emptyPrefab;
        bool useSymmetry = option != null ? option.CheckMakeSymmetry() : false;
        board.Init(builderPrefab, useSymmetry);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(2);
    }

    public override void AcademyStep()
    {
        steps++;
        if (board.CountAllive == 0)
        {
            Done();
        }
    }
}
