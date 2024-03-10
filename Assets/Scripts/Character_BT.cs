using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_BT : MonoBehaviour
{

    Character Character_Script;
    
    // Start is called before the first frame update
    void Start()
    {

        Character_Script = gameObject.GetComponent<Character>();

        //Character_Script.MoveToClicked();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
