using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragging : MonoBehaviour {
	public float maxStretch = 3.0f;
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;
	public AudioSource shootSound;
	public int counter = 0;

	private Transform catapult;
	private SpringJoint2D spring;
	private Rigidbody2D asteroid;
	private bool clickedOn;
	private Ray rayToMouse;
	private float maxStretchSqr;
	private Vector2 prevVelocity;
	private Ray leftCatapultToProjectile;
	private float circleRadius;
	private CircleCollider2D circle;


	public void Awake(){
		spring = GetComponent <SpringJoint2D> ();
		catapult = spring.connectedBody.transform;
	}

	public void Start () {
		
		asteroid = GetComponent <Rigidbody2D> ();
		circle = GetComponent<CircleCollider2D> ();

		LineRendererSetup ();
		rayToMouse = new Ray (catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray (catapultLineFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		circleRadius = circle.radius;

	}

	void Update () {
		
		
		if (clickedOn) {
            Drag ();
			//LineRendererUpdate ();
  
		}
		if (catapultLineFront.enabled == true) {
			LineRendererUpdate ();

		}
		if (spring.enabled == true) {
			
			//spring.enabled = false;
			//rayToMouse.direction = Vector3.zero;
			//leftCatapultToProjectile.direction = Vector3.zero;
			if (!asteroid.isKinematic && prevVelocity.sqrMagnitude > asteroid.velocity.sqrMagnitude) {
				counter++;
				spring.enabled = false;
				catapultLineFront.enabled = false;
				catapultLineBack.enabled = false;
				asteroid.velocity = prevVelocity;
			} 
			if (!clickedOn) 
				prevVelocity = asteroid.velocity;
			
		} 

	}

	public void LineRendererSetup(){
		catapultLineFront.SetPosition (0, catapultLineFront.transform.position);
		catapultLineBack.SetPosition (0, catapultLineBack.transform.position);

		catapultLineFront.sortingLayerName = "ForGround";
		catapultLineBack.sortingLayerName = "ForGround";

		catapultLineFront.sortingOrder = 3;
		catapultLineBack.sortingOrder = 1;
	}

	void OnMouseDown(){
		spring.enabled = false;
		clickedOn = true;
	}

	void OnMouseUp(){
		spring.enabled = true;
		asteroid.isKinematic = false;
		clickedOn = false;
		shootSound.Play ();
	}

	void Drag(){
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		}
		mouseWorldPoint.z = 0f;
		this.transform.position = mouseWorldPoint;
		//transform.position = new Vector3 (1, 1, 1);
		//Debug.Log (this);

	}
	
	public void LineRendererUpdate(){
		Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint (catapultToProjectile.magnitude + circleRadius);
		catapultLineFront.SetPosition (1, holdPoint);
		catapultLineBack.SetPosition (1, holdPoint);
	}

}
