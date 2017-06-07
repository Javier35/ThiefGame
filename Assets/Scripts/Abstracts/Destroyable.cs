using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Destroyable : MonoBehaviour {

	protected SpriteEffector spriteEffector;
	protected bool spawned = false;

	abstract public void DestroySelf ();
	abstract public void Respawn ();

	public void setSpawned(bool isSpawned){
		spawned = isSpawned;
	}
}
