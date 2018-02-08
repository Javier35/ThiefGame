using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchParentRenderer : MonoBehaviour {

	public SpriteRenderer parentRenderer;
	SpriteRenderer spriteRenderer;

	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		spriteRenderer.enabled = parentRenderer.enabled;
	}
}