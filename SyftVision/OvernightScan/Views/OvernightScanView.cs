using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OvernightScan.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class OvernightScanView : UserControl
    {
        public OvernightScanView()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbx = sender as ComboBox;
            var item = cbx.SelectedItem as ComboBoxItem;
            string selectedItem;
            if (item != null)
            {
                selectedItem = item.Content.ToString();

                switch (selectedItem)
                {
                    case "DPIS-Sensitivity&Impurity":
                    case "Infinity-Sensitivity&Impurity":
                        //Cold Start
                        BtnColdStart.Visibility = System.Windows.Visibility.Collapsed;
                        //SPIS
                        BtnOvernight.Visibility = System.Windows.Visibility.Collapsed;
                        BtnOnOff.Visibility = System.Windows.Visibility.Collapsed;
                        BtnEffectofValidation.Visibility = System.Windows.Visibility.Collapsed;
                        //Three Phase Common
                        BtnSensitiveAndImpurity.Visibility = System.Windows.Visibility.Visible;
                        BtnInjectionScan.Visibility = System.Windows.Visibility.Collapsed;
                        BtnICF.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS Common
                        BtnLongTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnShortTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchAndSettleTime.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS
                        BtnUPSAndDWSCurrent.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchStability.Visibility = System.Windows.Visibility.Collapsed;
                        //Infinity
                        BtnStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnLODs.Visibility = System.Windows.Visibility.Collapsed;
                        BtnBackground.Visibility = System.Windows.Visibility.Collapsed;
                        //Combo Box
                        if (CbxNumberofScan != null)
                            CbxNumberofScan.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case "DPIS-ColdStart":
                    case "Infinity-ColdStart":
                        //Cold Start
                        BtnColdStart.Visibility = System.Windows.Visibility.Visible;
                        //SPIS
                        BtnOvernight.Visibility = System.Windows.Visibility.Collapsed;
                        BtnOnOff.Visibility = System.Windows.Visibility.Collapsed;
                        BtnEffectofValidation.Visibility = System.Windows.Visibility.Collapsed;
                        //Three Phase Common
                        BtnSensitiveAndImpurity.Visibility = System.Windows.Visibility.Collapsed;
                        BtnInjectionScan.Visibility = System.Windows.Visibility.Collapsed;
                        BtnICF.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS Common
                        BtnLongTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnShortTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchAndSettleTime.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS
                        BtnUPSAndDWSCurrent.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchStability.Visibility = System.Windows.Visibility.Collapsed;
                        //Infinity
                        BtnStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnLODs.Visibility = System.Windows.Visibility.Collapsed;
                        BtnBackground.Visibility = System.Windows.Visibility.Collapsed;
                        //Combo Box
                        CbxNumberofScan.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case "SPIS-Overnight":
                        //Cold Start
                        BtnColdStart.Visibility = System.Windows.Visibility.Collapsed;
                        //SPIS
                        BtnOvernight.Visibility = System.Windows.Visibility.Visible;
                        BtnOnOff.Visibility = System.Windows.Visibility.Collapsed;
                        BtnEffectofValidation.Visibility = System.Windows.Visibility.Collapsed;
                        //Three Phase Common
                        BtnSensitiveAndImpurity.Visibility = System.Windows.Visibility.Collapsed;
                        BtnInjectionScan.Visibility = System.Windows.Visibility.Collapsed;
                        BtnICF.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS Common
                        BtnLongTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnShortTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchAndSettleTime.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS
                        BtnUPSAndDWSCurrent.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchStability.Visibility = System.Windows.Visibility.Collapsed;
                        //Infinity
                        BtnStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnLODs.Visibility = System.Windows.Visibility.Collapsed;
                        BtnBackground.Visibility = System.Windows.Visibility.Collapsed;
                        //Combo Box
                        CbxNumberofScan.Visibility = System.Windows.Visibility.Visible;
                        Number3.Visibility = System.Windows.Visibility.Collapsed;
                        Number4.Visibility = System.Windows.Visibility.Collapsed;
                        Number5.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case "SPIS-On/Off":
                        //Cold Start
                        BtnColdStart.Visibility = System.Windows.Visibility.Collapsed;
                        //SPIS
                        BtnOvernight.Visibility = System.Windows.Visibility.Collapsed;
                        BtnOnOff.Visibility = System.Windows.Visibility.Visible;
                        BtnEffectofValidation.Visibility = System.Windows.Visibility.Collapsed;
                        //Three Phase Common
                        BtnSensitiveAndImpurity.Visibility = System.Windows.Visibility.Collapsed;
                        BtnInjectionScan.Visibility = System.Windows.Visibility.Collapsed;
                        BtnICF.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS Common
                        BtnLongTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnShortTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchAndSettleTime.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS
                        BtnUPSAndDWSCurrent.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchStability.Visibility = System.Windows.Visibility.Collapsed;
                        //Infinity
                        BtnStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnLODs.Visibility = System.Windows.Visibility.Collapsed;
                        BtnBackground.Visibility = System.Windows.Visibility.Collapsed;
                        //Combo Box
                        CbxNumberofScan.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case "SPIS-EOV":
                    case "DPIS-EOV":
                        //Cold Start
                        BtnColdStart.Visibility = System.Windows.Visibility.Collapsed;
                        //SPIS
                        BtnOvernight.Visibility = System.Windows.Visibility.Collapsed;
                        BtnOnOff.Visibility = System.Windows.Visibility.Collapsed;
                        BtnEffectofValidation.Visibility = System.Windows.Visibility.Visible;
                        //Three Phase Common
                        BtnSensitiveAndImpurity.Visibility = System.Windows.Visibility.Collapsed;
                        BtnInjectionScan.Visibility = System.Windows.Visibility.Collapsed;
                        BtnICF.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS Common
                        BtnLongTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnShortTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchAndSettleTime.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS
                        BtnUPSAndDWSCurrent.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchStability.Visibility = System.Windows.Visibility.Collapsed;
                        //Infinity
                        BtnStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnLODs.Visibility = System.Windows.Visibility.Collapsed;
                        BtnBackground.Visibility = System.Windows.Visibility.Collapsed;
                        //Combo Box
                        CbxNumberofScan.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case "SPIS-Weekend":
                        //Cold Start
                        BtnColdStart.Visibility = System.Windows.Visibility.Collapsed;
                        //SPIS
                        BtnOvernight.Visibility = System.Windows.Visibility.Visible;
                        BtnOnOff.Visibility = System.Windows.Visibility.Collapsed;
                        BtnEffectofValidation.Visibility = System.Windows.Visibility.Visible;
                        //Three Phase Common
                        BtnSensitiveAndImpurity.Visibility = System.Windows.Visibility.Collapsed;
                        BtnInjectionScan.Visibility = System.Windows.Visibility.Collapsed;
                        BtnICF.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS Common
                        BtnLongTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnShortTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchAndSettleTime.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS
                        BtnUPSAndDWSCurrent.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchStability.Visibility = System.Windows.Visibility.Collapsed;
                        //Infinity
                        BtnStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnLODs.Visibility = System.Windows.Visibility.Collapsed;
                        BtnBackground.Visibility = System.Windows.Visibility.Collapsed;
                        //Combo Box
                        CbxNumberofScan.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case "DPIS-Coarse":
                        //Cold Start
                        BtnColdStart.Visibility = System.Windows.Visibility.Collapsed;
                        //SPIS
                        BtnOvernight.Visibility = System.Windows.Visibility.Collapsed;
                        BtnOnOff.Visibility = System.Windows.Visibility.Collapsed;
                        BtnEffectofValidation.Visibility = System.Windows.Visibility.Collapsed;
                        //Three Phase Common
                        BtnSensitiveAndImpurity.Visibility = System.Windows.Visibility.Collapsed;
                        BtnInjectionScan.Visibility = System.Windows.Visibility.Visible;
                        BtnICF.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS Common
                        BtnLongTermStability.Visibility = System.Windows.Visibility.Visible;
                        BtnShortTermStability.Visibility = System.Windows.Visibility.Visible;
                        BtnSourceSwitchAndSettleTime.Visibility = System.Windows.Visibility.Visible;
                        //DPIS
                        BtnUPSAndDWSCurrent.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchStability.Visibility = System.Windows.Visibility.Collapsed;
                        //Infinity
                        BtnStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnLODs.Visibility = System.Windows.Visibility.Collapsed;
                        BtnBackground.Visibility = System.Windows.Visibility.Collapsed;
                        //Combo Box
                        CbxNumberofScan.Visibility = System.Windows.Visibility.Visible;
                        Number3.Visibility = System.Windows.Visibility.Visible;
                        Number4.Visibility = System.Windows.Visibility.Visible;
                        Number5.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case "DPIS-Fine":
                        //Cold Start
                        BtnColdStart.Visibility = System.Windows.Visibility.Collapsed;
                        //SPIS
                        BtnOvernight.Visibility = System.Windows.Visibility.Collapsed;
                        BtnOnOff.Visibility = System.Windows.Visibility.Collapsed;
                        BtnEffectofValidation.Visibility = System.Windows.Visibility.Collapsed;
                        //Three Phase Common
                        BtnSensitiveAndImpurity.Visibility = System.Windows.Visibility.Visible;
                        BtnInjectionScan.Visibility = System.Windows.Visibility.Visible;
                        BtnICF.Visibility = System.Windows.Visibility.Visible;
                        //DPIS Common
                        BtnLongTermStability.Visibility = System.Windows.Visibility.Visible;
                        BtnShortTermStability.Visibility = System.Windows.Visibility.Visible;
                        BtnSourceSwitchAndSettleTime.Visibility = System.Windows.Visibility.Visible;
                        //DPIS
                        BtnUPSAndDWSCurrent.Visibility = System.Windows.Visibility.Visible;
                        BtnSourceSwitchStability.Visibility = System.Windows.Visibility.Visible;
                        //Infinity
                        BtnStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnLODs.Visibility = System.Windows.Visibility.Collapsed;
                        BtnBackground.Visibility = System.Windows.Visibility.Collapsed;
                        //Combo Box
                        CbxNumberofScan.Visibility = System.Windows.Visibility.Visible;
                        Number3.Visibility = System.Windows.Visibility.Visible;
                        Number4.Visibility = System.Windows.Visibility.Visible;
                        Number5.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case "Infinity":
                        //Cold Start
                        BtnColdStart.Visibility = System.Windows.Visibility.Collapsed;
                        //SPIS
                        BtnOvernight.Visibility = System.Windows.Visibility.Collapsed;
                        BtnOnOff.Visibility = System.Windows.Visibility.Collapsed;
                        BtnEffectofValidation.Visibility = System.Windows.Visibility.Collapsed;
                        //Three Phase Common
                        BtnSensitiveAndImpurity.Visibility = System.Windows.Visibility.Visible;
                        BtnInjectionScan.Visibility = System.Windows.Visibility.Visible;
                        BtnICF.Visibility = System.Windows.Visibility.Visible;
                        //DPIS Common
                        BtnLongTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnShortTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchAndSettleTime.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS
                        BtnUPSAndDWSCurrent.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchStability.Visibility = System.Windows.Visibility.Collapsed;
                        //Infinity
                        BtnStability.Visibility = System.Windows.Visibility.Visible;
                        BtnLODs.Visibility = System.Windows.Visibility.Visible;
                        BtnBackground.Visibility = System.Windows.Visibility.Visible;
                        //Combo Box
                        CbxNumberofScan.Visibility = System.Windows.Visibility.Visible;
                        Number3.Visibility = System.Windows.Visibility.Visible;
                        Number4.Visibility = System.Windows.Visibility.Visible;
                        Number5.Visibility = System.Windows.Visibility.Visible;
                        break;
                    case "Infinity-EOB":
                        //Cold Start
                        BtnColdStart.Visibility = System.Windows.Visibility.Collapsed;
                        //SPIS
                        BtnOvernight.Visibility = System.Windows.Visibility.Collapsed;
                        BtnOnOff.Visibility = System.Windows.Visibility.Collapsed;
                        BtnEffectofValidation.Visibility = System.Windows.Visibility.Collapsed;
                        //Three Phase Common
                        BtnSensitiveAndImpurity.Visibility = System.Windows.Visibility.Visible;
                        BtnInjectionScan.Visibility = System.Windows.Visibility.Collapsed;
                        BtnICF.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS Common
                        BtnLongTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnShortTermStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchAndSettleTime.Visibility = System.Windows.Visibility.Collapsed;
                        //DPIS
                        BtnUPSAndDWSCurrent.Visibility = System.Windows.Visibility.Collapsed;
                        BtnSourceSwitchStability.Visibility = System.Windows.Visibility.Collapsed;
                        //Infinity
                        BtnStability.Visibility = System.Windows.Visibility.Collapsed;
                        BtnLODs.Visibility = System.Windows.Visibility.Visible;
                        BtnBackground.Visibility = System.Windows.Visibility.Collapsed;
                        //Combo Box
                        CbxNumberofScan.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
