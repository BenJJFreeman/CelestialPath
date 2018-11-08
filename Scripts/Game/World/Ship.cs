using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {

	public Main main;

	public ShipType shipType;
	public Commander commander;

	public int maxStrength;
	public int strength;

	public int money;

	public int maxJumpCharges;
	public int jumpCharges;

	public int research;

	public float enginePower;

	public float maxJumpDistance;

	bool moving;

	public void SetUpShip (ShipType _shipType,Commander _commander){
		SetUpShipType (_shipType);
		SetUpCommander (_commander);


		strength = maxStrength;
		jumpCharges = maxJumpCharges;

	}
	public void SetUpShip (Vector3 _position,ShipType _shipType,Commander _commander,int _strength,int _money,int _jumpCharges,int _research){
		transform.position = _position;
		SetUpShipType (_shipType);
		SetUpCommander (_commander);


		strength = _strength;
		money = _money;
		jumpCharges = _jumpCharges;
		research = _research;
	}
	public void SetUpShipType (ShipType _shipType){
		shipType = _shipType;

		switch (shipType) {
		case ShipType.a:
			maxStrength = 25;
			maxJumpCharges = 2;
			maxJumpDistance = 125;
			break;
		case ShipType.b:
			maxStrength = 15;
			maxJumpCharges = 4;
			maxJumpDistance = 200;
			break;
		case ShipType.c:
			maxStrength = 20;
			maxJumpCharges = 3;
			maxJumpDistance = 150;
			break;
		}

	}
	public void SetUpCommander (Commander _commander){
		commander = _commander;

		switch (commander) {
		case Commander.a:
			money = 800;
			research = 200;
			maxStrength += 3;
			maxJumpCharges += 1;
			break;
		case Commander.b:
			money = 500;
			research = 500;
			maxStrength += 2;
			maxJumpCharges += 2;
			break;
		case Commander.c:
			money = 200;
			research = 800;
			maxStrength += 1;
			maxJumpCharges += 3;
			break;
		}
	}
	public bool UpdateShip (bool _holdToMove){


		UpdateMovement (_holdToMove);


	
		if (strength <= 0) {
			return false;
		}

		return true;
	}
	void UpdateMovement (bool _holdToMove){
		Collider[] overlaps;
		int o;

		if (_holdToMove) {
			if (Input.GetMouseButton (0)) {
				moving = true;
			} else {
				moving = false;
			}
		} else {
			if (Input.GetMouseButtonDown (0)) {
				moving = !moving;
			}
		}

		if (moving) {
			if (enginePower < 25) {
				enginePower += 25 * Time.deltaTime;
			} else {
				enginePower = 25;
			}
		} else {
			if (enginePower > 0) {
				enginePower -= 25 * Time.deltaTime;
			} else {
				enginePower = 0;
			}
		}


		ShipMovement.UpdateShipMovement (enginePower, transform);
			

	}


}
public enum ShipType {a,b,c}
public enum Commander {a,b,c}
