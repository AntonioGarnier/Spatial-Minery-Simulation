using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * Script to show the ammount of resources collected in the spaceship
 * 
 * @author "Sebastián José Díaz Rodríguez", "Ernesto Echeverría González", "Antonio Jesús López Garnier", "Germán Andrés Pescador Barreto"
 * @since October 2017
 * 
 * @version 1.0.0
 */
public class Cantidad : MonoBehaviour {
    public Text text; // Text to be showed above the spaceship
	public AudioSource audio; // Clip of audio that is played each time a resource arrives at the ship

	private float cantidad = 0; // Ammount of resources collected
    
    // Use this for initialization
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {
        text.text = "" + cantidad;
    }

	// Reproduces the audio clip
    public void play ()
    {
        audio.Play();
    }

	// Adds a resource ammount
    public void incrementa (float cant)
    {
        cantidad += cant;
    }
}
