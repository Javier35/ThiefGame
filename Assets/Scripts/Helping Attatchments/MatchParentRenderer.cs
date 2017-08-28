using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchParentRenderer : MonoBehaviour {

	public SpriteRenderer parentRenderer;
	SpriteRenderer renderer;

	void Start () {
		renderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		renderer.enabled = parentRenderer.enabled;
	}
}