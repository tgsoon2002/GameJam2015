using UnityEngine;
using System.Collections;

public class BeamRing : MonoBehaviour
{

	#region Data Members

	[SerializeField]
	private float RingSpeed;
	private Vector3 originLocation;
	private bool addedForces = false;
	private MovingUFO parentControl;


	#endregion

	#region Setters & Getters (empty)

	#endregion

	#region Built-in Unity Methods

	// Use this for initialization
	void Start ()
	{
		parentControl = transform.parent.GetComponent<MovingUFO> ();
		ResetLocation (transform.position);
	}

	void FixedUpdate ()
	{
		if (parentControl.UFOIsBeaming) {
			RingSpeed = RingSpeed + 0.15f;
			if ((transform.position - transform.parent.position).magnitude <= 1f) {
				LoopBeaming ();
			}
			Vector3 tempPos = new Vector3 (transform.parent.position.x - transform.position.x, RingSpeed * Time.deltaTime, transform.parent.position.z - transform.position.z);
			transform.Translate (tempPos);

		}

	}

	#endregion

	#region Public Methods

	private void LoopBeaming ()
	{
		addedForces = false;
		RingSpeed = 0.5f;
		transform.position = originLocation;
	}

	public void ResetLocation (Vector3 underUFOPosision)
	{
		RingSpeed = 0.5f;
		originLocation = underUFOPosision;
		LoopBeaming ();
	}

	#endregion

	#region Private Methods (empty)

	#endregion

	void OnTriggerStay (Collider col)
	{
		if (col.gameObject.CompareTag ("Cow") && parentControl.UFOIsBeaming) {
			if (Input.GetButtonDown ("AddForce") && !addedForces) {
				col.GetComponent<Rigidbody> ().AddForce (Vector3.up * 80, ForceMode.Impulse);
				addedForces = true;
			}
		}
	}





}
