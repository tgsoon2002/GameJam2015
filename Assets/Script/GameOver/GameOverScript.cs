using UnityEngine;using UnityEngine.UI;
using System.Collections;

//using System.IO;
//using System.Data;
//using Mono.Data.SqliteClient;

public class GameOverScript : MonoBehaviour {

	#region DataMember
	//Data members for linking with our database
//	IDbConnection dbconn;
//	IDbCommand dbcmd;
//	IDataReader reader;
//	string conn;
	
	public Text mainText;

	private int statusID = 0;
	private int cowNumber = 0;
	private float timeLeft = 0.0f;
	private string winLoseCondition ;
	private float minutes;
	private float seconds;
	#endregion

	#region built-in Method
	void Start () {

		PopulateText();
	}
//	void OnLevelWasLoaded(){
//		PopulateText();
//	}

	void Update(){
		if (Input.GetButtonDown ("Submit")) {
			PlayAgain();
		}
	}
	#endregion

	#region Public Method
	public void LoadFromData(){
		GameCurrentData.ldScript.Load();
		statusID = GameCurrentData.ldScript.statID;
		timeLeft = GameCurrentData.ldScript.timeLeft;
		cowNumber = GameCurrentData.ldScript.cow;
	
	}
	#endregion

	#region Private Method
	public void PlayAgain(){
		Application.LoadLevel (1);
	}
	#endregion

	void PopulateText(){
		LoadFromData();
		if (statusID == 0) {
			winLoseCondition = "YOU GOT OWNED";
		}
		else if(cowNumber != 0){
			winLoseCondition = "YOU GOT SOME COW!";
		}
		else {
			winLoseCondition = "AT LEAST YOU SAFE!";
		}
		minutes = Mathf.Floor(timeLeft / 60);
		seconds = Mathf.Floor(timeLeft % 60);
		mainText.text = winLoseCondition;
		mainText.text += "\nNumber of Cow : " + cowNumber;
		mainText.text += "\nTime left: " + string.Format("{0:00} : {1:00}", minutes, seconds);
	}

	void RetriveNewValue()
	{
//		conn = "URI=file:Assets/Plugin/KJData.s3db";
//		dbconn = (IDbConnection)new SqliteConnection(conn);
//		dbconn.Open();
//		dbcmd = dbconn.CreateCommand();
//		string sqlQuery = "SELECT * FROM NewScore";
//		dbcmd.CommandText = sqlQuery;
//		reader = dbcmd.ExecuteReader();
//		while (reader.Read()) 
//		{
//			statusID = reader.GetInt32(0);
//			cowNumber = reader.GetInt32(1);
//			timeLeft = reader.GetFloat(2);
//		}
//		dbconn.Close();
	}
	
	
}
