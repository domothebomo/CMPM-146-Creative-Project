using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Threading;
using UnityEngine;

// From https://stackoverflow.com/questions/273313/randomize-a-listt 
public static class ListExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n); // Corrected range
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class Character_BT : MonoBehaviour
{
    private const int SLEEP_MS = 100;

    private Character Character_Script;
    private UtilityFramework Utility_Script;

    // ------Behavior Tree Definitions-------

    // Basic behavior tree node abstraction
    protected abstract class BT_Node {

        public abstract bool run();

    }

    // -------------Composites---------------

    // Abstract composite node, has children and iterates through them in various ways.
    protected abstract class BT_Composite : BT_Node {

        protected List<BT_Node> children = new List<BT_Node>();

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
                if (!child.run()) {
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
                if (child.run()) {
                    return true;
                }
            }
            return false;
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
        protected BT_Node child;
        public void setChild(BT_Node n) {
            child = n;
        }
    }

    // Inverter, returns the opposite of its child's result
    private class BT_Inverter : BT_Decorator {
        public override bool run() {
            return !child.run();
        }
    }

    // Succeeder, always returns true after running its child
    private class BT_Succeeder : BT_Decorator {
        public override bool run() {
            child.run();
            return true;
        }
    }

    // Failer, always returns false after running its child
    private class BT_Failer : BT_Decorator
    {
        public override bool run()
        {
            child.run();
            return false;
        }
    }

    // Root, calls its child
    private class BT_Root : BT_Decorator {
        public override bool run() {
            child.run();
            return true;
        }
    }

    // Repeat until fail, calls its child until it returns false. Returns true upon completion.
    private class BT_Until_Fail : BT_Decorator {
        public override bool run() {
            while (child.run()) {
                Thread.Sleep(SLEEP_MS);
            }
            return true;
        }
    }

    // -------------End Decorators--------------

    // Behavior tree leaf node, calls a bool function when it is run
    private class BT_Leaf : BT_Node {

        private System.Func<bool> action;

        public BT_Leaf(System.Func<bool> f) {
            action = f;
        }

        public override bool run() {
            return action();
        }
    }

    // --------End Behavior Tree Definitions--------

    // --------------Main Execution-----------------

    BT_Root root;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

        Character_Script = gameObject.GetComponent<Character>();
        Utility_Script = gameObject.GetComponent<UtilityFramework>();

        // Create the behavior tree

        // Main sequence loop
        root = new BT_Root();
        BT_Selector mainSelector = new BT_Selector();
        root.setChild(mainSelector);

        // Self-preservation sequence
        BT_Sequence fireSequence = new BT_Sequence();
        mainSelector.addChild(fireSequence);

        BT_Leaf fireCheck = new BT_Leaf(Character_Script.CharacterOnFire);
        BT_Sequence putSelfOutSequence = new BT_Sequence();

        BT_Leaf findBucket = new BT_Leaf(FindWaterBucket);

        putSelfOutSequence.addChild(findBucket);
        putSelfOutSequence.addChild(executePickup);

        fireSequence.addChild(fireCheck);
        fireSequence.addChild(putSelfOutSequence);

        // Being on fire nullifies any other behavior
        mainSelector.addChild(fireCheck);

        // Move sequence
        BT_Sequence playerClickSeq = new BT_Sequence();
        mainSelector.addChild(playerClickSeq);

        BT_Leaf plrMoveRequestCheck = new BT_Leaf(Character_Script.IsMoveRequested);
        BT_Leaf moveToClick = new BT_Leaf(Character_Script.MoveToClicked);
        playerClickSeq.addChild(plrMoveRequestCheck);
        playerClickSeq.addChild(moveToClick);

        // Drop sequence
        BT_Sequence playerDropSeq = new BT_Sequence();
        mainSelector.addChild(playerDropSeq);

        BT_Leaf plrDropCheck = new BT_Leaf(Character_Script.IsDropRequested);
        BT_Leaf isHoldingObj = new BT_Leaf(Character_Script.IsHoldingObject);
        BT_Leaf dropObj = new BT_Leaf(Character_Script.DropObject);
        playerDropSeq.addChild(plrDropCheck);
        playerDropSeq.addChild(isHoldingObj);
        playerDropSeq.addChild(dropObj);

        // Pickup sequence
        BT_Sequence playerPickupSeq = new BT_Sequence();
        mainSelector.addChild(playerPickupSeq);

        BT_Leaf plrPickupCheck = new BT_Leaf(Character_Script.IsPickUpRequested);

        BT_Selector executePickup = new BT_Selector();
        
        BT_Sequence alreadyNearItem = new BT_Sequence();
        BT_Leaf objectAlreadyInRange = new BT_Leaf(Character_Script.ObjectInPickUpRange);
        BT_Leaf pickupCloseObj = new BT_Leaf(Character_Script.PickUpObject);
        alreadyNearItem.addChild(objectAlreadyInRange);
        alreadyNearItem.addChild(pickupCloseObj);

        BT_Failer moveAndPickup = new BT_Failer();
        BT_Leaf moveToPickup = new BT_Leaf(Character_Script.MoveToObjectToPickUp);
        moveAndPickup.setChild(moveToPickup);
        
        executePickup.addChild(alreadyNearItem);
        executePickup.addChild(moveAndPickup);
        
        playerPickupSeq.addChild(plrPickupCheck);
        playerPickupSeq.addChild(executePickup);

        // Default action (when all others fail)
        BT_Sequence defaultSequence = new BT_Sequence();
        mainSelector.addChild(defaultSequence);
        
        BT_Inverter invertBucket = new BT_Inverter();
        BT_Leaf bucketCheck = new BT_Leaf(Character_Script.IsHoldingBucket);
        invertBucket.setChild(bucketCheck);
        
        BT_Leaf avoidFire = new BT_Leaf(Utility_Script.AvoidObjectsOnFire);
        
        defaultSequence.addChild(invertBucket);
        defaultSequence.addChild(avoidFire);

    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= 0.1f)
        {
            root.run();
            timer = 0.0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
