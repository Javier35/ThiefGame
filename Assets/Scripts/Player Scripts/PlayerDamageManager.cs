using UnityEngine;
using System.Collections;

public class PlayerDamageManager : DamageManager {

	private LevelManager levelManager;
	private PlatformerCharacter2D player;
	//public BoxCollider2D attackBox;
	private float invincibilityTime = 1.5f;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager> ();
		player = GetComponent<PlatformerCharacter2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		//GameObject.Find ("Canvas").GetComponentInChildren<Animator> ().SetInteger ("Health", health);

//		if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack") && 
//			!animator.GetCurrentAnimatorStateInfo (0).IsName ("SecondAttack"))
//				DisableAttackBox ();
	}

	public void BecomeInvincible(){
		invincible = true;
		CameraShake.Shake (0.2f, 0.024f);
		StartCoroutine (spriteEffector.Flicker (invincibilityTime));
		Invoke ("ResetInvincibility", invincibilityTime);
	}

	public override void ReceiveDamage(int damage){
		
		if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
			if (invincible == false) {
				health -= damage;
				animator.SetTrigger("Damage");

				if (health <= 0) {
					animator.SetTrigger("Death");
				}

				Knockback ();
				BecomeInvincible ();
			}
	}

	void ResetInvincibility(){
		invincible = false;
	}

	private void Knockback(){
		var rbody = GetComponent<Rigidbody2D> ();
		rbody.velocity = new Vector2 (0, 0);
		player.WasDamaged ();
	}
		
	override public void DestroySelf(){
		levelManager.fader.BeginFade (1);
		levelManager.RespawnPlayer ();
	}

	override public void Respawn(){
		DestroySelf ();
	}
}
