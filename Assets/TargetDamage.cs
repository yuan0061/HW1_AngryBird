using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TargetDamage : MonoBehaviour {

	public int hitPoints = 1;
	public Sprite damagedSprite;
	public float damageImpactSpeed;
	public ParticleSystem partical;
	public AudioSource dieSound;
	public Text PassMessage;

	private int currentHitPoints;
	private float damageImpactSpeedSqr;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		currentHitPoints = hitPoints;
		damageImpactSpeedSqr = damageImpactSpeed * damageImpactSpeed;
		PassMessage.gameObject.SetActive (false);

	}
		
	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.tag != "Damager")
			return;
		//if (collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr)
		//	return;
		
		spriteRenderer.sprite = damagedSprite;
		currentHitPoints--;

		if (currentHitPoints <= 1) {
			Kill ();
			Invoke ("NextLevelOn", 5);
		}
			
			
	}

	void Kill(){
		dieSound.Play ();
		spriteRenderer.enabled = false;
		GetComponent<Collider2D>().enabled = false;
		GetComponent<Rigidbody2D>().isKinematic = true;
		partical.transform.position = this.transform.position;
		partical.gameObject.SetActive(true);
		PassMessage.gameObject.SetActive (true);

	}



	public void NextLevelOn(){
		//Application.LoadLevel (1);
		SceneManager.LoadScene("Second", LoadSceneMode.Single);
	}
}
