using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.TreeList
{
    public class TreeNode
    {
        public string Name { get; set; }
        public string Parent { get; set; }
        public List<TreeNode> ChildNodes { get; set; }
    }
}
