using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	#region Data Member
	public Text cowCounterT;
	public GuardDog dog;
	public BarMovement2 powerBar;
	public MovingUFO UFO;
	public FarmerAI farmer;
	public Camerashake mainCamera;
	public LevelTimer levelManager;

	public List<AudioClip> listAudio;
	public List<CowAI> listCow;

	private int counter = 0;
	private AudioSource audi;
	private bool frenzyMode = false;
	private static GameManager _instance;
	public static GameManager instance
	{
		get { return _instance; }
	}
	public FarmerSleepingInHouse house;
	#endregion

	#region getter and setter
	public int GMNumberOfCow{
		get { return counter; }
	}
	#endregion

	#region Built-in

	void Awake()
	{
		_instance = this;
	}

	void Start(){
		audi = gameObject.GetComponent<AudioSource>();
		GetAllTheCow();
	}
	
	// Update is called once per frame
	void Update () {
		cowCounterT.text = counter.ToString();
	}

	#endregion

	#region Public Methods
	public void IncreaseCow(CowAI cow){
		listCow.Remove(cow);
		counter++;
		powerBar.baseSize -= 0.1f;
		powerBar.baseSpeed += 0.5f;
	}

	public void WakeUpFarmer(){
		house.SleepDisturbance();
	}

	public void CowDropped(){
		dog.Noise();
		mainCamera.Shake();
		foreach(var cow in listCow)
		{
			cow.TriggerMove();
		}
	}

	public void GoToFrenzyMode(){
		if (!frenzyMode) {
			farmer.SetAggro();
			audi.clip = listAudio[1];
			audi.Play();
			frenzyMode = true;
		}
	}

	public void UFODestroyed(){
		Invoke("GameOverScreen",4f) ;
	}
	#endregion

	#region Private Method
	void GetAllTheCow(){
		foreach (var item in GameObject.FindGameObjectsWithTag("Cow")) {
			listCow.Add (item.GetComponent<CowAI>());
		}
	}

	void GameOverScreen(){
		levelManager.UFODestroyed();
	}

	#endregion

}
