using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Script to move the player and make him bleed when he crush into something
 * 
 * @author "Sebastián José Díaz Rodríguez", "Ernesto Echeverría González", "Antonio Jesús López Garnier", "Germán Andrés Pescador Barreto"
 * @since October 2017
 * 
 * @version 1.0.0
 */
public class PlayerMovement : MonoBehaviour {
	public float speed = 5;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		float translationRate = Time.deltaTime * speed;
		float zTranslation = Input.GetAxis ("Vertical") * translationRate;
		transform.Translate (0, 0, zTranslation);
		transform.Rotate(0, Input.GetAxis("Horizontal")*speed * 15 *Time.deltaTime, 0);
	}
	// Event listener of a collision
	public void OnCollisionEnter(Collision col) {
		GameObject.FindGameObjectWithTag ("blood").GetComponent<ParticleSystem> ().Play (); // The player's hand bleeds
	}
}
