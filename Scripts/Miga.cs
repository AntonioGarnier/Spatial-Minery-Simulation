using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/**
 * Script to store and change the cost of a crumb.
 * 
 * @author "Sebastián José Díaz Rodríguez", "Ernesto Echeverría González", "Antonio Jesús López Garnier", "Germán Andrés Pescador Barreto"
 * @since October 2017
 * 
 * @version 1.0.0
 */
public class Miga : MonoBehaviour {

    public int coste = 5; // Cost of the crumb. Meassures its duration.

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// Sustracts a unit to the cost of the crumb
    public void decrementa ()
    {
        coste -= 1;
    }

	// Adds a unit to the cost of the crumb
    public void incrementa ()
    {
        coste += 1;
    }
}
