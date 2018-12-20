using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotController : MonoBehaviour {

    [SerializeField]
    private float sensivity = 0.2f;
    [SerializeField]
    private DotHead dot;

	void Update ()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        if(horizontal < -sensivity)
        {
            dot.TurnLeft();
        }
        else if(horizontal > sensivity)
        {
            dot.TurnRight();
        }
	}
}
