using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/**
 * Script of the GameController
 * 
 * @author "Sebastián José Díaz Rodríguez", "Ernesto Echeverría González", "Antonio Jesús López Garnier", "Germán Andrés Pescador Barreto"
 * @since October 2017
 * 
 * @version 1.0.0
 */
public delegate void DroneCollisionDelegate();

public class Controller : MonoBehaviour
{
	private int crumpsSoundCD = 0; //  Cooldown for the crumbs sound
	private int botSoundCD = 0; // Cooldown for the collectors sound
	private int ccCD = 0; // Cooldown for the change of camera
	public GameObject player; // Player. Used to get the distance to the crumbs or the collectors to play the sounds
	public GameObject fullRock; // Ore prefab

	public Camera playerCamera; // Main camera, used by the player
	public Camera droidCamera; // Second camera, used by the drone

	public int rockCount; // Number of ores to be spawned
    public float range = 200f;

    void Start()
    {
		playerCamera.enabled = true;
		droidCamera.enabled = false;
		spawnRocks();
    }

    void Update()
	{
		// Change of camera
		float changeCamera = Input.GetAxis ("Fire1");
		if(changeCamera != 0 && ccCD <= 0) {
			playerCamera.enabled = !playerCamera.enabled;
			droidCamera.enabled = !droidCamera.enabled;
			ccCD = 5;
		}
		reduceCD ();
		// Execution of the Crumb sound
		GameObject[] crumps = GameObject.FindGameObjectsWithTag ("Crumb");
		foreach(GameObject i in crumps) {
			float crumps2ethan = Vector3.Distance(i.transform.position, player.transform.position);
			if(crumps2ethan > 10 && crumpsSoundCD <= 0) {
				crumpsSoundCD = 5400;
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource> ().Play ();
			}
		}
		// Execution of the Collector sound
		GameObject[] bots = GameObject.FindGameObjectsWithTag ("Collector");
		foreach(GameObject i in bots) {
			float bot2ethan = Vector3.Distance(i.transform.position, player.transform.position);
			if(bot2ethan > 5 && botSoundCD <= 0) {
				botSoundCD = 5400;
				i.GetComponent<AudioSource> ().Play ();
			}
		}

	}

	// Reduces the cooldown of the sounds and camera change
	public void reduceCD() {
		if (crumpsSoundCD > 0)
			crumpsSoundCD--;
		if (botSoundCD > 0)
			botSoundCD--;
		if (ccCD > 0)
			ccCD--;
	}

	// Spawn of ores
	public void spawnRocks()
	{
		for(int i = 0; i < rockCount; i++)
		{
            Vector3 point;
            if (RandomPoint(transform.position, range, out point))
            {
                Instantiate(fullRock, point, Quaternion.identity);
			} else{
				i--;	
			}
        }
	}

	// Gets a random position and checks if it's already occupied
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 0.2f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
		return false;
    }

}