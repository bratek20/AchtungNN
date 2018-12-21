using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour {

    [SerializeField]
    private DotConfig config;
    [SerializeField]
    private DotHead dot;

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
