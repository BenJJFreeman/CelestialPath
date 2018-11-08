using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement {

	public static bool UpdateShipMovement (float _power,Transform _ship) {
		
		Vector3 mousePos = Input.mousePosition;
		Vector3 screenPos = Camera.main.WorldToScreenPoint(_ship.position);
		Vector3 offset = new Vector3(mousePos.x - screenPos.x, mousePos.y - screenPos.y);

		float angle = Mathf.Atan2(offset.x, offset.y) * Mathf.Rad2Deg;
		_ship.rotation = Quaternion.AngleAxis(angle, Vector3.up); 


		_ship.position += _ship.forward * _power *Time.deltaTime;


		return false;
	}

}
