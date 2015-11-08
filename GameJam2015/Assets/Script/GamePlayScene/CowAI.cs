using UnityEngine;
using System.Collections;

public class CowAI : MonoBehaviour
{
    #region Data Members
    
    public float radius;
    public Transform center;

	Rigidbody physx;
	Vector2 direction;
  
    private Animator anim;

    [SerializeField]
    private Vector3 destination;        

    [SerializeField]
    private Vector3 newPosition;

    [SerializeField]
    private float moveTime;

    [SerializeField]
    private bool isSucked;

    [SerializeField]
    private bool isMoving;

	private bool nearFence;

    #endregion

    #region Setters & Getters
    
    #endregion

    #region Built-in Unity Methods
    
    // Use this for initialization
	void Start () 
    {
		direction = new Vector2();
        destination = new Vector3();
//        direction.x = (float)(Random.Range(-1, 1));
//        direction.z = (float)(Random.Range(-1, 1));
		anim = gameObject.GetComponent<Animator>();
		physx = gameObject.GetComponent<Rigidbody>();
        //Getting the direction vector
//        direction = new Vector3(direction.x - gameObject.transform.position.x, gameObject.transform.position.y,
//                                direction.z - gameObject.transform.position.z);
        isSucked = false;
        isMoving = false;

	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(isMoving)
        {
			Move();
			if ( (gameObject.transform.position - destination).magnitude <= 0.2f) {
				isMoving = false;
				anim.SetBool("notMoving",true );
			}
        }
    }

    #endregion

    #region Public Methods

    public void TriggerMove()
    {
		anim.SetBool("sleeping",false);
        if(!isSucked)
        {
            isMoving = true;
			DetermineDirection();
        }
    }

    public void SuckItUp()
    {
        isSucked = true;
        isMoving = false;
    }

	public void GainCounsious(){
		isSucked = false;
		isMoving = false;
	}

    #endregion

    #region Private Methods
    
    void DetermineDirection()
    {
		destination = new Vector3(center.position.x + Random.Range(-radius, radius), 
		                        0.0f, 
		                        center.position.z + Random.Range(-radius, radius));
		anim.SetBool ("notMoving",false );
    }
	
    void Move()
    {
		gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination, 1.5f*Time.deltaTime);
		direction =new Vector2(destination.x - gameObject.transform.position.x,destination.z - gameObject.transform.position.z);
		anim.SetFloat ("xDirect",direction.x );
		anim.SetFloat ("yDirect",direction.y );	


	}
	#endregion

	#region Collider and collision
	
//	void OnCollision(Collider col)
//	{   
//		if(col.tag == "ground" && isSucked)
//		{
//			isSucked = false; 
//		}
//	}
	// trigger when meet the fence, make the cow stop moving. and flag when cow near fence
	void OnTriggerEnter(Collider fence){
		if (fence.gameObject.CompareTag ("Fence") && !nearFence) {
			nearFence = true;
			isMoving = false;
		}
	}
	// when the cow far away from fence, turn off the flag.
	void OnTriggerExit(Collider fence){
		if (fence.gameObject.CompareTag ("Fence")) {
			nearFence = false;
		}
	}
	#endregion
}
