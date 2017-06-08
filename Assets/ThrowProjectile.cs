using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : MonoBehaviour {

	public Transform projectileSource;
	public GameObject projectile;

	public void InstantiateProjectile(){
		var newProjectile = Instantiate (projectile, projectileSource.transform.position, projectile.transform.rotation);
		var newProjectileBehavior = newProjectile.GetComponent<ProjectileBehavior> ();
		newProjectileBehavior.facingRight = GetComponentInParent<PlatformerCharacter2D> ().m_FacingRight;
	}
}
