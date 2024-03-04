using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{ 
    Character_BT Behavior_Tree;

    Vector3 move_destination;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
       // move to position
       Debug.Log("moving to " + position);
    }
}
