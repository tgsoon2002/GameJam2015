using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.CompilerServices;

public class MovingUFO : MonoBehaviour
{

	#region DataMember

	float horizontalMove, verticalMove;
	private bool isBeaming = false;
	private bool beamingHayStack = false;

	private float UFOSpeed = 8.0f;
	private float carryingSpeed = 3.0f;
	private float nonCarryingSpeed = 8.0f;


	public GameObject forceBar;
	public Transform shadowTransform;
	public Text warningText;
	private int health = 5;
	private bool justGotHit = false;
	private float beamForce = 0.0f;
	private bool gEngineFail = false;
	private bool UFOmoving = false;
	[SerializeField]
	private bool controlable = true;


	private BeamRing r_BeamRing;
	private GameObject r_BeamLight;

	private bool turnOn = false;


	Animator anim;

	#endregion

	#region getter setter

	public bool UFOIsBeaming {
		get { return isBeaming; }
	}

	public bool UFOIsMoving {
		get { return UFOmoving; }
	}

	public bool UFOControlling {
		set { controlable = value; } 
	}

	#endregion

	#region built-in Method

	// Use this for initialization
	void Start ()
	{
		r_BeamRing = transform.GetChild (3).GetComponent<BeamRing> ();
		r_BeamLight = transform.GetChild (0).gameObject;
	
		horizontalMove = 0;
		verticalMove = 0;
		anim = gameObject.GetComponent<Animator> ();
		//Cursor.visible = false;
	}

	// FixedUpdate is called once per frame. timescale = 0 will stop the fixed update
	void FixedUpdate ()
	{
		// check if UFO can be controll, not controllable if dead of pausing
		if (controlable && health > 0) {
			HandleBeamming ();
			MoveUFO ();	
		} else {
			UFOBeamingOff ();
		}

		//if UFO moving then set to moving bool to true
		if (horizontalMove != 0 || verticalMove != 0) {
			UFOmoving = true;
		} else {
			UFOmoving = false;
		}
	}

	#endregion

	#region private Method

	/// <summary>
	/// Handles the beamming.
	/// 
	/// </summary>
	void HandleBeamming ()
	{
		if (Input.GetAxis ("Beamming") > 0.0f && !gEngineFail) {
			UFOBeamingOn ();
		} else {
			UFOBeamingOff ();
		}
	}

	/// <summary>
	/// Moves the UF.
	/// If UFO is not beaming haystack then move slow
	/// else move normal speed
	/// </summary>
	void MoveUFO ()
	{
		horizontalMove = Input.GetAxis ("Horizontal");
		verticalMove = Input.GetAxis ("Vertical");

		if (isBeaming && !beamingHayStack) {
			UFOSpeed = carryingSpeed;
		} else {
			UFOSpeed = nonCarryingSpeed;
		}

		this.GetComponent<Rigidbody> ().velocity = new Vector3 (horizontalMove * UFOSpeed, 0, verticalMove * UFOSpeed);
	}

	/// <summary>
	/// UFOs the die.
	/// Run animation and stop control. tell game manger fail mission.
	/// </summary>
	void UFODie ()
	{
		gameObject.GetComponent<ParticleSystem> ().Play ();
		gameObject.GetComponent<Rigidbody> ().useGravity = true;
		GameManager.instance.UFODestroyed ();
		controlable = false;
	}

	/// <summary>
	/// Takes the damage.
	/// </summary>
	void TakeDamage ()
	{
		if (!justGotHit) {
			health--;
			StartCoroutine ("JustGotHit");
			if (health <= 0) {
				UFODie ();
			}
		}
	}

	#endregion

	#region Public Method

	/// <summary>
	/// Moves the toward.
	/// Used for cinematic entrance
	/// </summary>
	/// <param name="destination">Destination.</param>
	public void MoveToward (Vector3 destination)
	{
		isBeaming = true;
		gameObject.transform.position = Vector3.MoveTowards (transform.position, destination, 4f * Time.deltaTime);
	}

	#endregion


	#region Helper Method

	void UFOBeamingOn ()
	{
		// set beam light and ring active and set is beaming to true
		r_BeamRing.gameObject.SetActive (true);
		r_BeamLight.SetActive (true);
		isBeaming = true;

		// if ring is just turn on first time then reset it
		if (!turnOn) {
			r_BeamRing.GetComponent<BeamRing> ().ResetLocation (shadowTransform.position);
			turnOn = true;
		}

		// if farmer is wake up then go to frenzy mode
		if (GameManager.instance.farmer != null) {
			GameManager.instance.GoToFrenzyMode ();
		}
	}

	void UFOBeamingOff ()
	{
		// set beam light and ring deactive. and set change is beaming to false.
		r_BeamRing.gameObject.SetActive (false);
		r_BeamLight.SetActive (false);
		isBeaming = false;

		// reset the flag for first time of beam turn on.
		turnOn = false;
	}

	IEnumerator JustGotHit ()
	{
		justGotHit = true;
		anim.SetBool ("JustGotHit", true);
		yield return new WaitForSeconds (1f);
		anim.SetBool ("JustGotHit", false);
		justGotHit = false;
	}

	#endregion

	#region Collision Method

	/// <summary>
	/// Raises the trigger stay event.
	/// Detect collide with cow or haystack
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnTriggerStay (Collider collision)
	{
		if (collision.gameObject.CompareTag ("Cow") || collision.gameObject.CompareTag ("HayStack")) {
			if (isBeaming) {
				
				if (collision.gameObject.CompareTag ("HayStack")) {
					beamingHayStack = true;
					collision.GetComponent<HayStackScript> ().beingBeam = this;
				}
			} else {
				collision.GetComponent<Rigidbody> ().drag = 2.0f;
				beamingHayStack = false;
			}
		}
	}

	/// <summary>
	/// Raises the collision enter event.
	/// Detect collide with bullet
	/// </summary>
	/// <param name="bullet">Bullet.</param>
	void OnCollisionEnter (Collision bullet)
	{
		if (bullet.gameObject.CompareTag ("Bullet")) {
			Destroy (bullet.gameObject);
			TakeDamage ();

		}
	}



	#endregion


}

