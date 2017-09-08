using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : MonoBehaviour {

	public Transform projectileSource;
	public GameObject projectile;
	[SerializeField] private int projectilePoolSize = 7;
	List <GameObject> objectPool = new List<GameObject>();
	private Vector3 spawnPosition;

	void Start(){
		
		for (int i = 0; i < projectilePoolSize; i++) {

			var projectilePool = GameObject.Find ("PooledObjects");

			objectPool.Add (Instantiate (projectile, projectilePool.transform));
			objectPool [i].SetActive(false);
		}
	}

	GameObject getPooledObject(){
		for (int i = 0; i < objectPool.Count; i++) {
			if (!objectPool [i].activeInHierarchy) {
				return objectPool [i];
			}		
		}
		return null;
	}

	public void SpawnProjectile(){

		var newProjectile = getPooledObject ();

		if (newProjectile != null) {


			newProjectile.transform.position = new Vector3 (projectileSource.position.x, projectileSource.position.y, projectileSource.position.z + 1);
			newProjectile.SetActive(true);

			var newProjectileBehavior = newProjectile.GetComponent<ProjectileBehavior> ();

			if (GetComponentInParent<PlatformerCharacter2D> ().facingRight) {
				newProjectileBehavior.setDirection (1);
			} else {
				newProjectileBehavior.setDirection (-1);
			}
		}
	}
}
