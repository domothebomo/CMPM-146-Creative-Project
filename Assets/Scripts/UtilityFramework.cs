using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFramework : MonoBehaviour
{
	// Import scripts
	Character Character_Script;

	// Avoid objects on fire utility
	public void AvoidObjectsOnFire(Character character)
	{
		// Find all colliders within a 5-unit radius sphere
		Collider[] hitColliders = Physics.OverlapSphere(character.transform.position, 5f);
		foreach (var hitCollider in hitColliders) {
			// Check OnFire tag built-in UnityEngine
			if(hitCollider.CompareTag("OnFire")) {
				// Calculate the direction vector from fire object to character
				Vector3 directionToAvoid = character.transform.position - hitCollider.transform.position;
				// Calculate a safe distance for character to move away from fire object
				Vector3 newDestination = character.transform.position + directionToAvoid.normalized * 5f;
				character.SetMoveDestination(newDestination);
				character.MoveToClicked();
				break;
			}
		}
	}

	// Determine if Character should Ignore Order or not
	public bool ShouldIgnoreOrder(Character character, int playerOpinionThreshold)
	{
		// Retrieves character's current opinion
		int currentOpinion = character.GetPlayerOpinion();
		// Logic to determine whether or not to ignore order based on threshold
		return currentOpinion < playerOpinionThreshold;
	}
}
