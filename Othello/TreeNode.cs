using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Othello
{
    class TreeNode<T>
    {
        private T value;
        private TreeNode<T> parent;

        /// <summary>
        /// Evaluation function
        /// </summary>
        public void Eval()
        {
           //TODO 
        }

        /// <summary>
        /// Is the game in final state
        /// </summary>
        /// <returns>True if the game is over</returns>
        public bool Final()
        {
            //TODO
            return true;
        }

        /// <summary>
        /// Returns the applicable operators
        /// </summary>
        /// <returns>List of operators</returns>
        public List<TreeNode<T>> Ops()
        {
            //TODO
            return null;
        }

        /// <summary>
        /// Apply opperator on this node
        /// </summary>
        /// <returns></returns>
        public TreeNode<T> Apply(TreeNode<T> op)
        {
            //TODO
            return null;
        }
    }
}
