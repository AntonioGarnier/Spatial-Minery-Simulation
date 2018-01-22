using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Script that stores the ammount of resources left in an ore.
 * 
 * @author "Sebastián José Díaz Rodríguez", "Ernesto Echeverría González", "Antonio Jesús López Garnier", "Germán Andrés Pescador Barreto"
 * @since October 2017
 * 
 * @version 1.0.0
 */
public class Resource : MonoBehaviour {
    public bool isVisitado = false; // The ore has been visited
    private float cantidad = 100f; // Ammount of resource stored by the ore

    // Use this for initialization
    void Start () {
        isVisitado = false;
        cantidad = 100f;
    }
	
	// Update is called once per frame
	void Update () {
        if (cantidad < 10)
            Destroy(this.gameObject);
	}

	// Sustracts resources from the ore
    public void decrementa ()
    {
        cantidad -= 10;
    }

}
