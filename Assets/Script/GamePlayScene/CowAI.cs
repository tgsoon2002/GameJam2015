using UnityEngine;
using System.Collections;

public class CowAI : MonoBehaviour
{
	#region Data Members

	public float radius;
	public Transform pasterCenter;

	private float cowSpeed = 2.5f;

	Vector2 direction;
	private Animator anim;
	private Vector3 destination;
	private Vector3 newPosition;
	private float moveTime;
	private bool isSucked = false;
	private bool isMoving = false;
	private Color spriteColor = new Color (1.0f, 1.0f, 1.0f, 0.0f);

	#endregion

	#region Built-in Unity Methods

	// Use this for initialization
	void Start ()
	{
		direction = new Vector2 ();
		destination = new Vector3 ();
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isMoving) {
			Move ();
			if ((gameObject.transform.position - destination).magnitude <= 0.2f) {
				StopMoving ();
			}
		}
	}

	#endregion

	#region Public Methods

	public void SuckItUp ()
	{
		isSucked = true;
		cowSpeed = 1.5f;
		//StopMoving ();
	}

	public void GainCounsious ()
	{
		isSucked = false;
		StopMoving ();
	}

	public void TriggerMove ()
	{
		anim.SetBool ("sleeping", false);
		if (!isSucked && !isMoving) {
			MoveToNewLocation ();
			isMoving = true;
			cowSpeed = 2.5f;
		}
	}

	public void StopMoving ()
	{
		isMoving = false;
		anim.SetBool ("notMoving", true);
	}

	#endregion

	#region Private Methods

	public void FadeIn ()
	{
		this.GetComponent<SpriteRenderer> ().color = spriteColor;
		StartCoroutine (ChangeAlpha ());
	}

	IEnumerator ChangeAlpha ()
	{
		while (this.GetComponent<SpriteRenderer> ().color.a != 1.0f) {
			yield return new WaitForSeconds (0.1f);
			spriteColor.a += 0.1f;
			this.GetComponent<SpriteRenderer> ().color = spriteColor;
		}

	}

	#endregion

	#region HelperMethod

	// Call by trigger move
	// Find new destination and set animation to move.set value in animator for blend tre,
	void MoveToNewLocation ()
	{
		isMoving = true;
		destination = new Vector3 (pasterCenter.position.x + Random.Range (-radius, radius), 0.0f, 
			pasterCenter.position.z + Random.Range (-radius, radius));
		anim.SetBool ("notMoving", false);
		direction = new Vector2 (destination.x - gameObject.transform.position.x, destination.z - gameObject.transform.position.z);
		anim.SetFloat ("xDirect", direction.x);
		anim.SetFloat ("yDirect", direction.y);	
	}

	 
	/// <summary>
	/// Move this instance.
	/// call in update
	/// move the cow position to new place,
	/// </summary>
	void Move ()
	{
		Vector3 direction = transform.position - destination;
		direction.y = transform.GetComponent<Rigidbody> ().velocity.y;

		GetComponent<Rigidbody> ().velocity = direction * Time.deltaTime * cowSpeed;
	}

	#endregion
}
