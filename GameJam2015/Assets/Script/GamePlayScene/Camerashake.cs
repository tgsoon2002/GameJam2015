using UnityEngine;
using System.Collections;
public class Camerashake : MonoBehaviour
{
	#region MyRegion
	public float shake_decay;
	public float shake_intensity;

	private float cameraSpeed = 0.5f;
	private Vector3 originPosition;
	private Vector3 tempPosition;
	
	#endregion

	#region built-in method
	void Start(){
		originPosition = transform.position;
		tempPosition = new Vector3();
		//Debug.Log("initial x and y: "+initialPosition.x+" "+ initialPosition.y);
		//Debug.Log ("initial x and y: "+initialPosition.position.x +" , "+initialPosition.position.y);
	}
	
	
	
	void Update (){

		if (shake_intensity > 0 ){
			transform.position = originPosition + (Random.insideUnitSphere * shake_intensity);
			shake_intensity -= shake_decay;
		}
		if (shake_intensity <= 0.0f)
		{
			transform.position = originPosition;
		}
		MoveCamera();

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
	#endregion

	
	
}