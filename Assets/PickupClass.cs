using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupClass : MonoBehaviour
{
    Character Character_Script;
    [SerializeField] private LayerMask PickupLayer;
    [SerializeField] private Camera PlayerCamera;
    [SerializeField] private float PickupRange; 
    private Rigidbody CurrentObjectRigidBody;
    private Collider CurrentObjectCollider;
    private RaycastHit infoFromHit;

    void Start(){
        Character_Script = gameObject.GetComponent<Character>();
    }

    void Update(){
        if(Input.GetMouseButtonDown(0)){
            GameObject item;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo)){
                if(hitInfo.transform.gameObject.tag == "Object"){
                    if(!Character_Script.IsHoldingObject()){
                        Character_Script.PickUp(hitInfo.transform.gameObject);
                    }
                    else if(Character_Script.IsHoldingObject() &&  hitInfo.transform.gameObject == Character_Script.GetHeldObject()){
                        item = Character_Script.GetHeldObject();
                        Character_Script.Drop(item);
                    }
                }
            }
        }
    }


}
