using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour {

	public Transform hmdOrientation;
	private Vector3 tempRot;
	
	// Update is called once per frame
	void Update () {
		tempRot.x = -hmdOrientation.localEulerAngles.y;
		tempRot.y = hmdOrientation.localEulerAngles.z;
		tempRot.z = -hmdOrientation.localEulerAngles.x;

		transform.localEulerAngles = tempRot;
	}
}
