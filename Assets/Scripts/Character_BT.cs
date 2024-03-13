using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// From https://stackoverflow.com/questions/273313/randomize-a-listt
private static Random rng = new Random();  
public static void Shuffle<T>(this IList<T> list)  
{  
    int n = list.Count;  
    while (n > 1) {  
        n--;  
        int k = rng.Next(n + 1);  
        T value = list[k];  
        list[k] = list[n];  
        list[n] = value;  
    }  
}

public class Character_BT : MonoBehaviour
{
    private const int SLEEP_MS = 100;

    private Character Character_Script;

    // ------Behavior Tree Definitions-------

    // Basic behavior tree node abstraction
    private abstract class BT_Node {

        public abstract bool run();

    }

    // -------------Composites---------------

    // Abstract composite node, has children and iterates through them in various ways.
    private abstract class BT_Composite : BT_Node {

        private List<BT_Node> children;

        public void addChild(BT_Node n) {
            children.Add(n);
        }

    }

    // Sequence, iterates through children.
    // If a child returns false, it will return false. If it reaches the end, it will return true.
    // Analagous to an AND gate.
    private class BT_Sequence : BT_Composite {
        public override bool run() {
            foreach (BT_Node child in children) {
                if (not child.run()) {
                    return false;
                }
            }
            return true;
        }
    }

    // Selector, iterates through children.
    // If a child returns true, it will return true. If it reaches the end, it will return false.
    // Analagous to an OR gate.
    private class BT_Selector : BT_Composite {
        public override bool run() {
            foreach (BT_Node child in children) {
                if (not child.run()) {
                    return false;
                }
            }
            return true;
        }
    }

    // Random Selector
    // A selector that operates on a randomized child list
    private class BT_Rand_Selector : BT_Selector {
        public override bool run() {
            children.Shuffle();
            return base.run();
        }
    }

    // Random Sequence
    // A sequence that operates on a randomized child list
    private class BT_Rand_Sequence : BT_Selector {
        public override bool run() {
            children.Shuffle();
            return base.run();
        }
    }

    // -----------End Composites-------------

    // -------------Decorators---------------

    // Abstract decorator node, has exactly one child and performs an operation on its output.
    // May also repeat the child node a certain number of times.
    private abstract class BT_Decorator : BT_Node {
        BT_Node child;
        public void setChild(BT_Node n) {
            child = n
        }
    }

    // Inverter, returns the opposite of its child's result
    private class BT_Inverter : BT_Decorator {
        public override bool run() {
            return not child.run();
        }
    }

    // Succeeder, always returns true after running its child
    private class BT_Succeeder : BT_Decorator {
        public override bool run() {
            child.run();
            return true;
        }
    }

    // Repeater, infinitely calls its child
    private class BT_Succeeder : BT_Decorator {
        public override bool run() {
            while (true) {
                child.run();
                Thread.sleep(SLEEP_MS)
            }
            return false; //idk
        }
    }

    // Repeat until fail, calls its child until it returns false. Returns true upon completion.
    private class BT_Until_Fail : BT_Decorator {
        public override bool run() {
            while (child.run()) {
                Thread.sleep(SLEEP_MS)
            }
            return true;
        }
    }

    // -------------End Decorators--------------

    // Behavior tree leaf node, calls a bool function when it is run
    private class BT_Leaf : BT_Node {

        private Func<bool> action;

        public BT_Leaf(Func<bool> f) {
            action = f;
        }

        public override bool run() {
            return action();
        }
    }

    // --------End Behavior Tree Definitions--------

    // --------------Main Execution-----------------

    // Start is called before the first frame update
    void Start()
    {

        Character_Script = gameObject.GetComponent<Character>();

        // Create the behavior tree

        // Main sequence loop
        BT_Root root = new BT_Repeater();
        BT_Selector mainSelector = new BT_Rand_Selector();
        root.setChild(mainSelector);

        BT_Sequence playerClickSeq = new BT_Sequence();
        mainSelector.addChild(playerClickSeq);

        BT_Leaf plrMoveRequestCheck = new BT_Leaf(Character_Script.IsMoveRequested);
        BT_Leaf moveToClick = new BT_Leaf(Character_Script.MoveToClicked);
        playerClickSeq.addChild(plrMoveRequestCheck);
        playerClickSeq.addChild(moveToClick);

        root.run();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}