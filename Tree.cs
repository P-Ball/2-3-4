using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Original Code by Michael Audet - Used with Permission by Sri/Trent
namespace COIS2020HLab12
{
    class Tree<T> where T : IComparable
    {
        //public for testing
        public Node<T> root;//the root of our tree
        private int maxKeysPerNode;
        private int minKeysPerNode;
        private int maxChildrenPerNode;

        public Tree()
        {
            //get our root.
            int t = 2;
            this.root = new Node<T>();
            this.maxKeysPerNode = 2 * t - 1;
            this.minKeysPerNode = t - 1;
            this.maxChildrenPerNode = 2 * t;

        }//end Tree constructor

        /*
         * Insert()
         * 
         * parameter: the key to insert of type T
         * returns true on success and false if key was already in the tree.
         */
        public bool Insert(T key)
        {
            //we have to check if the root needs to be split.
            //this is a special case.
            if(this.root.keys.Count == this.maxKeysPerNode)
            {
                Node<T> newRoot = new Node<T>();
                //we add the left child
                newRoot.children.Add(this.root);
                SplitNode(newRoot, 0);
                this.root = newRoot; 
               
            }
            return Insert(this.root, key);
        }//end Insert

        /*
         * Private helper method that takes a node as a paramenter for recursive calls.
         * We are guaranteed to have room in node.
         */
        private bool Insert(Node<T> node, T key)
        {
            //base case
            if(node.isLeafNode())
            {
                int index = 0;
                //get the index to insert into.
                for(; index <= node.keys.Count; index++)
                {
                    if(index == node.keys.Count ||
                        key.CompareTo(node.keys[index]) < 0)
                    {
                        break;
                    }
                }//end for

                //we're done.  It's in.
                if (!node.keys.Contains(key))
                {
                    node.keys.Insert(index, key);
                    return true;
                }
                else
                {
                    //already there.
                    return false;
                }
            }
            else //recursive case
            {
                //find the child node we need to send this to.
                //and split it if necessary
                int childIndex = this.getChildIndex(node, key);
                if(childIndex == -1)
                {
                    //probably already there.
                    return false;
                }
                //this does nothing if node not full
                this.SplitNode(node, childIndex);
                //the child index may have changed if we split
                childIndex = this.getChildIndex(node, key);
                return this.Insert(node.children[childIndex], key);
            }
   
        }

        /*
         * getChildIndexForInsert
         * this returns the index to go down in an insert or deletion
         * returns - 1 if no children.
         */
         private int getChildIndex(Node<T> node, T key)
        {
            //there is no child index if we have no children or if the key is in this node.
            if(node.children.Count == 0 || node.keys.Contains(key))
            {
                return -1;
            }

            //go through all the child indexes.  If we hit the count of
            //the keys, we are bigger than all the values in this node.
            //we go to the last index of the children.
            //otherwise if we are less than a key
            //we go to the same index in the children list as the key we
            //are less than.
            int index = 0;
            for(;index < node.children.Count; index++)
            {
                if (index == node.keys.Count ||
                    key.CompareTo(node.keys[index]) < 0)
                {
                    //we've got our index
                    break;
                }
            }

            return index;
        }//end getChildIndex

        /*
         * splitNode takes as a parameter a node to split splits it's ith child
         * note: node is guaranteed to have space for at leat one additional item.
         */
        private void SplitNode(Node<T> node, int index)
        {
            Node<T> childNodeToSplit = node.children[index];
            if(childNodeToSplit == null)
            {
                Console.WriteLine("Error! child node to split is null!!!");
                return;
            }
            //we only split if the child is full
            if (childNodeToSplit.keys.Count == this.maxKeysPerNode)
            {
                //get the index of the middle value
                int middleIndex = this.maxKeysPerNode / 2;//truncation takes care of the floor
                //get the middle key to insert into the parent
                T middleKey = childNodeToSplit.keys[middleIndex];

                       

                //this will take the "Big side" of the child to split
                Node<T> newBigChild = new Node<T>();


                //mode the keys over.  We start at the end so that we don't disturb the indexes when we remove the items.
                for(int keyIndex = this.maxKeysPerNode - 1; keyIndex > middleIndex; keyIndex--)
                {
                    //move the value over
                    //put each item at the beginning of the list to maintain the order.
                      newBigChild.keys.Insert(0, childNodeToSplit.keys[keyIndex]);
                    //remove it from the old node
                    childNodeToSplit.keys.RemoveAt(keyIndex);
                }

                //we can't move children over if we're a leaf node!
                if(!childNodeToSplit.isLeafNode())
                {
                    //move the large half of the children over to the new node.
                    for (int childIndex = this.maxChildrenPerNode - 1; childIndex > middleIndex; childIndex--)
                    {
                        //move the value over
                        newBigChild.children.Insert(0, childNodeToSplit.children[childIndex]);
                        //remove it from the old node
                        childNodeToSplit.children.RemoveAt(childIndex);
                    }
                }//end for each of the children on the right side
                //remove the middle value we are moving to the parent from the smaller child
                childNodeToSplit.keys.RemoveAt(middleIndex);

                //insert the middleKey into the parent.  It goes at index because old split child is ending up to its left.
                node.keys.Insert(index, middleKey);

                //insert the new bigger node into the parent at index + 1 because we are to the right of the new entry
                node.children.Insert(index + 1, newBigChild);


                //and we're done.
            }//end if we split

            //else do nothing.
            
        }//end split node.

        /*
         * Delete()
         * parameter: the key to delete of type T
         * returns true if the item is deleted and false if it was not found.
        
        public bool Delete(T key)
        {
TO BE IMPLEMENTED BY STUDENT

        }//end Delete
         */
        /*
         * getAndDeleteLargest()
         * This deletes the largest value in a sub tree and returns its value.
         * It's called to get the largest value from a sub tree to repalce a value when deleteing another
         * value.
         */
        private T getAndDeleteLargest(Node<T> node)
        {
            //check for bad input
            if(node == null)
            {
                Console.WriteLine("getAndDeleteLargest: bad input!");
                return default(T);

            }

            //base case - largest will always be in a leaf node.
            if(node.isLeafNode())
            {
                T largestValue =  node.keys[node.keys.Count - 1];
                node.keys.RemoveAt(node.keys.Count - 1);
                return largestValue;

            }
            else
            {
                //got the largest from our largest child.
                this.ensureTValues(node, node.children.Count - 1);
                return getAndDeleteLargest(node.children[node.children.Count - 1]);
            }
        }

        /*
        * getAndDeleteSmallest()
        * This deletes the smallest value in a sub tree and returns its value.
        * It's called to get the smallest value from a right sub tree to repalce a value when deleteing another
        * value.
        */
        private T getAndDeleteSmallest(Node<T> node)
        {
            //check for bad input
            if (node == null)
            {
                Console.WriteLine("getAndDeleteSmallest: bad input!");
                return default(T);

            }

            //base case - largest will always be in a leaf node.
            if (node.isLeafNode())
            {
                T smallestValue = node.keys[0];
                node.keys.RemoveAt(0);
                return smallestValue;
  
            }
            else
            {
                //got the largest from our largest child.
                this.ensureTValues(node, 0);
                return getAndDeleteSmallest(node.children[0]);
            }
        }

        /*
         * Deletes a value from a node.
         * We are guaranteed to have at least t values in the node so that we can
         * remove one.
         

        private bool Delete(Node<T> node, T key)
        {
     
TO BE IMPLEMENTED BY STUDENT


            }//end else
       
        }//end Delete
        */

        //borrows a value from a left sibling, moves it into the parent, and moves a value down into the rigth sibling
        //to get t values in a delete.\
        //Public for testing
        public bool borrowFromLeftSibling(Node<T> parentNode, int index)
        {
            //chek for bogus input.
            if(parentNode == null ||  index >= parentNode.children.Count || index == 0) //can't borrow left if we are the furthest left
            {
                //no can do.
                Console.WriteLine("boorowFromLeftSibling: bad input!");
                return false;
            }

            Node<T> leftSibling = parentNode.children[index];
            Node<T> rightSibling = parentNode.children[index + 1];

            //checked for more bogus input
            if (leftSibling.keys.Count < this.minKeysPerNode + 1)
            {
                //no can do.
                Console.WriteLine("boorowFromLeftSibling: not enough keys to borrow from!");
                return false;
            }//end if not enough values.


            //we are always taking the largest value from the left sibling, and it's child becomes the first child in the right sibling.
            //the value we are moving down to the right child becomes the first key in the right child.
            //move the value at index down.

            //put value from parent into rigth child.
            rightSibling.keys.Insert(0, parentNode.keys[index]);

            //put the highest value from the left sibling into the parent.
            parentNode.keys[index] = leftSibling.keys[leftSibling.keys.Count - 1];

            //clear out the highest key from the left sibling
            leftSibling.keys.RemoveAt(leftSibling.keys.Count - 1);

            if (!leftSibling.isLeafNode())
            {
                //put the last child from left sibling into first position of rigth sibling.
                rightSibling.children.Insert(0, leftSibling.children[leftSibling.children.Count - 1]);

                //clear out the pointer in the left sibling that we just moved
                leftSibling.children.RemoveAt(leftSibling.children.Count - 1);
            }
            //we're done.
          
            return true;
        }//end boorowFromLeftSibling


        //Borrows a value from a right sibling, moves it into the parent, and moves a value down into the left sibling
        //to get t values in a delete.
        //Public for testing
        public bool boorowFromRightSibling(Node<T> parentNode, int index)
        {
            //chek for bogus input.
            if (parentNode == null || index  > parentNode.children.Count -2)//we need to have a right sibling to borrow from it
            {
                //no can do.
                Console.WriteLine("boorowFromRightSibling: bad input!");
                return false;
            }

            Node<T> leftSibling = parentNode.children[index];
            Node<T> rightSibling = parentNode.children[index + 1];

            //checked for more bogus input
            if(rightSibling.keys.Count < this.minKeysPerNode + 1)
            {
                //no can do.
                Console.WriteLine("boorowFromRightSibling: not enough keys to borrow from!");
                return false;
            }//end if not enough values.


                //we are always taking the largest value from the left sibling, and it's child becomes the first child in the right sibling.
                //the value we are moving down to the right child becomes the first key in the right child.
                //move the value at index down.

            //put value from parent into the highest index of the left sibling.
            leftSibling.keys.Add(parentNode.keys[index]);

            //put the lowest value from the right sibling into the parent.
            parentNode.keys[index] = rightSibling.keys[0];

            //clear out the lowest key from the right sibling
            rightSibling.keys.RemoveAt(0);

            if (!rightSibling.isLeafNode())
            {
                //put the first child from right sibling into last position of left sibling.
                leftSibling.children.Add(rightSibling.children[0]);

                //clear out the pointer in the right sibling that we just moved
                rightSibling.children.RemoveAt(0);
            }

            //we're done.

            return true;
        }//end boorowFromLeftSibling


        /*
         * This takes a node and an index.  It merges the two children to the left and the right of the index.
         * Public for testing.
         */
        public bool MergeNodes(Node<T> parentNode, int index)
        {
            if(parentNode.isLeafNode() || index >= parentNode.keys.Count)
            {
                //we have a bad child index.
                Console.WriteLine("MergeNodes: bad child index!");
                return false;
            }
            //this shouldn'd happen.  We should only be merging a node if the left and right nodes
            //have the min.
            if(parentNode.children[index].keys.Count != this.minKeysPerNode ||
                parentNode.children[index + 1].keys.Count != this.minKeysPerNode)
            {
                //something went wrong
                Console.WriteLine("MergeNodes: left and rigth are too big!!");
                return false;
            }

            //copy keys
            int middleKeyIndex = this.maxKeysPerNode / 2;
            //move down to maintain order.
            for(int keyIndex = middleKeyIndex - 1; keyIndex >= 0; keyIndex--)
            {
                //move the left nodes keys into the right node.
                parentNode.children[index + 1].keys.Insert(0, parentNode.children[index].keys[keyIndex]);
                //no need to clear our the old left.  It's going away.
            }//end for

            //note: we don't need to check both children.  All leaf nodes are at the same level.
            if (!parentNode.children[index].isLeafNode())
            {
                int middleChildIndex = this.maxChildrenPerNode / 2;
                for (int childIndex = middleChildIndex - 1; childIndex >= 0; childIndex--)
                {
                    //move the left child pointers to the right node.
                    parentNode.children[index + 1].children.Insert(0, parentNode.children[index].children[childIndex]);
                }//end for the children
            }//end if we have children

            //mode the new middle key down into it's right child
            parentNode.children[index + 1].keys.Insert(middleKeyIndex, parentNode.keys[index]);

            //remove the value from the parent and delete the old left child now that everything is in the right one.
            parentNode.keys.RemoveAt(index);
            parentNode.children.RemoveAt(index);
            
           
            return true;

        }//end mergeNodes


        /*
         * ensureTValues()
         * 
         * This methid checks a node's child at an index
         * to ensure qwe have t values there before we descend.
         */
         private bool ensureTValues(Node<T> parentNode, int childIndex)
        {
            bool mergeResult;

            //check for bad input
            if (parentNode == null || childIndex >= parentNode.children.Count)
            {
                Console.WriteLine("ensureTValues: we got bad input!");
                return false;
            }//end if

            //check to see if we already have t values
            if(parentNode.children[childIndex].keys.Count > this.minKeysPerNode)
            {
                //we're already good.
                return true;
            }

             //if we're here, we have some work to do.

            //if we have a left sibling
            if (childIndex > 0 &&
                parentNode.children[childIndex - 1].keys.Count > this.minKeysPerNode)
            {
                //we can do it!  key index in parent is index -1 because we are the right sibling, which is index + 1.
                return this.borrowFromLeftSibling(parentNode, childIndex - 1);
              
            }
            //if we have a right sibling
            else if(childIndex < parentNode.children.Count - 1 &&
                parentNode.children[childIndex + 1].keys.Count > this.minKeysPerNode)
            {
                //we can do it!  key index in parent is index because we are the left sibling, which is the same as the key index.
                return this.boorowFromRightSibling(parentNode, childIndex);
            }

            
            //else we have to merge.
            //we know that we have a sibling that we can merge with because otherwise we could have borrowed.
            else if (childIndex > 0)
            {
                //we have a left sibling to merge with
                //since we are the right sibling, the key index is our index - 1.
                mergeResult = this.MergeNodes(parentNode, childIndex - 1);
            }
            else
            {
                //since we are the left sibling, the key index is our index.
                mergeResult = this.MergeNodes(parentNode, childIndex);
            }

            //speical case: the root just got emptied
            if(mergeResult && parentNode == root && parentNode.keys.Count == 0)
            {
                this.root = this.root.children[0];
            }
            return mergeResult;

       
                        

        }//end ensureTvalues

      
        /*
         * Contains()
         * paramter: the key to search for of type T
         * returns true if key found, false otherwise.
         * 
         */
        public bool Contains(T key)
        {
            return Contains(this.root, key);

        }//end Contains

        /*
         * Private helper method for contains that takes a node for recursive calls.
         */
         private bool Contains(Node<T> node, T key)
        {
            //if we're null, it isn't here.
            if(node == null)
            {
                return false;
            }
            //if this node has the key, then we found it
            else if(node.keys.Contains(key))
            {
                return true;
            }

            else //keep looking
            {
                //if we have no children to check, it isn't there.
                if(node.children.Count == 0)
                {
                    return false;
                }
                //else we keep looing
                int index = 0;
                //get the index of the item that is bigger than us
                //or the last index of the children because we're bigger
                for(;index < node.children.Count; index++)
                {
                    if(index == node.keys.Count ||
                        key.CompareTo(node.keys[index]) < 0)
                    {
                        break;
                    }
                }//end for
                return Contains(node.children[index], key);

            }//end else we didn't find it

        }

        /*
         * Print()
         * 
         * Prints all the keys in the tree in order.
         * Takes an optional parammter to suppress the output in testing.
         * Returns a string representaiton of the keys in order.
         * 
         */
        public String Print(bool suppressOutput = false)
        {
            String printout = "";
            //check for an empty tree
            if(this.root == null || root.keys.Count == 0)
            {
                return printout;
            }
            this.Print(this.root, ref printout);

            //get rid of the trailing comma and space.
            printout = printout.Substring(0, printout.Length - 2);
            if(!suppressOutput)
            {
                Console.WriteLine(printout);
            }
            return printout;
        }//end Print

        /*
         * Print()
         * private recursive helper method for print.
         * This takes a node and a reference to a tring that will be populated with the key values.
         */
        private void Print(Node<T> node, ref String printout)
        {
            if(node.isLeafNode())
            {
                foreach(T key in node.keys)
                {
                    printout += key.ToString() + ", ";
                }//end foreach
                //no children to worry about.
            }
            else //we have children
            {
                for(int index = 0; index < node.children.Count; index++)
                {
                    Print(node.children[index], ref printout);
                    if(index < node.keys.Count)
                    {
                        printout += node.keys[index].ToString() + ", ";
                    }

                }//end for each child
            }
        }




    }//end class Tree
}//end namespace
