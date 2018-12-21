using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    [SerializeField]
    private float size = 5f;
    [SerializeField]
    private DotHead[] dots;

    private void Start()
    {
        Init();    
    }

    public void Init()
    {
        Camera.main.orthographicSize = size;
        foreach(var dot in dots)
        {
            dot.Init(Random.insideUnitCircle * size);
        }
    }

    private void Update()
    {
        bool allKilled = true; 
        foreach(var dot in dots)
        {
            allKilled  = allKilled && dot.Killed;
        }
        if(allKilled)
        {
            Init();
        }

    }
}
