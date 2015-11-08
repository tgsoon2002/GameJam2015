using UnityEngine;
using System.Collections;

public class canvasManager : MonoBehaviour {


	public GameObject cowImage;

	void Update(){
		if (Input.GetButtonDown ("Submit")) {
			changeScene();
		}
	}
	// Use this for initialization
	public void DestroyCow(){
		Destroy (cowImage);
	}

	public void changeScene(){
		Application.LoadLevel (1);
	}
}
