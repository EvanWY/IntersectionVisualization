using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using System;

public static class RoadConfig {
 
    public static RoadConfigContent config;
             
    public static void Save() {
        FileStream file = File.Create (Application.dataPath + "/roadconfig.json");
		byte[] info = new UTF8Encoding(true).GetBytes(JsonUtility.ToJson(config));
        file.Write(info, 0, info.Length);
        file.Close();
    }   
     
    public static void Load() {
        if(File.Exists(Application.dataPath + "/roadconfig.json")) {
            FileStream fileStream = File.Open(Application.dataPath + "/roadconfig.json", FileMode.Open);
			string contents;
			using(var sr = new StreamReader(fileStream)) {
				contents = sr.ReadToEnd();
			}
			config = JsonUtility.FromJson<RoadConfigContent>(contents);
            fileStream.Close();
        }
    }
}

[Serializable]
public class RoadConfigContent {
	public int LaneNum;
    public float RoadWidth;
	public float RoadLength;
}