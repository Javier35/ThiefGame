using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : MonoBehaviour {

	public Transform projectileSource;
	public GameObject projectile;
	[SerializeField] private int projectilePoolSize = 7;
	List <GameObject> objectPool = new List<GameObject>();
	// private Vector3 spawnPosition;

	PlatformerCharacter2D player;
	FollowerBehavior follower;

	void Start(){

		player = GetComponentInParent<PlatformerCharacter2D> ();
		follower = GetComponent<FollowerBehavior>();
		
		for (int i = 0; i < projectilePoolSize; i++) {

			var projectilePool = GameObject.Find ("PooledObjects");

			objectPool.Add (Instantiate (projectile, projectilePool.transform));
			objectPool [i].SetActive(false);
		}
	}

	GameObject getPooledObject(){
		for (int i = 0; i < objectPool.Count; i++) {
			if (!objectPool [i].activeInHierarchy && objectPool [i].name.Contains(projectile.name)) {
				
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
			bool facingRight = player == null ? follower.facingRight : player.facingRight;

			if (facingRight) {
				newProjectileBehavior.setDirection (1);
			} else {
				newProjectileBehavior.setDirection (-1);
			}
		}
	}
}
