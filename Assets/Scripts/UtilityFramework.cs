using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFramework : MonoBehaviour
{
	// Import scripts
	Character Character_Script;

	// Avoid objects on fire utility
	public void AvoidObjectsOnFire(Character character) {
		// Find all colliders within a 5-unit radius sphere
		Collider[] hitColliders = Physics.OverlapSphere(character.transform.position, 5f);
		foreach (var hitCollider in hitColliders) {
			// Check OnFire tag built-in UnityEngine
			if(hitCollider.CompareTag("OnFire")) {
				// Calculate the direction vector from fire object to character
				Vector3 directionToAvoid = character.transform.position - hitCollider.transform.position;
				// Calculate a safe distance for character to move away from fire object
				Vector3 newDestination = character.transform.position + directionToAvoid.normalized * 5f;
				character.move(newDestination);
				break;
			}
		}
	}

	// Ideas for Character ignoring orders from player when developed low opinion of player
	// Implement a way to get the character's current opinion in Character.cs
	// Set a threshold for when an order should be ignored based on the numerical value of character opinion
}
