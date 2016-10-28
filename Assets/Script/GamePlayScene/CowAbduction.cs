using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CowAbduction : MonoBehaviour
{

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
	private bool nearFence = false;

	#endregion

	#region getter setter

	public bool CowIsAbducted {
		get{ return abducted; }
	}

	#endregion

	#region built-in Method

	// Use this for initialization
	void Start ()
	{
		rid = gameObject.GetComponent<Rigidbody> ();
		audiSource = gameObject.GetComponent<AudioSource> ();
		ai = gameObject.GetComponent<CowAI> ();
		anim = gameObject.GetComponent<Animator> ();
		damnYouDog = GameManager.instance.dog.gameObject;
	}

	#endregion

	#region Private Method

	// this method call when cow got drop, play sound and signal gamemanager cow got drop
	void GotDropped ()
	{
		anim.SetBool ("stunned", true);
		PlayThudSound ();
		ai.StopMoving ();
		Invoke ("PlayMooSound", 0.75f);
		Invoke ("BeingFree", 3.0f);
		GameManager.instance.CowDropped ();
	}

	/// <summary>
	/// Beings the beam.
	/// this happen when cow is being beam and 
	/// </summary>
	void BeingBeam ()
	{
		ai.SuckItUp ();
	}

	void BeingFree ()
	{
		anim.SetBool ("stunned", false);
		anim.SetBool ("notMoving", true);
		ai.GainCounsious ();
	}

	#endregion

	#region Public Method

	// this method call when cow reach ufo and signal GameManger Cow got abducted
	public void BeingAbducted ()
	{
		if (!abducted) {
			anim.SetBool ("teleported", true);
			rid.Sleep ();
			abducted = true;
			GameManager.instance.IncreaseCow (ai);
			Destroy (gameObject, 0.75f);
		}
	}

	#endregion

	#region helperMethod

	void PlayMooSound ()
	{
		audiSource.clip = listAudio [0];
		audiSource.Play ();
	}

	void PlayThudSound ()
	{
		audiSource.clip = listAudio [1];
		audiSource.Play ();
	}



	#endregion

	#region collision case

	// Trigger when Cow enter a collision
	//-> trigger when cow enter UFO beaming area.
	//->  trigger when cow get back to the ground
	//->  trigger when cow near the fence.
	void OnTriggerEnter (Collider collision)
	{
		if (collision.gameObject.CompareTag ("Player")) {
			indicator.SetActive (true);
		}
		if (collision.gameObject.CompareTag ("Ground")) {
			if (lifting == true) {
				GotDropped ();
				lifting = false;
			}
		}
		if (collision.gameObject.CompareTag ("Fence") && !nearFence) {
			nearFence = true;
			ai.StopMoving ();
		}
	}
	// Trigger when Cow enter a collision
	//-> trigger when cow Exit UFO beaming area.
	//-> trigger when cow far away from the ground.
	//-> trigger when cow get away from the fence
	void OnTriggerExit (Collider collision)
	{
		if (collision.gameObject.CompareTag ("Player")) {
			indicator.SetActive (false);
			GetComponent<Rigidbody> ().drag = 2.0f;
		}
		if (collision.gameObject.CompareTag ("Ground")) {
			lifting = true;
		}
		if (collision.gameObject.CompareTag ("Fence")) {
			nearFence = false;
		}

	}
	// Trigger when Cow stay in a collision ;trigger every frame
	// -> if player beaming, cow will be lift of
	// -> also make cow move away.
	void OnTriggerStay (Collider collision)
	{
		if (collision.gameObject.CompareTag ("Player")) {
			if (collision.GetComponent<MovingUFO> ().UFOIsBeaming) {
				GetComponent<Rigidbody> ().drag = 20.0f;
				//If the cow is pull up and enough distance from the ground. the cow is being lift.
				// else it will move to get away from the beam light.
				if (lifting) {
					BeingBeam ();
				} else if (true) {
					ai.TriggerMove ();
				}	
			} else {
				GetComponent<Rigidbody> ().drag = 2.0f;
			}
		}
	}

	#endregion

}
