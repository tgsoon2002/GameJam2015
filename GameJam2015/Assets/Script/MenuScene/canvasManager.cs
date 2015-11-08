using UnityEngine;
using System.Collections;

public class canvasManager : MonoBehaviour {


	public GameObject cowImage;
	// Use this for initialization
	public void DestroyCow(){
		Destroy (cowImage);
	}

	public void changeScene(){
		Application.LoadLevel (1);
	}
}
