using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelTimer : MonoBehaviour 
{
    #region Data Members

    public Text timerText;
	public Text scroreText;
	public GameObject gameoverUI;
	public GameObject playerUI;

    private static LevelTimer _instance;
    private bool isOver;
    private bool countDown;

    [SerializeField]
    private float timer;
    private float minutes;
    private float seconds;
    private float fraction;

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
        isOver = false;
        countDown = false;

        timer = maxTime;
    }

    void FixedUpdate()
    {
        if(countDown)
        {
            timer -= Time.fixedDeltaTime;
            minutes = Mathf.Floor(timer / 60);
			seconds = Mathf.Floor(timer % 60);
			timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);	
        }

        if(timer <= 0.0f)
        {
			timer = 0.0f;
			countDown = false;
			UFODestroyed();
		}
	}

    #endregion

    #region Public Methods

    //
    public void StartTimer()
    {
        if(!countDown)
        {
            countDown = true;
        }
    }

	public void UFODestroyed(){
		countDown = false;
		playerUI.SetActive (false);
		gameoverUI.SetActive(true);
		scroreText.text = "Number of Cow : " + GameManager.instance.GMNumberOfCow.ToString();
		scroreText.text += "\nTime left: " + string.Format("{0:00} : {1:00}", minutes, seconds);
		Invoke("GoToMenu", 3.0f);
	}
    #endregion

    #region Private Methods

	void GameOver(){
		//Application.LoadLevel (0);

	}

    //Might not use 
    IEnumerator CountDown()
    {
        countDown = true;
        yield return new WaitForSeconds(maxTime);
        isOver = true;
    }

	void GoToMenu(){
		Application.LoadLevel(0);
	}
    #endregion
}
