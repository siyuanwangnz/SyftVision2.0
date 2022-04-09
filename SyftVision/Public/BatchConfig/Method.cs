using Public.ChartConfig;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.BatchConfig
{
    public class Method
    {
        public string Name { get; set; } = "";
        public string NameCode { get => Name.ToLower().Replace(" ", ""); }
        public int NameHashCode { get => NameCode.GetHashCode(); }
        public ObservableCollection<string> ChartCodeList { get; set; } = new ObservableCollection<string>();
        public List<int> ChartHashCodeList { get => ChartCodeList.Select(a => a.GetHashCode()).ToList(); }
    }
}
