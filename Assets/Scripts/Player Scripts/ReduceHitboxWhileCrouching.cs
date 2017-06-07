using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceHitboxWhileCrouching : MonoBehaviour {

	private float reductionOffset = 0.45f;

	public GameObject physicalHitboxesObject;
	public GameObject triggetHitboxesObject;

	private BoxCollider2D physicalBox;
	private BoxCollider2D triggerBox;


	private Vector2 originaPhysicalHitboxSize;
	private Vector2 originalPhysicalHitboxOffset;

	private Vector2 originalTriggerHitboxSize;
	private Vector2 originalTriggerHitboxOffset;

	private Animator anim;

	void Start(){
		physicalBox = physicalHitboxesObject.GetComponent<BoxCollider2D> ();
		triggerBox = triggetHitboxesObject.GetComponent<BoxCollider2D> ();

		originaPhysicalHitboxSize = physicalBox.size;
		originalPhysicalHitboxOffset = physicalBox.offset;

		originalTriggerHitboxSize = triggerBox.size;
		originalTriggerHitboxOffset = triggerBox.offset;

		anim = GameObject.Find ("Moxi").GetComponent<Animator> ();
	}

	void Update () {
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Crouch") ||
		    anim.GetCurrentAnimatorStateInfo (0).IsName ("CrouchLoop")) {

			ReduceHitboxes ();

		} else {
			if(physicalBox.size.y != originaPhysicalHitboxSize.y)
				RestoreHitboxes ();
		}
	}

	private void ReduceHitboxes(){
		 
		physicalBox.size = new Vector2 (physicalBox.size.x, reductionOffset);
		var heightDifference = Mathf.Abs (originaPhysicalHitboxSize.y - physicalBox.size.y);
		physicalBox.offset = new Vector2 (originalPhysicalHitboxOffset.x, originalPhysicalHitboxOffset.y - heightDifference/2);
	
		triggerBox.size = new Vector2 (triggerBox.size.x, reductionOffset);
		heightDifference = Mathf.Abs (originalTriggerHitboxSize.y - triggerBox.size.y);
		triggerBox.offset = new Vector2 (originalTriggerHitboxOffset.x, originalTriggerHitboxOffset.y - heightDifference/2);
	}

	private void RestoreHitboxes(){
		physicalBox.size = originaPhysicalHitboxSize;
		physicalBox.offset = originalPhysicalHitboxOffset;

		triggerBox.size = originalTriggerHitboxSize;
		triggerBox.offset = originalTriggerHitboxOffset;
	}
}
