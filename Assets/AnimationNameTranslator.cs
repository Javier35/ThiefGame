using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationNameTranslator : MonoBehaviour {

	Dictionary<int, string> animationHash = new Dictionary<int, string>();

	void Start(){

		animationHash.Add(Animator.StringToHash ("Idle") , "Idle");
		animationHash.Add(Animator.StringToHash ("Run") , "Run");
		animationHash.Add(Animator.StringToHash ("Dash") , "Dash");
		animationHash.Add(Animator.StringToHash ("Jump") , "Jump");
		animationHash.Add(Animator.StringToHash ("Fall") , "Fall");
		animationHash.Add(Animator.StringToHash ("Death") , "Death");
	}

	public string getNameByHash(int hash){

		if (animationHash.ContainsKey(hash))
		{
			return animationHash[hash];
		}

		return "Idle";
	}
}
