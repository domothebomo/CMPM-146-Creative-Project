using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{ 
    Character_BT Behavior_Tree;

    Vector3 move_destination = new Vector3(0.0f, 1.0f, 0.0f);
    NavMeshAgent agent;

    GameObject waypoint;

    bool move_requested = false;
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {

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

    public void SetMoveRequest(bool request)
    {
        move_requested = request;
    }

    public void SetMoveDestination(Vector3 destination)
    {
        move_destination = destination;
    }

    public void MoveToClicked()
    {
        Move(move_destination);
    }

    void Move(Vector3 position)
    {
        agent.destination = position;
    }
}
