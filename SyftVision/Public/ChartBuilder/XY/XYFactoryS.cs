using Public.SettingConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.ChartBuilder.XY
{
    public abstract class XYFactoryS
    {
        public abstract XYItemS GetXYItemS(Setting setting);
    }
}
