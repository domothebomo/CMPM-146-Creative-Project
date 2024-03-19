using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{ 
    Character_BT Behavior_Tree;

    Vector3 move_destination = new Vector3(0.0f, 1.0f, 0.0f);
    NavMeshAgent agent;

    Transform holdPos;
    GameObject heldObject = null;

    GameObject waypoint;

    bool move_requested = false;

    [SerializeField]
    private int playerOpinion = 10; // default player opinion value
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        holdPos = transform.GetChild(0);

        /*waypoint = GameObject.Find("BasicObject");
        Move(waypoint.transform.position);*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Vector3.Distance(waypoint.transform.position, transform.position) <= 2 && heldObject == null)
        {
            PickUp(waypoint);
        }*/

        if (heldObject != null)
        {
            heldObject.transform.position = holdPos.position;
        }
    }

    public void RequestMove()
    {
        move_requested = true;
    }

    public bool IsMoveRequested()
    {
        if (move_requested)
        {
            move_requested = false;
            return true;
        }
        return false;
    }

    public void SetMoveDestination(Vector3 destination)
    {
        move_destination = destination;
    }

    public bool MoveToClicked()
    {
        Move(move_destination);
        return true;
    }

    void Move(Vector3 position)
    {
        Vector3 lookPos = position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.LookRotation(lookPos);
        agent.destination = position;
    }

    public bool IsHoldingObject()
    {
        return (heldObject != null);
    }

    public GameObject GetHeldObject()
    {
        return heldObject;
    }

    public void PickUp(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().useGravity = false;
        obj.transform.position = holdPos.position;
        obj.transform.parent = transform;
        obj.transform.rotation = transform.rotation;
        heldObject = obj;
    }

    public void Drop(GameObject obj)
    {
        obj.transform.parent = null;
        obj.GetComponent<Rigidbody>().useGravity = true;
        heldObject = null;
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
