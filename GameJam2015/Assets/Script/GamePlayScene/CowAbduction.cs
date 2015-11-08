using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CowAbduction : MonoBehaviour {

	#region Data Member
	public List<AudioClip> listAudio;

	CowAI ai;
	Rigidbody rid;
	Animator anim;


	public GameObject damnYouDog;
	public GameObject indicator;
	private bool abducted = false;
	private bool lifting = false;
	private AudioSource audiSource;
	#endregion

	#region built-in Method
	// Use this for initialization
	void Start () {
		rid = gameObject.GetComponent<Rigidbody>();
		audiSource = gameObject.GetComponent<AudioSource>();
		ai = gameObject.GetComponent<CowAI>();
		anim = gameObject.GetComponent<Animator>();
		damnYouDog = GameManager.instance.dog.gameObject;
	}
	#endregion

	#region Private Method
	// this method call when cow got drop, play sound and signal gamemanager cow got drop
	void GotDropped(){
		anim.SetBool("stunned",true);
		PlayThudSound();
		Invoke("PlayMooSound",0.75f);
		Invoke("BeingFree",3.0f);
		GameManager.instance.CowDropped();

	}
	// this method call when cow reach ufo and signal GameManger Cow got abducted
	void BeingAbducted(){
		anim.SetBool ("teleported",true);
		rid.Sleep();
		abducted = true;
		GameManager.instance.IncreaseCow(this.GetComponent<CowAI>());
		Destroy (gameObject,0.75f);
	}

	void BeingBeam(){
	
		gameObject.GetComponent<CowAI>().SuckItUp();
	}
	
	void BeingFree(){

		anim.SetBool("stunned",false);
		anim.SetBool("notMoving",true);
		gameObject.GetComponent<CowAI>().GainCounsious();
	}
	#endregion

	#region Public Method


	public void DestroyCowObject(){
		Destroy(this.gameObject);
	}
	#endregion

	#region helperMethod
	void PlayMooSound(){
		audiSource.clip = listAudio[0];
		audiSource.Play();
	}
	
	void PlayThudSound(){
		audiSource.clip = listAudio[1];
		audiSource.Play();
	}

	#endregion

	#region collision case
	// detect when ufo on top of the cow. || cow reach the ufo
	void OnTriggerEnter(Collider collision){
		if (collision.gameObject.CompareTag ("Player")) {
			indicator.SetActive (true);
		}
		if (collision.gameObject.CompareTag ("UFOBottom") && !abducted) {
			BeingAbducted();
			Debug.Log("Abducted");
		}
	}
	// detect when ufo get off the cow position.
	void OnTriggerExit(Collider collision)
	{
		if (collision.gameObject.CompareTag ("Player")) {
			indicator.SetActive (false);
		}
		if (collision.gameObject.CompareTag ("Ground")) {
			lifting = true;
		}
	}

	// will call when UFO stay on top of the cow
	void OnTriggerStay(Collider collision)
	{
		if (collision.gameObject.CompareTag ("Player")) {
			if (collision.GetComponent<MovingUFO>().UFOIsBeaming && lifting) {
				BeingBeam();

			}
		}
	}
	
	void OnCollisionEnter(Collision ground){
		if (ground.gameObject.CompareTag ("Ground")) {
			if (lifting == true) {
				GotDropped();
				lifting = false;
			}
		}
	}
	#endregion

}
