using UnityEngine;
using System.Collections;

public enum AIState
{
    OBSERVING,
    CHASING,
    FIRING,
    STUNNED
}

public class FarmerAI : MonoBehaviour
{
    #region Data Members
    
 	private Transform ufoTransform ;
    private NavMeshPath path;
    private FarmerNick farmerBody;
    private FiniteStateMachine<AIState> farmerBrain;
    private delegate void HandleState(AIState s);
    private HandleState runState;
    
    #endregion

    #region MyRegion
	
	#endregion

    #region Built-In Method
    
    // Use this for initialization
	void Start () 
    {
		GameManager.instance.farmer = this;
		ufoTransform = GameManager.instance.UFO.gameObject.transform;
        path = new NavMeshPath();
	    farmerBody = gameObject.GetComponent<FarmerNick>();
        farmerBrain = new FiniteStateMachine<AIState>();
        farmerBrain.PushState(AIState.OBSERVING);
        runState += Observing;
        runState += Chasing;
        runState += Attacking;
        runState += Stunned;
		farmerBody.ChangeBubleText (0);

	}
	
	// Update is called once per frame
	void Update () 
    {
        runState(farmerBrain.CurrentState());
    }

    #endregion

    #region Public Methods

    public void SetAggro()
    {
        if(farmerBrain.CurrentState() == AIState.OBSERVING)
        {

            farmerBrain.PopState();
            farmerBrain.PushState(AIState.CHASING);
			farmerBody.ChangeBubleText(2);
			farmerBody.StartTalking();
			Invoke ("ChangeBubleToAgro",0.8f);
        }
    }

    public void SetStunned()
    {
        farmerBrain.PushState(AIState.STUNNED);
        StartCoroutine(StunDuration());
    }
    
    #endregion

    #region State Methods

    void Observing(AIState s)
    {
        if(s == AIState.OBSERVING)
        {
            //TO DO -- Insert some code here that will initially set the farmer's move speed to not chase the UFO,
            //         but simply just follow and observe. Perhaps, change animation as well if we can.

			if(!CheckDistance(farmerBody.Attack_Range))
            {
				GetPath();
				farmerBody.MoveFarmer(path.corners[1]);
            }
        }
    }

    /// <summary>
    /// Chasing state, means the farmer is now in aggro mode.
    /// Once in aggro mode, this state will be the default state
    /// of the farmer for the rest of the level.
    /// </summary>
    /// <param name="s"></param>
    void Chasing(AIState s)
    {
        if(s == AIState.CHASING)
        {
            //If the calculated distance between the UFO's ground position and the farmer is greater than the farmer's
            //attack range, the farmer will chase the UFO.
			if(!CheckDistance(farmerBody.Attack_Range))
            {
                GetPath();
				farmerBody.MoveFarmer(path.corners[1]);            
            }
            else
            {
                farmerBrain.PushState(AIState.FIRING);
            }
        }
    }

    /// <summary>
    /// Attacking state, means that the UFO is within the farmer's
    /// firing range. The farmer will fire up two twice, then after
    /// that the it will switch to Reloading state.
    /// </summary>
    /// <param name="s"></param>
    void Attacking(AIState s)
    {
        if(s == AIState.FIRING)
        {
            //TO DO -- Define some parameter for the number of times the farmer has fired his boom stick.
            //         The above implementation should be created within the farmer gun class.
			if(CheckDistance (farmerBody.Attack_Range))
            {
				farmerBody.Shoot();
            }
			else 
			{
				farmerBrain.PopState();
				farmerBrain.PushState(AIState.CHASING);
			}

        }
    }

    /// <summary>
    /// State when the farmer is stunned by dropping
    /// and object onto it.
    /// </summary>
    /// <param name="s"></param>
    void Stunned(AIState s){
        if(s == AIState.STUNNED)
        {
            farmerBody.StunTheFarmer(true);
        }
    }




    #endregion

	#region helper Method
	/// <summary>
	/// 
	/// </summary>
	void GetPath(){
		//TO DO -- Call GameManager to check if the UFO is moving
		if(true){
			//Getting the UFO position.  
			Vector3 tmp = GameManager.instance.UFO.gameObject.transform.position;
			
			//Transforming the UFO's position to the same coordinate plane of the farmer's.
			tmp.y = gameObject.transform.position.y;
			
			NavMesh.CalculatePath(gameObject.transform.position, tmp, NavMesh.AllAreas, path);
		}
	}

	/// <summary>
	/// Check the if ufo is in range of the distance
	/// </summary>
	bool CheckDistance(float distance){
		return (Vector3.Distance(gameObject.transform.position,new Vector3(ufoTransform.position.x, 0.0f,ufoTransform.position.z)) <= distance);
	}

	/// <summary>
	/// Check the if ufo is in range of the distance
	/// </summary>
	void ChangeBubleToAgro(){
		farmerBody.ChangeBubleText (1);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	IEnumerator StunDuration()
	{
		yield return new WaitForSeconds(10.0f);
		farmerBody.StunTheFarmer(false);
		farmerBrain.PopState();
	}
	#endregion

	#region Collider Method
	void OnCollisionEnter(Collision hayStack){
		if (hayStack.gameObject.name == "HayStack") {
			hayStack.gameObject.GetComponent<HayStackScript>().HitObeject();
			SetStunned();
		}
	}
	#endregion

}
