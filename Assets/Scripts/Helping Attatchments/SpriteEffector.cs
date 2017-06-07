using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffector : MonoBehaviour {

	private bool corutineRunning = false;
	protected SpriteRenderer spriteRenderer;

	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	//Flicker (sprite visible and invisible)
	public IEnumerator Flicker(float duration){
		if (!corutineRunning) {
			corutineRunning = true;

			InvokeRepeating ("ToggleRenderer", 0.1f, 0.1f);
			yield return new WaitForSeconds (duration);
			stopFlicker ();
		}
	}

	public void stopFlicker(){
		CancelInvoke ("ToggleRenderer");
		corutineRunning = false;
		spriteRenderer.enabled = true;
	}

	private void ToggleRenderer(){
		spriteRenderer.enabled = !spriteRenderer.enabled;
	}


	//ColorChanging
	public void FlashRedOnce(){
		spriteRenderer.material.color = Color.red;
		Invoke ("RestoreColor", 0.1f);
	}
	public void RestoreColor(){
		spriteRenderer.material.color = Color.white;
	}
}
