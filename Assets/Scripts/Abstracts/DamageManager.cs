using UnityEngine;
using System.Collections;

public abstract class DamageManager : Destroyable {

	public int health = 3;
	public int maxHealth = 5;
	public int damage = 1;
	public bool invincible = false;

	public Animator animator;

	void Awake(){
		animator = gameObject.GetComponent<Animator> ();
		spriteEffector = GetComponent<SpriteEffector> ();
	}

	public abstract void ReceiveDamage (int damage);
	
	public void StartDeathAnim(){
		animator.SetTrigger("Death");
	}

	public void Heal(int healing){
		health += healing;
		if (health > maxHealth)
			health = maxHealth;
	}
}
