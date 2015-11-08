using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FarmerNick : MonoBehaviour
{
    #region Data Members

    public GunControl rifle;
	public SpriteRenderer bubleText;
	public List<AudioClip> listAudio;   
	public List<Sprite> listSprite;


    private Animator anim;
	private AudioSource audSour;


    private Collider[] hitColliders;
  
    [SerializeField]
    private float attackRange;

	[SerializeField]
	private float chasingSpeed;

    #endregion

    #region Setters & Getters
	public float Attack_Range
	{
		get { return attackRange; }
	}

    #endregion

    #region Built-in Unity Methods


    // Use this for initialization
	void Start () 
    {
        //farmerNavMesh = gameObject.GetComponent<NavMeshAgent>();

		audSour = gameObject.GetComponent<AudioSource>();
	    anim = gameObject.GetComponent<Animator>();
        attackRange = 25.0f;
		chasingSpeed = 1.5f;
	}
    #endregion

    #region Public Methods

	public void StunTheFarmer(bool val)
    {
		anim.SetBool ("Stunned",val);
    }

	public void StartTalking(){
		KeepPlayRandomClip();
	}

	public void Shoot(){
		rifle.Shoot();
	}
	
	public void MoveFarmer(Vector3 destination){
		gameObject.transform.position = Vector3.MoveTowards(transform.position, destination,  chasingSpeed*Time.deltaTime);
	}

	public void ChangeBubleText(int index){
		bubleText.sprite = listSprite[index];
	}
	
    #endregion

    #region Private Methods


	void PlayAudioClip(int clipIndex){
		audSour.clip = listAudio[clipIndex];
		audSour.Play();
	}

	void KeepPlayRandomClip(){
		int randomClip = Random.Range(0,listAudio.Count);
			PlayAudioClip(randomClip);
		Debug.Log(randomClip);	
		Invoke("KeepPlayRandomClip",listAudio[randomClip].length+1.5f);

	}
	
    #endregion
}
