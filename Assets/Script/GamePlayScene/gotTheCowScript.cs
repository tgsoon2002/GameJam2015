using UnityEngine;
using System.Collections;

public class gotTheCowScript : MonoBehaviour {

	AudioSource audi;
	// Use this for initialization
	void Start () {
		audi = gameObject.GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider cow){
		if (cow.CompareTag ("Cow") && !cow.GetComponent<CowAbduction>().CowIsAbducted) {
			cow.GetComponent<CowAbduction>().BeingAbducted();
			audi.Stop();
			audi.Play();	
		}
	}
}
