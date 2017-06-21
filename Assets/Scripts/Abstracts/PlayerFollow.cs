using System;
using UnityEngine;


public abstract class PlayerFollow : MonoBehaviour
{
	protected bool allowFollow = true;
	protected GameObject character;

    private void Awake()
    {
		character = GameObject.Find ("Cricket");
    }

	public void StopFollowing(){
		allowFollow = false;
	}
	public void StartFollowing(){
		allowFollow = true;
		moveToPlayer ();
	}

	abstract public void moveToPlayer ();
}

