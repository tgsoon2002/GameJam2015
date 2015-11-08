using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.ImageEffects;

public class GameManager : MonoBehaviour {

	#region Data Member
	public Text cowCounterT;
	public GuardDog dog;
	public BarMovement2 powerBar;
	public MovingUFO UFO;
	public FarmerAI farmer;
	public CameraController mainCamera;
	public LevelTimer levelManager;
	public FarmerSleepingInHouse farmerHouse;
	private BlurOptimized blurEffect;


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
		blurEffect = mainCamera.GetComponent<BlurOptimized>();
		cowCounterT.text = counter.ToString();
	}
	
	#endregion

	#region Public Methods
	// Called by ufo if cow reach the bottom of ufo.
	// remove the cow from the list, increase the number of cow, increase the difficulty of the beamming.
	public void IncreaseCow(CowAI cow){
		listCow.Remove(cow);
	
		counter++;
		cowCounterT.text = counter.ToString();
		powerBar.baseSize -= 0.1f;
		powerBar.baseSpeed += 0.5f;
	}
	// Called by the dog. on interval.
	// CAll fucntion sleep disturbance of the house.
	public void WakeUpFarmer(){
		if (house != null) {
			house.SleepDisturbance();	
		}
	}
	// Called by the cow being dropped, make noise to the dog and move all other cow around.
	// call the fucntoin noise of the dog, call camera shake, go through list of cow and make all of them move.
	public void CowDropped(){
		dog.Noise();
		mainCamera.Shake();
		foreach(var cow in listCow)
		{
			cow.TriggerMove();
		}
	}
	// Called when ufo beaming and farmer is out side the house.
	// set farmer to aggro mode and change music to frenzy track.
	public void GoToFrenzyMode(){
		if (!frenzyMode) {
			farmer.SetAggro();
			PlayAudioClip(1,true);
			frenzyMode = true;
		}
	}
	// Called by UFO when UFO health = 0 
	// Call function game over screen after 4 second, make timer stop
	public void UFODestroyed(){
		PlayAudioClip(2,false);
		UFO.UFOControlling = false;
		Invoke("GameOverScreen",4f) ;
		levelManager.TimerRunning(false);
	}
	// Called by level timer to pause the game. 
	// Active or deactive the blur effect of camera.
	public void PauseGame(bool val){
		blurEffect.enabled = val;
	}
	#endregion

	#region Private Method

	void PlayAudioClip(int clip, bool loopable){
		audi.loop = loopable;
		audi.clip = listAudio[clip];
		audi.Play();
	}

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
