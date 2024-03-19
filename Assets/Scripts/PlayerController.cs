using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    Character Character_Script;
    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float PickupRange; 
    private Rigidbody CurrentObjectRigidBody;
    private Collider CurrentObjectCollider;
    private RaycastHit infoFromHit;
    
    // Declare variables that we will/might need later
    CustomActions input;
    UnityEngine.AI.NavMeshAgent agent;
    // Animator animator; // uncomment if we want to add animations later

    [Header("Movement")]
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

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
        //RaycastHit hit;
        // if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){    // if the click is detected
        //     Character_Script.SetMoveDestination(hit.point);                                 // set the destination to the spot we clicked 
        //     Character_Script.RequestMove(); // request the bot to move
        //     if (clickEffect != null){
        //         Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
        //     }
        // }
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo)){
            if(hitInfo.transform.gameObject.tag == "Object" || hitInfo.transform.gameObject.tag == "Bucket")
            {
                    if(!Character_Script.IsHoldingObject()){
                        Debug.Log("pick up!");
                        Character_Script.SetObjectToPickUp(hitInfo.transform.gameObject);
                        Character_Script.RequestPickUp();
                    }
                    else if(Character_Script.IsHoldingObject() && hitInfo.transform.gameObject == Character_Script.GetHeldObject()){
                        Debug.Log("drop!");
                        GameObject item = Character_Script.GetHeldObject();
                        Character_Script.SetObjectToDrop(item);
                        Character_Script.RequestDrop();
                    }
            }
            else{
                Character_Script.SetMoveDestination(hitInfo.point);                                 // set the destination to the spot we clicked 
                Character_Script.RequestMove(); // request the bot to move
                if (clickEffect != null){
                    Instantiate(clickEffect, hitInfo.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
                }
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
