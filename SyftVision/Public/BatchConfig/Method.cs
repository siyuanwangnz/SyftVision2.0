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
        public string MethodName { get; set; }
        public ObservableCollection<ChartProp> ChartsList { get; set; }
    }
}
