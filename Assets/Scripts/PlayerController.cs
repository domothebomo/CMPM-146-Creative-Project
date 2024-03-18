using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    // Import Character Scripts
    Character Character_Script;
    
    // Declare variables that we will/might need later
    CustomActions input;
    UnityEngine.AI.NavMeshAgent agent;
    // Animator animator; // uncomment if we want to add animations later

    [Header("Movement")]
    ParticleSystem clickEffect;
    LayerMask clickableLayers;

    //float lookRotationSpeed = 8f;

    void Start(){
        Character_Script = gameObject.GetComponent<Character>();
    }
    void Awake(){
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); 
        // animator = GetComponent<Animator>(); // uncomment if we want to add animations later

        input = new CustomActions();
        AssignInputs();
    }

    void AssignInputs(){
        input.Main.Move.performed += ctx => ClickToMove();
    }

    void ClickToMove(){
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){    // if the click is detected
            Character_Script.SetMoveDestination(hit.point);                                 // set the destination to the spot we clicked 
            Character_Script.RequestMove();                                                 // request the bot to move
            if(clickEffect != null){
                Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
            }
        }
    }

    void OnEnable(){
        if (input == null){
            input = new CustomActions();
        }
        input.Enable();
    }
    void OnDisable(){
        if(input != null){
            input.Disable();
        }
    }
}
