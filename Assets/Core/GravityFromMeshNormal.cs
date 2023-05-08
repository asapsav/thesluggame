using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFromMeshNormal : MonoBehaviour
{

	public Transform tGameCharacter;

	void Update(){
		// raycast downwards to find the normal
		 
		// implement gravity 
	}
    
	public void ImplementGravityFromNormal(Transform t,Vector3 n){
		t.up = n;
	}
	
}
