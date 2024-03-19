using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{ 
    Character_BT Behavior_Tree;

    NavMeshAgent agent;

    Transform holdPos;
    GameObject heldObject = null;

    GameObject waypoint;

    Vector3 moveDestination = new Vector3(0.0f, 1.0f, 0.0f);
    GameObject objectToPickUp = null;
    GameObject objectToDrop = null;

    bool moveRequested = false;
    bool pickUpRequested = false;
    bool dropRequested = false;

    [SerializeField]
    private int playerOpinion = 10; // default player opinion value
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        holdPos = transform.GetChild(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (heldObject != null)
        {
            heldObject.transform.position = holdPos.position;
            heldObject.transform.rotation = holdPos.rotation;

            if (heldObject.CompareTag("Bucket") && IsFireNearby())
            {
                DouseFires();
            }
        }
    }

    private void FacePos(Vector3 position)
    {
        Vector3 lookPos = position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);
    }

    public bool IsHoldingObject()
    {
        return (heldObject != null);
    }

    public bool IsHoldingBucket()
    {
        return (heldObject != null && heldObject.CompareTag("Bucket"));
    }

    public GameObject GetHeldObject()
    {
        return heldObject;
    }

    private bool IsFireNearby()
    {
        if (GetComponent<Flammable>().IsOnFire())
        {
            return true;
        } 
        
        // Find all colliders within a 5-unit radius sphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Object") && hitCollider.gameObject.GetComponent<Flammable>().IsOnFire())
            {
                return true;
            }
        }
        return false;
    }

    private void DouseFires()
    {
        if (GetComponent<Flammable>().IsOnFire())
        {
            GetComponent<Flammable>().PutOutFire();
        }

        // Find all colliders within a 5-unit radius sphere
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Object") && hitCollider.gameObject.GetComponent<Flammable>().IsOnFire())
            {
                hitCollider.gameObject.GetComponent<Flammable>().PutOutFire();
            }
        }
    }

    public bool FindWaterBucket()
    {
        GameObject bucket = GameObject.FindWithTag("Bucket");
        if (bucket != null)
        {
            return true;
        }
        return false;
    }

    #region Move
    public void RequestMove()
    {
        moveRequested = true;
    }

    public bool IsMoveRequested()
    {
        if (moveRequested)
        {
            moveRequested = false;
            return true;
        }
        return false;
    }

    public void SetMoveDestination(Vector3 destination)
    {
        moveDestination = destination;
    }

    public bool MoveToClicked()
    {
        Move(moveDestination);
        return true;
    }

    void Move(Vector3 position)
    {
        FacePos(position);
        agent.destination = position;
    }
    #endregion

    #region Pick Up
    public void RequestPickUp()
    {
        pickUpRequested = true;
    }

    public bool IsPickUpRequested()
    {
        if (pickUpRequested)
        {
            return true;
        }
        return false;
    }

    public void SetObjectToPickUp(GameObject obj)
    {
        objectToPickUp = obj;
    }

    public bool PickUpObject()
    {
        PickUp(objectToPickUp);
        return true;
    }

    public bool ObjectInPickUpRange()
    {
        return Vector3.Distance(transform.position, objectToPickUp.transform.position) < 3.0;
    }

    public bool MoveToObjectToPickUp()
    {
        SetMoveDestination(objectToPickUp.transform.position);
        RequestMove();
        return true;
    }

    private void PickUp(GameObject obj)
    {
        FacePos(obj.transform.position);
        obj.GetComponent<Rigidbody>().useGravity = false;
        obj.transform.position = holdPos.position;
        obj.transform.parent = transform;
        obj.transform.rotation = transform.rotation;
        heldObject = obj;
        pickUpRequested = false;
    }
    #endregion

    #region Drop
    public void RequestDrop()
    {
        dropRequested = true;
    }

    public bool IsDropRequested()
    {
        if (dropRequested)
        {
            dropRequested = false;
            return true;
        }
        return false;
    }

    public void SetObjectToDrop(GameObject obj)
    {
        objectToDrop = obj;
    }

    public bool DropObject()
    {
        Drop(objectToDrop);
        return true;
    }

    private void Drop(GameObject obj)
    {
        obj.transform.parent = null;
        obj.GetComponent<Rigidbody>().useGravity = true;
        heldObject = null;
    }
    #endregion

    public void ClearRequests()
    {
        pickUpRequested = false;
        dropRequested = false;
        moveRequested = false;
    }

    public int GetPlayerOpinion()
    {
        return playerOpinion;
    }

    public void ChangePlayerOpinion(int opinionChange)
    {
        playerOpinion += opinionChange;
    }
}
