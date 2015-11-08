using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuardDog : MonoBehaviour
{
    #region Data Members

    private Animator anim;          //Reference the dog's Animator component
	private AudioSource auSour;          //Reference the dog's AudioSource component
	public ParticleSystem zParticle;
    public List<AudioClip> listClip;
    
    [SerializeField]
    private float stunDuration;

    [SerializeField]
    private int sleepMeter;         //Number of 'Z' icons on the dog's head, will
                                    //also represent that the dog is sleeping   
    [SerializeField]
    private bool isSleeping;        //Checks if the dog is sleeping

    [SerializeField]
    private bool isStunned;         //Checks if the dog is stunned

    [SerializeField]
    private bool barkIsAnnoying;    //If this is true, farmer's sleep is being disturbed

    #endregion

    #region Setters & Getters

    #endregion

    #region Built-in Unity Methods
    // Use this for initialization
	void Start () 
    {
        isStunned = false;
        isSleeping = true;
        sleepMeter = 3;
		stunDuration = 10.0f;
        anim = gameObject.GetComponent<Animator>();
		auSour = gameObject.GetComponent<AudioSource>();
	}

    #endregion

    #region Public Methods
 	// this Noise() will be call by gamemanager everytime the cow got dropped.
    public void Noise()
    {
        sleepMeter--;
		if (sleepMeter == 0) {
			anim.SetBool("wake",true);
			zParticle.Stop();
			Invoke ("StartNotice",1.0f);
		}
    }

    #endregion

    #region Private Methods
	// Will call this to when the dog wake up and repeat invoke barking().
	void StartNotice()
	{
		InvokeRepeating ("Barking",0,3.5f);
	}
	// This barking() call by invoke repeat. If dog not stunned 
	// call game manager to wakeupfarmer, if soud not play then play barking sound.
    void Barking()
    {
		if(!isStunned)
        {
			GameManager.instance.WakeUpFarmer();
			if (!auSour.isPlaying) {
				auSour.clip = listClip[0];
				auSour.loop = true;
				auSour.Play();
			}
        }
    }
	// Call this fucntion by collider when haystack hit the dog.
	// play whining sound and animation go back to sleep.
	// invoke  GainConsious() with stunduration.
	void DogGetStunned(){
		//sound
		auSour.clip = listClip[1];
		auSour.loop = false;
		auSour.Play();
		//animation
		anim.SetBool("stunned",true);
		isStunned = true;
		Invoke("GainConsious",stunDuration);
	}
	// This GainCounsious() call by invoke in DogGetStunned()
	// play the barking sound and set animation barking and keep annoying the farmer.
	void GainConsious(){
		auSour.clip = listClip[0];
		auSour.loop = true;
		auSour.Play();
		anim.SetBool("stunned",false);
		isStunned = false;
	}

	#endregion
	
	#region Collision method
	void OnCollisionEnter(Collision hayStack){
		if (hayStack.gameObject.name == "HayStack") {
			hayStack.gameObject.GetComponent<HayStackScript>().HitObeject();
			DogGetStunned();
		}
	}
	#endregion

}
