using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CinematicScript : MonoBehaviour 
{

	public Camera mainCamera;
	public GameObject UI;
	public MovingUFO UFO;

	public Text mainText;
	public GameObject background;
	public LevelTimer timer;

	public AudioClip[] listAudioClip;
	public Texture[] listBackground;
	private string textToScreen;
	[SerializeField]
	private Vector3 middleScreen;
	private AudioSource auSou;

	private enum CinematicState
	{
		INIT,
		UFOENTER,
		DONE
	}

	CinematicState s;

	private string state = "";
	// Use this for initialization
	void Start ()
	{
		s = CinematicState.INIT;
		textToScreen = "Nov 2, 1989" + '\n' + "Infinite Pastures, Alabama";
		auSou = gameObject.GetComponent<AudioSource>();
		StartCoroutine(TypingText(textToScreen));
		UFO.transform.position =new Vector3(transform.position.x,10.0f,transform.position.z - 20.0f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(s == CinematicState.UFOENTER)
		{
			UFOAppear(new Vector3(transform.position.x,10.0f,transform.position.z));
			if(Vector3.Distance (UFO.transform.position,new Vector3(transform.position.x,10.0f,transform.position.z)) < 1.0f)
				s = CinematicState.DONE;
		}
		else if(s == CinematicState.DONE)
		{
			mainText.enabled = false;
			UI.gameObject.SetActive(true);
			timer.StartTimer();
			UFO.UFOControlling = true;
			Destroy(this);
		}
	}
	
	void UFOAppear(Vector3 destination){
		if ((UFO.transform.position - destination).magnitude >= 0.5f) {
				UFO.MoveToward(destination);
		}	
	}

	void SelfDestroy(){
		Destroy(gameObject,0.5f);
	}

	#region Helper Method
	void FocusTarget(){

	}

	IEnumerator TypingText(string inputText)
	{
		UFO.UFOControlling = false;
		UI.SetActive(false);
		foreach(char letter in  inputText.ToCharArray())
		{
			mainText.text += letter;
			auSou.PlayOneShot(listAudioClip[0]);

			yield return new WaitForSeconds(0.05f);
		}

		s = CinematicState.UFOENTER;
	}
	#endregion

}
