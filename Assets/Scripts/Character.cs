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
    
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // --- Test code - Set up an object in scene called Waypoint, and character will follow the x and z coords of that object
        //waypoint = GameObject.Find("Waypoint");
        //SetMoveDestination(new Vector3(waypoint.transform.position.x, 1.0f, waypoint.transform.position.z));
        //MoveToClicked();
        // ---
    }

    // Update is called once per frame
    void Update()
    {
        // --- Test code
        //SetMoveDestination(new Vector3(waypoint.transform.position.x, 1.0f, waypoint.transform.position.z));
        //MoveToClicked();
        // ---
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
