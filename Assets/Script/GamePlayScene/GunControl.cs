using UnityEngine;
using System.Collections;

public class GunControl : MonoBehaviour {

	public GameObject bullet;
	public Transform rifleTip;

	public bool gunAimed = true;
	public bool reloading = false;
	private float bulletSpeed = 700.0f;
	public Transform target;
	[SerializeField]
	private int bulletInGun = 2;
	Animator anim;
	void Start(){
		anim = gameObject.GetComponent<Animator>();
		target = GameManager.instance.UFO.transform;
	}

	public void Shoot(){
		// if gun is aimed and still have bullet
		if (gunAimed && bulletInGun != 0) {

			anim.SetTrigger("Shoot");
			gunAimed = false;
			Invoke("Aimed",2f);
		}
		//if gun run out of bullet then reload
		else if(bulletInGun == 0 && !reloading){
			//bulletInGun = -1;
			anim.SetTrigger("Reload");
			reloading = true;
			//Invoke("DoneReload",2f);
		}
	}

	// only instatinate bullet a fix animation frame
	public void CreateBullet(){
		GameObject bull = Instantiate (bullet,rifleTip.position,rifleTip.rotation) as GameObject;
		bull.transform.LookAt (target.position);
		bull.GetComponent<Rigidbody>().AddForce(bull.transform.forward * bulletSpeed);
		bulletInGun--;
		Destroy (bull,2.5f);
	}

	public void DoneReload(){

		bulletInGun = 2;
		reloading = false;
	}

	void Aimed(){
		Debug.Log("ready");
		gunAimed = true;
	}
}
