using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {

	bool justClick;
	Vector3 lastPos;

	void Update () {
		if (Input.GetMouseButton(0)) {
			if (justClick) {
				var delta = Input.mousePosition - lastPos;
				transform.Rotate(Vector3.up * delta.x * 0.2f, Space.Self);
				var rot = transform.Find("pitch").localEulerAngles;
				rot.x = Mathf.Clamp(rot.x - delta.y * 0.2f, 10, 90);
				transform.Find("pitch").localEulerAngles = rot;
			}
			lastPos = Input.mousePosition;
			justClick = true;
		}
		else {
			justClick = false;
		}

		var lp = transform.Find("pitch/Main Camera").localPosition;
		lp.z = Mathf.Clamp(lp.z +Input.mouseScrollDelta.y*3f , -150, -10);
		transform.Find("pitch/Main Camera").localPosition = lp;
	}
}
