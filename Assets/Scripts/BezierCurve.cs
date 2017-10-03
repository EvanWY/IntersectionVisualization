using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour {

	public static Vector3 Sample(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t) {
		return Mathf.Pow(1-t, 3) * P0 
			+ 3*t*(1-t)*(1-t) * P1 
			+ 3*t*t*(1-t) * P2
			+ t*t*t * P3;
	}
}
