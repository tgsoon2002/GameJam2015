using UnityEngine;
using System.Collections;

public class HayStackScript : MonoBehaviour {

	#region DataMember
	public MovingUFO beingBeam;
	private Vector3 originPosition;
	private Vector3 tempPosition;
	private bool hitTheDog = false;
	public GameObject indicator;
	#endregion

	#region built-in Method
	// Use this for initialization
	void Start () {
		originPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (beingBeam != null && !hitTheDog) {
			originPosition = beingBeam.transform.position;
			originPosition.y = transform.position.y;
			transform.position = originPosition;
			if (!beingBeam.UFOIsBeaming) {
				beingBeam = null;
			}

		}


	}
	#endregion

	#region Public Method
	public void HitObeject(){
		gameObject.GetComponent<AudioSource>().Play();
		gameObject.GetComponent<Animator>().SetTrigger("hitTarget");
		Destroy (gameObject,1.5f);
	}
	#endregion

	void OnTriggerEnter(Collider ufo){
		if (ufo.gameObject.CompareTag("Player")) {
			indicator.SetActive(true);
		}
	}
	void OnTriggerExit(Collider ufo){
		if (ufo.gameObject.CompareTag("Player")) {
			indicator.SetActive(false);
		}
	}




}
