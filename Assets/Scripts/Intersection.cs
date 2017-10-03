using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intersection : MonoBehaviour {

	public List<List<Transform>> routes = new List<List<Transform>>();

	public Text trafficCd;

	void Awake() {

		RoadConfig.Load();
	}

	void Start () {
		foreach (var mesh in GetComponentsInChildren<MeshRenderer>()) {
			var mat = mesh.material;
			mat.SetInt("LaneNum", RoadConfig.config.LaneNum);
			mat.SetInt("CrosswalkCount", Mathf.RoundToInt(RoadConfig.config.RoadWidth));
			mat.SetFloat("Length", RoadConfig.config.RoadLength);
		}
		foreach (Transform child in transform.Find("scale")) {
			if (child.name.StartsWith("side")) {
				child.localScale = new Vector3(1, 1, RoadConfig.config.RoadLength / RoadConfig.config.RoadWidth);
			}
		}
		transform.Find("scale").localScale = Vector3.one * (RoadConfig.config.RoadWidth * 0.1f);

		Transform[] spawn = new Transform[4];
		Transform[] cross = new Transform[4];

		for (int i=0; i<4; i++) {
			spawn[i] = transform.Find("scale").Find("side"+i).Find("controlpoints/spawn");
			cross[i] = transform.Find("scale").Find("side"+i).Find("controlpoints/cross");
		}

		float laneWidth = RoadConfig.config.RoadWidth * 0.5f / RoadConfig.config.LaneNum;
		for (int i=0; i<4; i++) {
			for (int lane=0; lane < RoadConfig.config.LaneNum; lane++) {
				float offset = (lane + 0.5f) * laneWidth;
				for (int p=0; p<4; p++) {
					if (i != p) {
						if ((p == (i + 3) % 4 && lane == 0 )
						|| (p == (i + 1) % 4 && lane == RoadConfig.config.LaneNum-1)
						|| (p == (i + 2) % 4)) {

							List<Transform> route = new List<Transform>();
							var tis = Transform.Instantiate(spawn[i], spawn[i].parent);
							var tic = Transform.Instantiate(cross[i], cross[i].parent);
							var tps = Transform.Instantiate(spawn[p], spawn[p].parent);
							var tpc = Transform.Instantiate(cross[p], cross[p].parent);

							tps.Rotate(new Vector3(0,180f,0));
							tpc.Rotate(new Vector3(0,180f,0));

							tis.Translate(Vector3.right * offset, Space.Self);
							tic.Translate(Vector3.right * offset, Space.Self);
							tps.Translate(Vector3.right * offset, Space.Self);
							tpc.Translate(Vector3.right * offset, Space.Self);

							route.Add(tis);
							route.Add(tic);
							route.Add(tpc);
							route.Add(tps);

							routes.Add(route);
						}
					}
				}
			}
		}

		StartCoroutine(TrafficLight());

	}

	IEnumerator TrafficLight() {
		transform.Find("scale/center/light0/green").gameObject.SetActive(true);
		transform.Find("scale/center/light0/red").gameObject.SetActive(false);
		transform.Find("scale/center/light1/green").gameObject.SetActive(false);
		transform.Find("scale/center/light1/red").gameObject.SetActive(true);
		transform.Find("scale/center/light2/green").gameObject.SetActive(true);
		transform.Find("scale/center/light2/red").gameObject.SetActive(false);
		transform.Find("scale/center/light3/green").gameObject.SetActive(false);
		transform.Find("scale/center/light3/red").gameObject.SetActive(true);
		while (true) {
			for (int i=0; i<30; i++) {
				trafficCd.text = "Traffic Light Count Down: " + (30-i);
				yield return new WaitForSeconds(1);
			}
			transform.Find("scale/center/light0/green").gameObject.SetActive(false);
			transform.Find("scale/center/light0/red").gameObject.SetActive(true);
			transform.Find("scale/center/light2/green").gameObject.SetActive(false);
			transform.Find("scale/center/light2/red").gameObject.SetActive(true);
			for (int i=0; i<5; i++) {
				trafficCd.text = "Traffic Light Waiting .. " + (5-i);
				yield return new WaitForSeconds(1);
			}
			transform.Find("scale/center/light1/green").gameObject.SetActive(true);
			transform.Find("scale/center/light1/red").gameObject.SetActive(false);
			transform.Find("scale/center/light3/green").gameObject.SetActive(true);
			transform.Find("scale/center/light3/red").gameObject.SetActive(false);

			for (int i=0; i<30; i++) {
				trafficCd.text = "Traffic Light Count Down: " + (30-i);
				yield return new WaitForSeconds(1);
			}
			transform.Find("scale/center/light1/green").gameObject.SetActive(false);
			transform.Find("scale/center/light1/red").gameObject.SetActive(true);
			transform.Find("scale/center/light3/green").gameObject.SetActive(false);
			transform.Find("scale/center/light3/red").gameObject.SetActive(true);
			for (int i=0; i<5; i++) {
				trafficCd.text = "Traffic Light Waiting .. " + (5-i);
				yield return new WaitForSeconds(1);
			}
			transform.Find("scale/center/light0/green").gameObject.SetActive(true);
			transform.Find("scale/center/light0/red").gameObject.SetActive(false);
			transform.Find("scale/center/light2/green").gameObject.SetActive(true);
			transform.Find("scale/center/light2/red").gameObject.SetActive(false);
		}
	}

	float timeRemain = 0;

	void Update() {
		if (Input.GetKeyDown(KeyCode.A)) {
			SpawnCar();
		}

		if (timeRemain < 0.001f) {
			timeRemain = RoadConfig.config.CarSpawnWaitTime;
			SpawnCar();
		}
		timeRemain -= Time.deltaTime;
	}

	void SpawnCar() {
		var go = GameObject.Instantiate(transform.Find("templatecar").gameObject);
		go.GetComponent<CarControl>().route = new List<Transform>(routes[Random.Range(0, routes.Count)]);
		go.SetActive(true);
	}

	public void ReloadConfig() {
        SceneManager.LoadScene(0);
	}
}
