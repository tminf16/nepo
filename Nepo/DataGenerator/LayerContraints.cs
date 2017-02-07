namespace Nepo.DataGenerator
{
    public class LayerConstraints
    {
        public int MinDeadzoneCount { get; set; } = 1;

        public int MaxDeadzoneCount { get; set; } = 5;

        public int DeadzoneBaseSize { get; set; } = 50;

        public int MinHotspots { get; set; } = 10;

        public int MaxHotspots { get; set; } = 30;

        public int HotspotsSize { get; set; } = 30;

    }
}