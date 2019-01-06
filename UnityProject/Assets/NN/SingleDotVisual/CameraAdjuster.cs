using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjuster : MonoBehaviour {

    [SerializeField]
    private DotHead dot;
	
	void FixedUpdate () {
        Vector2 dir = dot.Direction;
        float angle = Vector2.SignedAngle(dir, Vector2.up);
        //Debug.Log("Dir: " + dir);
        //Debug.Log("Angle: " + angle);
        transform.localEulerAngles = new Vector3(0,0, -angle);
    }
}
