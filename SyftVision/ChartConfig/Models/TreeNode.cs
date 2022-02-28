using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartConfig.Models
{
    public class TreeNode
    {
        public string Name { get; set; }
        public TreeNode Parent { get; set; }
        public List<TreeNode> ChildNodes { get; set; }
    }
}
