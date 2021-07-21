using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Original Code by Michael Audet - Used with Permission by Sri/Trent
namespace COIS2020HLab12
{
    class Node<T>
    {
        public List<Node<T>> children;
        public List<T> keys;

        public Node()
        {
            //set our min number of child nodes 
            children = new List<Node<T>>();
            keys = new List<T>();
        }//end constructor

        /*
         * Checks the number of children to report if we are a leaf node.
         */
        public bool isLeafNode()
        {
            return children.Count == 0;
        }




    }//end Node
}//end namespace
