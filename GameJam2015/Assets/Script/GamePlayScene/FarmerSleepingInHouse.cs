using UnityEngine;
using System.Collections;


public class FarmerSleepingInHouse : MonoBehaviour
{
    #region Data Members

    public Transform spawnLocation;
    public GameObject farmerNick;
	public ParticleSystem zparticle;
	public GameObject bubbleText;


    private static FarmerSleepingInHouse _instance;

    [SerializeField]
    private int sleepMeter;

    #endregion

    #region Setters & Getters

    public static FarmerSleepingInHouse instance
    {
        get { return _instance; }
    }

    #endregion

    #region Built-in Unity Methods

    void Awake()
    {
        if(_instance != null)
        {
            _instance = this;
        }
        else
        {
          //  Destroy(this);
        }
    }

    // Use this for initialization
	void Start () 
    {
	    sleepMeter = 5;
	}
	
	// Update is called once per frame
	void Update () 
    {
		if(sleepMeter <= 1)
		{
			bubbleText.SetActive(true);
		}
        if(sleepMeter <= 0)
        {
            FarmerWakesUp();
        }
    }

    #endregion

    #region Public Methods

    public void SleepDisturbance()
    {
        sleepMeter--;
    }

    #endregion

    #region Private Methods

    void FarmerWakesUp()
	{
		zparticle.Stop();
		Destroy (bubbleText) ;
		Instantiate(farmerNick, spawnLocation.position, Quaternion.identity);
        Destroy(this);
    }

    #endregion
}
