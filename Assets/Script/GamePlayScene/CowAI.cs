using UnityEngine;
using System.Collections;

public class CowAI : MonoBehaviour
{
    #region Data Members
    
    public float radius;
    public Transform center;
	
	Vector2 direction;
    private Animator anim;
    private Vector3 destination;        
    private Vector3 newPosition;
    private float moveTime;
    private bool isSucked = false;
	private bool isMoving = false;
	private bool nearFence = false;

    #endregion

    #region Built-in Unity Methods
    
    // Use this for initialization
	void Start () 
    {
		direction = new Vector2();
        destination = new Vector3();
		anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(isMoving)
        {
			Move();
			if ( (gameObject.transform.position - destination).magnitude <= 0.2f) {
				StopMoving();
			}
        }
	

    }

    #endregion

    #region Public Methods

    public void SuckItUp()
    {
        isSucked = true;
		StopMoving();
    }

	public void GainCounsious(){
		isSucked = false;
		StopMoving();
	}

	public void TriggerMove()
	{
		anim.SetBool("sleeping",false);
		if(!isSucked && !isMoving)
		{
			MoveToNewLocation();
			isMoving = true;
		}
	}

	public void BeingPull(Vector3 pullPoint){
		Vector2 groundPos = new Vector2 ( transform.position.x,transform.position.z);
		Vector2 groundPullPoint = new Vector2 (pullPoint.x, pullPoint.z);
		if ((groundPos-groundPullPoint).magnitude <= 0.5f) {
			transform.position = new Vector3(pullPoint.x,4.0f,pullPoint.z);
			Debug.Log(new Vector3(pullPoint.x,4.0f,pullPoint.z));
		}else if ((groundPos-groundPullPoint).magnitude >= 0.05f){
			gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, pullPoint, 2f*Time.deltaTime);
		}
	}
    #endregion

    #region Private Methods
	void StopMoving(){
		isMoving = false;
		anim.SetBool ("notMoving",true );

	}
	#endregion

	#region HelperMethod
	// Call by trigger move 
	// Find new destination and set animation to move.set value in animator for blend tre,
	void MoveToNewLocation()
	{
		isMoving = true;
		destination = new Vector3(center.position.x + Random.Range(-radius, radius),0.0f, 
		                          center.position.z + Random.Range(-radius, radius));
		anim.SetBool ("notMoving",false );
		direction =new Vector2(destination.x - gameObject.transform.position.x,destination.z - gameObject.transform.position.z);
		anim.SetFloat ("xDirect",direction.x );
		anim.SetFloat ("yDirect",direction.y );	
	}
	// call in update
	// move the cow position to new place,  
	void Move()
	{
		gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination, 1.5f*Time.deltaTime);
	}

	#endregion

	#region Collider and collision
	
	// trigger when meet the fence, and not near the fence 
	// Call StopMoving(). and flag when cow near fence
	void OnTriggerEnter(Collider fence){
		if (fence.gameObject.CompareTag ("Fence") && !nearFence) {
			nearFence = true;
			StopMoving();
		}
	}
	// when the cow far away from fence, 
	// turn off the flag.
	void OnTriggerExit(Collider fence){
		if (fence.gameObject.CompareTag ("Fence")) {
			nearFence = false;
		}
	}
	#endregion
}
