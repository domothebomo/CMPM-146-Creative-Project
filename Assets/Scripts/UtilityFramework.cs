using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityFramework : MonoBehaviour
{
	// Import scripts
	Character Character_Script;



    private void Start()
    {
        Character_Script = GetComponent<Character>();
    }

    // Avoid objects on fire utility
    public bool AvoidObjectsOnFire()
	{
		// Find all colliders within a 5-unit radius sphere
		Collider[] hitColliders = Physics.OverlapSphere(Character_Script.transform.position, 5f);
		foreach (var hitCollider in hitColliders) {
			// Check OnFire tag built-in UnityEngine
			if(hitCollider.CompareTag("Object") && hitCollider.gameObject.GetComponent<Flammable>().IsOnFire()) {
				// Calculate the direction vector from fire object to character
				Vector3 directionToAvoid = Character_Script.transform.position - hitCollider.transform.position;
				// Calculate a safe distance for character to move away from fire object
				Vector3 newDestination = Character_Script.transform.position + directionToAvoid.normalized * 5f;
				Character_Script.ClearRequests();
				Character_Script.SetMoveDestination(newDestination);
				Character_Script.MoveToClicked();
				break;
			}
		}
		return true;
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
