using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Script to launch a shooting star
 * 
 * @author "Sebastián José Díaz Rodríguez", "Ernesto Echeverría González", "Antonio Jesús López Garnier", "Germán Andrés Pescador Barreto"
 * @since October 2017
 * 
 * @version 1.0.0
 */
public class starlight : MonoBehaviour {
	private bool ida = true; // Indicates the orientation of the shooting star

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		if (ida) {
			transform.Translate (0, 0, 0.2f + Input.acceleration.z);
			if (transform.position.z > 350)
				ida = false;
			
		} else {
			transform.Translate (0, 0, -0.2f - Input.acceleration.z);
			if (transform.position.z < -105)
				ida = true;
		}
	}
}
