using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public float dragSpeed = 4;
	private Vector3 dragOrigin;
	public bool galaxyCamera;
	public Transform target;

	public void UpdateCamera (bool _follow) {

		float horizontal = Input.GetAxis ("Horizontal");
		float vertical = Input.GetAxis ("Vertical");
		float scroll = Input.GetAxis ("Mouse ScrollWheel") * 100;

		transform.position += new Vector3 (horizontal, -scroll, vertical) * 100 * Time.deltaTime;
		if (transform.position.y < 40) {
			transform.position = new Vector3 (transform.position.x, 40, transform.position.z);
		} else if (transform.position.y > 250) {
			transform.position = new Vector3 (transform.position.x, 250, transform.position.z);
		}

		if (galaxyCamera) {
			UpdateGalaxyCamera ();
		} else {
			UpdateSystemCamera (_follow);
		}


	}
	public void UpdateGalaxyCamera (){

		if (Input.GetMouseButtonDown(0))
		{
			dragOrigin = Input.mousePosition;
			return;
		}

		if (!Input.GetMouseButton(0)) return;

		Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
		Vector3 move = new Vector3(pos.x * dragSpeed, 0, pos.y * dragSpeed);


		transform.position -= new Vector3 (move.x, 0, move.z) * 100 * Time.deltaTime;

	}
	public void UpdateSystemCamera (bool _follow){

		if (_follow) {

			transform.position = new Vector3 (target.position.x, transform.position.y, target.position.z - 50);
			transform.LookAt (target.position);
		} else {
			transform.rotation = Quaternion.Euler (45,0,0);
		}


	}
}
