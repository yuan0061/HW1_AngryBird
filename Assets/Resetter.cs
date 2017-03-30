using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Resetter : MonoBehaviour {

	public Rigidbody2D projectile;
	public float resetSpeed = 0.3f;
	public Camera mainCamera;
	public GameObject target;
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;
	public Text FailMessage;
	 

	private float resetSpeedSqr;
	private SpringJoint2D spring;
	private Vector3 projectilePosition;
	private Quaternion projectileRotation;
	private SpriteRenderer render;
	private Dragging dragging;

	// Use this for initialization
	void Start () {
		resetSpeedSqr = resetSpeed * resetSpeed;
		spring = projectile.GetComponent<SpringJoint2D> ();
		projectilePosition = projectile.transform.position;
		projectileRotation = projectile.transform.rotation;
		render = target.GetComponent<SpriteRenderer> ();
		dragging = projectile.GetComponent<Dragging> ();
		FailMessage.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			Reset ();
		}

		if (dragging.counter < 3 && dragging.counter > 0 && spring.enabled == false && projectile.velocity.sqrMagnitude < resetSpeedSqr && render.enabled == true) {
			
			ResetProjectile ();
			catapultLineFront.enabled = true;
			catapultLineBack.enabled = true;
			projectile.isKinematic = true;
			spring.enabled = true;

			//Dragging.LineRendererSetup ();
			//Dragging.rayToMouse = new Ray (catapult.position, Vector3.zero);
			//leftCatapultToProjectile = new Ray (catapultLineFront.transform.position, Vector3.zero);

		}
		if (dragging.counter > 2 && render.enabled == true && projectile.velocity.sqrMagnitude < resetSpeedSqr) {
			FailMessage.gameObject.SetActive (true);
		}

	}



	void OnTriggerExit2D(Collider2D other){
		if (dragging.counter < 3 && dragging.counter > 0 && other.GetComponent<Rigidbody2D>() == projectile /*&& spring.enabled == false*/) {
			ResetProjectile ();
			catapultLineFront.enabled = true;
			catapultLineBack.enabled = true;
			projectile.isKinematic = true;
			//spring.enabled = true;
		}
		if (dragging.counter > 2 && render.enabled == true) {
			FailMessage.gameObject.SetActive (true);
		}
	}

	void ResetProjectile(){
		projectile.transform.position = projectilePosition;
		projectile.transform.rotation = projectileRotation;
		if (projectile != null) {
			projectile.velocity = Vector3.zero;
			projectile.angularVelocity = 0f;
		}
	}

	void Reset(){
		//Application.LoadLevel (0);
		SceneManager.LoadScene("Main", LoadSceneMode.Single);
	}
}
