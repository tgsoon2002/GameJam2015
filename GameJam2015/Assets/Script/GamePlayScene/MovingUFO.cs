using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MovingUFO : MonoBehaviour {

	#region DataMember
	float horizontalMove,verticalMove;
	private bool isBeaming = false;
	private bool beamingHayStack = false;

	private float UFOSpeed = 8.0f;
	public GameObject beamRay;
	public GameObject forceBar;

	public Text warningText;
	private int health = 5;
	private bool justGotHit = false;
	private float beamForce = 0.0f;
	private bool gEngineFail = false;
	private bool UFOmoving =false;
	private bool controlable = true;
	Animator anim;
	#endregion

	#region getter setter
	public float UFOBeamForce{
		set { beamForce = value;} 
	}
	public bool UFOIsBeaming{
		get { return isBeaming;}
	}
	public bool UFOIsMoving{
		get { return UFOmoving;}
	}

	public bool UFOControlling{
		set { controlable = value;} 
	}
	#endregion

	#region built-in Method
	// Use this for initialization
	void Start () {
		horizontalMove = 0;
		verticalMove = 0;
		anim= gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Beamming") > 0.0f && !gEngineFail) {
			beamRay.SetActive(true);
			isBeaming = true;
			if (forceBar.activeSelf == false) {
				forceBar.SetActive(true);
			}
			if (GameManager.instance.farmer != null) {
				GameManager.instance.GoToFrenzyMode();
			}
		}
		else {
			beamForce = 0.0f;
			beamRay.SetActive(false);	
			forceBar.SetActive(false);
			isBeaming = false;
			forceBar.GetComponent<BarMovement2>().ResetBeamDevice();
		}


		if (isBeaming && !beamingHayStack) {
			horizontalMove = 0;
			verticalMove = 0;
		}
		else  {
			horizontalMove = Input.GetAxis("Horizontal");
			verticalMove = Input.GetAxis("Vertical");
		}

		//if UFO moving then set to moving bool to true
		if (horizontalMove != 0 || verticalMove != 0) {
			UFOmoving = true;
		}
		else {
			UFOmoving = false;
		}


		if ( health > 0 && controlable) {
			MoveUFO();	
		}

	}
	#endregion

	#region private Method
	void MoveUFO(){
		this.GetComponent<Rigidbody>().velocity = new Vector3(horizontalMove * UFOSpeed,0,verticalMove * UFOSpeed);
	}
	void UFODie(){
		gameObject.GetComponent<ParticleSystem>().Play();
		gameObject.GetComponent<Rigidbody>().useGravity = true;
		GameManager.instance.UFODestroyed();
	}

	void TakeDamage(){
		if (!justGotHit) {
			health--;
			StartCoroutine("JustGotHit");
			if (health<=0) {
				UFODie();
			}
		}

	}
	#endregion

	#region Public Method
	public void FailGEngine(){
		Debug.Log("Fail beaming");
		gEngineFail =true;
		warningText.text = "Gravity\nEngine\n Fail";
		Invoke("ResetGEgnine",1.0f);
	}

	public void MoveToward(Vector3 destination){
		isBeaming = true;
		gameObject.transform.position = Vector3.MoveTowards(transform.position, destination,  4f*Time.deltaTime);
	}

	#endregion


	#region Helper Method
	void ResetGEgnine(){
		gEngineFail = false;
		warningText.text = "";
	}

	IEnumerator JustGotHit(){
		justGotHit = true;
		anim.SetBool("JustGotHit",true);
		yield return new WaitForSeconds(1f);
		anim.SetBool("JustGotHit",false);
		justGotHit = false;
	}
	#endregion
	#region Collision Method
	void OnTriggerStay(Collider collision){
		
		if (collision.gameObject.CompareTag ("Cow") || collision.gameObject.CompareTag ("HayStack") ) {
			if (isBeaming) {
				collision.GetComponent<Rigidbody>().drag = 20.0f;
				if (beamForce > 0) {
					collision.GetComponent<Rigidbody>().AddForce(Vector3.up * beamForce,ForceMode.Impulse);
					beamForce = 0;
				}
				if (collision.gameObject.CompareTag ("HayStack")) {
					beamingHayStack = true;
					collision.GetComponent<HayStackScript>().beingBeam = this;
				}
			}
			else {
				collision.GetComponent<Rigidbody>().drag = 2.0f;
				beamingHayStack = false;
			}
			
		}
	}

	void OnTriggerEnter(Collider bullet){
		if (bullet.gameObject.CompareTag("Bullet")) {
			Destroy (bullet.gameObject);
			TakeDamage();

		}
	}
	#endregion


}

