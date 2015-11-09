using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BarMovement2 : MonoBehaviour {

	#region Data Member
	public float dotDirection = -1.0f;
	public float dotSpeed = 2.0f;
	//public Transform redBar;
	public Transform greenBar;
	public MovingUFO UFO ;

	public float baseSpeed = 2.0f;
	public float baseSize = 1.0f;
	
	Slider slide;


	private float beamForce = 110f;
	private Rigidbody2D physx;
	private float changedGreenBarSize;
	private float oldGreenBarSize;
	public bool happyPlace = false;
	private int failTime = 2;
	public bool hasPressedSpace = false;
	#endregion

	#region Built-In Method
	// Use this for initialization
	void Start () {
		slide = gameObject.GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () 
	{

		MoveDot ();
		if (Input.GetButtonDown("AddForce")&& failTime > 0) {
			if (slide.value >= 40.0f && slide.value <=60.0f && !hasPressedSpace) {
				hasPressedSpace = true;
				SuccessBeam();
			}
			else {
				FailBeam();
			}
		}
		if ((slide.value < 30.0f || slide.value >70.0f) && hasPressedSpace) {
			hasPressedSpace = false;
		}
	}
	#endregion

	#region Private Method
	// when play fail to beam at right time, will have check for the make engine not work.
	void FailBeam(){
		failTime -- ;
		if (failTime == 0) {
			RecoverFromFailEngine();
		}
	}
	// Call this function when player success beam, change beam force, add speed, small size.
	void SuccessBeam(){
		UFO.AddForce(beamForce);
		dotSpeed += 0.3f;
		oldGreenBarSize = greenBar.localScale.y;
		changedGreenBarSize = oldGreenBarSize * 0.9f;
		greenBar.localScale = new Vector3(1.0f,changedGreenBarSize);
	}
	// call this function in update. make slide change value.
	void MoveDot() {
		if (slide.value == slide.minValue || slide.value == slide.maxValue) {
			dotDirection *= -1.0f;
		}
		slide.value += dotDirection * dotSpeed;
	}
	#endregion

	#region Public Method
	public void ResetBeamDevice(){

		dotSpeed = baseSpeed;
		greenBar.localScale = new Vector3(baseSize, 1.0f);
	}
	#endregion

	#region Helper Method
	void RecoverFromFailEngine(){
		UFO.FailGEngine();
		failTime = 2;
		ResetBeamDevice();
	}
	#endregion


}
