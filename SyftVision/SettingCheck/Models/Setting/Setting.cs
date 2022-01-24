namespace SettingCheck.Models
{
    class Setting
    {
        public Setting(string name, double settingvalue, bool isMassDrivenActive)
        {
            Name = name;
            SettingValue = settingvalue;
            IsMassDrivenActive = isMassDrivenActive;
        }
        public string Name { get; private set; }
        public double SettingValue { get; private set; }
        public bool IsMassDrivenActive { get; private set; }

        public double MaximumValue { get; set; }
        public double MinimumValue { get; set; }

        public int ColorGreen { get => IsMassDrivenActive ? unchecked((int)0x8080ff80) : 0x80ff80; }
        public int ColorRed { get => IsMassDrivenActive ? unchecked((int)0x80ff8080) : 0xff8080; }
        public int Color { get => (SettingValue <= MaximumValue && SettingValue >= MinimumValue) ? ColorGreen : ColorRed; }
    }
}
