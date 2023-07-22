using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;



[System.Serializable]
public class SkipList<T> where T : System.IComparable
{
    internal class SkipListNode
    {
        internal T val;
        internal SkipListNode prev;
        internal SkipListNode next;
        internal SkipListNode lower;

        internal SkipListNode(T val)
        {
            this.val = val;
        }

        public override string ToString()
        {
            return "Val: " + val + ", Next: " + (next != null ? next.val : "Null") + ", Prev: " + (prev != null ? prev.val : "Null");
        }
    }

    private SkipListNode head;
    [SerializeField]
    private int levels;

    public SkipList()
    {
        head = new SkipListNode(default);
        levels = 0;
    }

    public bool Add(T val)
    {
        if (head.next == null)
        {
            SkipListNode first = new SkipListNode(val);
            head.next = first;
            first.prev = head;
            return true;
        }

        SkipListNode current = head.next;
        SkipListNode inserted = null;
        SkipListNode[] levelNodes = new SkipListNode[levels];
        int levelInd = 0;

        while (inserted == null)
        {
            //At the bottom level
            if (current.lower == null)
            {
                string err;
                (inserted, err) = AddToLevel(val, ref current);

                if(err != null)
                {
                    Debug.LogWarning(err);
                    return false;
                }
            }
            //Not at the bottom level yet
            else
            {
                //Check if there's still room to move right on this level if next node is less than added val
                if (current.next != null && val.CompareTo(current.next.val) > 0)
                {
                    current = current.next;
                }
                //Check if val is smaller than any nodes on the left
                else if (current.prev != head && val.CompareTo(current.prev.val) < 0)
                {
                    current = current.prev;
                }
                //No where to move on this level, time to move down
                else
                {
                    levelNodes[levelInd] = current;
                    levelInd++;
                    current = current.lower;
                }
            }
        }

        current = inserted;
        while (MoveUp())
        {
            levelInd--;
            SkipListNode levelUp = null;

            //New Levels
            if (levelInd < 0)
            {
                levelUp = new SkipListNode(val);
                levelUp.prev = head;
                head.next = levelUp;
                levels = levels + 1;
            }
            else
            {
                SkipListNode nodeAtLevel = levelNodes[levelInd];
                while (levelUp == null)
                {
                    (levelUp, _) = AddToLevel(val, ref nodeAtLevel);
                }
            }

            levelUp.lower = current;
            current = levelUp;
        }

        return true;
    }

    private (SkipListNode, string) AddToLevel(T val, ref SkipListNode current)
    {
        SkipListNode inserted = null;
        int compared = val.CompareTo(current.val);

        //Added val is greater than current val
        if (compared > 0)
        {
            //Added value is greater than all other values, append to end
            if (current.next == null)
            {
                inserted = new SkipListNode(val);
                inserted.prev = current;
                current.next = inserted;
            }
            //Added value is inbetween current and next value, insert between them
            else if (val.CompareTo(current.next.val) < 0)
            {
                inserted = new SkipListNode(val);
                inserted.prev = current;
                inserted.next = current.next;
                current.next.prev = inserted;
                current.next = inserted;
            }
            //Still room to move right
            else
            {
                current = current.next;
            }
        }
        //Added val is less than current val
        else if (compared < 0)
        {
            //Added val is smallest val, insert at front
            if (current.prev == head)
            {
                inserted = new SkipListNode(val);
                //If no levels have been created yet, head only points to the highest level first node, but all levels point to head
                if (head.next == current)
                {
                    head.next = inserted;
                }
                inserted.prev = current.prev;
                inserted.next = current;
                current.prev = inserted;
            }
            //Added val is in between current and previous, insert between them
            else if (val.CompareTo(current.prev.val) > 0)
            {
                inserted = new SkipListNode(val);
                inserted.next = current;
                inserted.prev = current.prev;
                current.prev.next = inserted;
                current.prev = inserted;
            }
            //Still room to move left
            else
            {
                current = current.prev;
            }
        }
        // Value is same as current value
        else
        {
            return (null, "Value is already in list, not adding another node");
        }

        return (inserted, null);
    }

    private bool MoveUp()
    {
        return Random.value > 0.5f;
    }

    public bool Contains(T val)
    {
        return FindNode(val) != null;
    }

    public bool Remove(T val)
    {
        SkipListNode toDelete = FindNode(val);
        bool deleted = toDelete != null;

        while (toDelete != null)
        {
            // Node has a next node
            if (toDelete.next != null)
            {
                toDelete.next.prev = toDelete.prev;
            }

            // Is entry node
            if (head.next == toDelete)
            {
                // Other node on level to make entry node
                if (toDelete.next != null)
                {
                    head.next = toDelete.next;
                }
                // Node is the only node on the top level, set entry node to left most node of next level
                else if (toDelete.lower != null)
                {
                    head.next = GetLeftMostNode(toDelete.lower);
                }
                // Removing the only node in the list
                else
                {
                    head.next = null;
                }
            }
            else if (toDelete.prev != head)
            {
                toDelete.prev.next = toDelete.next;
            }

            toDelete = toDelete.lower;
        }

        return deleted;
    }

    private SkipListNode FindNode(T val)
    {
        SkipListNode found = null;
        SkipListNode current = head.next;

        while (current != null)
        {
            int compared = val.CompareTo(current.val);

            // Found Val
            if (compared == 0)
            {
                found = current;
                break;
            }

            // val is greater that current value
            if (compared > 0)
            {
                // Move right if val is still higher or equal
                if (current.next != null && val.CompareTo(current.next.val) >= 0)
                {
                    current = current.next;
                }
                // Move down if available
                else if (current.lower != null)
                {
                    current = current.lower;
                }
                // At bottom level, val is greater than current val and less than next val so val doesn't exist
                else
                {
                    break;
                }
            }
            // val is less than current value
            else
            {
                // Move left if prev is not head and val is less than or equal to prev val
                if (current.prev != head && val.CompareTo(current.prev.val) <= 0)
                {
                    current = current.prev;
                }
                // Move down if available
                else if (current.lower != null)
                {
                    current = current.lower;
                }
                // At bottom level, val is less than current and greater than prev so val doesn't exist 
                else
                {
                    break;
                }
            }
        }

        return found;
    }

    public void PrintList()
    {
        StringBuilder sb = new StringBuilder();
        SkipListNode current = head.next;

        while (current != null)
        {
            while (current.next != null)
            {
                sb.Append(current.val + ", ");
                current = current.next;
            }
            sb.Append(current.val + "");
            sb.AppendLine();

            if (current.lower != null)
            {
                current = GetLeftMostNode(current.lower);
            }
            else
            {
                current = null;
            }
        }

        Debug.Log(sb.ToString());
    }

    private SkipListNode GetLeftMostNode(SkipListNode node)
    {
        SkipListNode leftMost = node;
        while (leftMost.prev != head)
        {
            leftMost = leftMost.prev;
        }

        return leftMost;
    }
}
