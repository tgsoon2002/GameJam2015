using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//using System.IO;
//using System.Data;
//using Mono.Data.SqliteClient;

public class LevelTimer : MonoBehaviour 
{
    #region Data Members

	//Data members for linking with our database
//	IDbConnection dbconn;
//	IDbCommand dbcmd;
//	IDataReader reader;
//	string conn;
    public Text timerText;
	public Text scroreText;
	public GameObject gameoverUI;
	public GameObject playerUI;

	public Slider volumeSlider;

    private static LevelTimer _instance;
    
    private bool countDown;
	private bool isPausing = false;
    [SerializeField]
    private float timer;
    private float minutes;
    private float seconds;

    [SerializeField]
    private float maxTime;

    #endregion

    #region Setters and Getters

    public static LevelTimer instance
    {
        get { return _instance; }
    }

    public float Level_Time
    {
        get { return maxTime; }
        set { maxTime = value; }
    }

    #endregion

    #region Built-in Engine Methods

    void Awake()
    {
        if(_instance != null)
        {
            _instance = this;
        }
        else
        {
            Destroy(_instance);
        }
    }

    void Start()
    {
//		conn = "URI=file:Assets/Plugin/KJData.s3db";
//		dbconn = (IDbConnection)new SqliteConnection(conn);
        countDown = false;
        timer = maxTime;
		volumeSlider.value = GameManager.instance.GetComponent<AudioSource>().volume;
    }

	void Update(){
		if (Input.GetButtonDown ("PauseGame")) {

			isPausing = !isPausing;
			GameManager.instance.PauseGame(isPausing);
			gameoverUI.SetActive(isPausing);
			if (isPausing) {
				Time.timeScale = 0.0f;
			}
			else {
				Time.timeScale = 1.0f;
			}

		}
		if (isPausing) {
			volumeSlider.value += Input.GetAxis ("Horizontal")*0.01f;
		}
	}

    void FixedUpdate()
    {
		if(timer <= 0.0f)
		{
			timer = 0.0f;
			countDown = false;
			GameOver(1);
		}
		if(countDown )
        {
            timer -= Time.fixedDeltaTime;
            minutes = Mathf.Floor(timer / 60);
			seconds = Mathf.Floor(timer % 60);
			timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);	
        }
      
	}

    #endregion

    #region Public Methods

	public void AddExtraTime(float val){
		timer+= val;
	}

    //
    public void TimerRunning(bool val)
    {
		countDown = val;
    }

	public void UFODestroyed(){
		GameOver(0);
	}
    #endregion

    #region Private Methods

	void GameOver(int val){

		//string sqlQuery = "UPDATE NewScore SET winLose = " + val + " , CowCounter = " + GameManager.instance.GMNumberOfCow + ", TimeLeft = " +timer;
		//ExecuteUpdateQuerry(sqlQuery);
		GameCurrentData.ldScript.statID = val;
		GameCurrentData.ldScript.cow = GameManager.instance.GMNumberOfCow;
		GameCurrentData.ldScript.timeLeft = timer;
		GameCurrentData.ldScript.Save();
		Application.LoadLevel (2);
	}

 	void GoToMenu(){
		Application.LoadLevel(0);
	}
//
//	void ExecuteUpdateQuerry(string sqlQuery){
//		dbconn.Open ();
//		dbcmd = dbconn.CreateCommand ();
//		dbcmd.CommandText = sqlQuery; 
//		dbcmd.ExecuteNonQuery ();
//		dbconn.Close ();
//	}
    #endregion
}
