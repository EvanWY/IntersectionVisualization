using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour {

	void Start () {
		ReloadConfig();
	}

	public void ReloadConfig() {
		RoadConfig.Load();
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
	}
}
