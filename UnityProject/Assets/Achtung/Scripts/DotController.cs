using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour {

    [SerializeField]
    private DotHead dot;

    private DotConfig config;

    private void Start()
    {
        config = dot.Config;    
    }

    void Update ()
    {    
        if(Input.GetKey(config.leftKey))
        {
            dot.TurnLeft();
        }
        else if(Input.GetKey(config.rightKey))
        {
            dot.TurnRight();
        }
	}
}
