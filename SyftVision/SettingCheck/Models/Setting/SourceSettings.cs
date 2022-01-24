namespace SettingCheck.Models
{
    class SourceSettings
    {
        public SourcePhaseSettings Pressure { get; set; }
        public SourcePhaseSettings MV { get; set; }
        public SourcePhaseSettings Mesh { get; set; }
    }
}
