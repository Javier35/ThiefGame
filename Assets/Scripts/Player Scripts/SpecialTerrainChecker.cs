using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialTerrainChecker : MonoBehaviour {

	public SpecialTerrain specialTerrain;

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Platform"){
			
			ContactPoint2D contact = collision.contacts[0];
			if(Vector3.Dot(contact.normal, Vector3.up) > 0.5){

				//collision was from below
				if(collision.gameObject.GetComponent<SpecialTerrain>() != null ){
					specialTerrain = collision.gameObject.GetComponent<SpecialTerrain> ();
				}
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Platform"){

			ContactPoint2D contact = collision.contacts[0];
			if(Vector3.Dot(contact.normal, Vector3.up) > 0.5){

				//collision was from below
				if(collision.gameObject.GetComponent<SpecialTerrain>() != null ){
					specialTerrain = null;
				}
			}
		}
	}

}
