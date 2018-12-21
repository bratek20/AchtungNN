using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour {

    [SerializeField]
    private string leftKey;
    [SerializeField]
    private string rightKey;
    [SerializeField]
    private DotHead dot;

	void Update ()
    {    
        if(Input.GetKey(leftKey))
        {
            dot.TurnLeft();
        }
        else if(Input.GetKey(rightKey))
        {
            dot.TurnRight();
        }
	}
}
