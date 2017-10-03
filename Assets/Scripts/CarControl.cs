using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControl : MonoBehaviour {

	public List<Transform> route;
	public float maxSpeed;
	public float currentSpeed;
	public float targetSpeed;
	public float progress;

	void Start () {
		transform.position = route[0].position;
		transform.rotation = route[0].rotation;
		ResetProgress();
	}

	Vector3 P0, P1, P2, P3;
	float sumdist;

	float vel;

	void Update() {
		if (progress >= 1) {
			if (! ResetProgress()) {
				return;
			}
		}

		var frontpos = BezierCurve.Sample(P0,P1,P2,P3, progress + 7/sumdist);
		if (Physics.Linecast(transform.position + transform.up + transform.forward*2f, frontpos + transform.up)){
			targetSpeed = 0;
		}
		else {
			targetSpeed = maxSpeed;
		}

		Debug.DrawLine(transform.position + transform.up + transform.forward*2f, 
			frontpos + transform.up,
			targetSpeed > 0.1 ? Color.green : Color.yellow, Time.deltaTime);

		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref vel, 0.1f);
		
		progress = Mathf.Clamp01(progress + (currentSpeed / sumdist) * Time.deltaTime);

		var newpos = BezierCurve.Sample(P0,P1,P2,P3,progress);
		transform.LookAt(newpos);
		transform.position = newpos;
	}

	bool ResetProgress() {
		if (route.Count < 2) {
			Destroy(gameObject);
			return false;
		}
		else {
			P0 = route[0].position;
			P3 = route[1].position;
			var ang = 0.5f * Vector3.Angle(route[0].forward, route[1].forward);
			sumdist = (P3 - P0).magnitude * (1f/Mathf.Cos(ang * Mathf.Deg2Rad));
			P1 = P0 + route[0].forward * sumdist / 3;
			P2 = P3 - route[1].forward * sumdist / 3;
			
			progress = 0;
			route.RemoveAt(0);
			return true;
		}
	}
}
