using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

public class GameCurrentData : MonoBehaviour {

	public static GameCurrentData ldScript;
	public int statID;
	public int cow;
	public float timeLeft;

	void Awake(){
		if (ldScript == null) {
			DontDestroyOnLoad(gameObject);
			ldScript = this;
		}
		else if (ldScript != this) {
			Destroy(gameObject);
		}

	}

	public void Save(){
		BinaryFormatter bf= new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath+"/udderAbduction.dat",FileMode.OpenOrCreate);
		PlayerData currentData = new PlayerData();
		currentData.gameoverID = statID;
		currentData.cow = cow;
		currentData.time = timeLeft;
		bf.Serialize(file,currentData);
		file.Close();
	}

	public void Load(){
		if (File.Exists (Application.persistentDataPath+"/udderAbduction.dat")) {
			BinaryFormatter bf= new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath+"/udderAbduction.dat", FileMode.Open);
			PlayerData currentData = (PlayerData)bf.Deserialize(file);
			statID = currentData.gameoverID;
			cow = currentData.cow;
			timeLeft = currentData.time;
			file.Close ();
		}
	}
}

[Serializable]
public class PlayerData{
	public int gameoverID;
	public int cow;
	public float time;
}