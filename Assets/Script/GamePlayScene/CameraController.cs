using UnityEngine;
using System.Collections;
public class CameraController : MonoBehaviour
{
	#region Data Member
	private float shake_decay = 0.0f;
	private float shake_intensity = 0.0f;
	private float cameraSpeed = 0.5f;
	private Vector3 originPosition;
	private Vector3 tempPosition;
	private Transform ufoTransform;

	private bool controlable  = true;
	#endregion

	#region Getter and Setter

	public bool CameraControlable{
		set{ controlable = value;}
	}

	#endregion

	#region built-in method
	// set the origin position 
	void Start(){
		originPosition = transform.position;
		tempPosition = new Vector3();
		ufoTransform = GameManager.instance.UFO.transform;

	}
	// In this fucntion will handle the shaking of camera, move camera and set focus camera to ufo if press focus UFO btn
	// make tranform position to origin so any change only need to change "originPosition".
	void FixedUpdate (){
		if (shake_intensity > 0 ){
			transform.position = originPosition + (Random.insideUnitSphere * shake_intensity);
			shake_intensity -= shake_decay;
		}
		if (shake_intensity <= 0.0f)
		{
			transform.position = originPosition;
		}
		if (controlable) {
			MoveCamera();
			if (Input.GetButtonDown("focusUFO")) {
				FocusUFO();
			}
		}


	}

	#endregion

	#region Private Method
	void MoveCamera(){
		tempPosition.x = Input.GetAxis("CHorizontal") * cameraSpeed;
		tempPosition.z = Input.GetAxis("CVertical") * cameraSpeed;
		gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, originPosition + tempPosition, 1.5f*Time.deltaTime);
		originPosition += tempPosition;
	}
	#endregion

	#region Public Method
	public void Shake()
	{
		//initialPosition = transform.position;
		//initialRotation= this.transform.rotation;
		originPosition = transform.position;
		//originRotation = transform.rotation;
		shake_intensity = 0.1f;
		shake_decay = 0.002f;
	}

	public void FocusUFO(){
		originPosition = ufoTransform.position;
		originPosition.y = 20.0f;
		originPosition.z -= 25.0f;
		transform.position = originPosition;
	}

	public void FocusDog(){
		originPosition = GameManager.instance.dog.transform.position;
		originPosition.y = 20.0f;
		originPosition.z -= 40.0f;
		transform.position = originPosition;
		Debug.Log(originPosition);
	}

	public void FocusHouse(){
		originPosition = GameManager.instance.farmerHouse.transform.position;
		originPosition.y = 20.0f;
		originPosition.z -= 40.0f;
		transform.position = originPosition;
	}
	#endregion

	
	
}