namespace AdAstra.Engine
{
    internal class Settings
    {
        public int WindowWidth { get; set; } = 1280;
        public int WindowHeight { get; set; } = 720;
        public bool Fullscreen { get; set; } = false;
        public bool Borderless { get; set; } = true;
        public bool VSync { get; set; } = false;
        public float TargetFrameRate { get; set; } = 60.0f;
        public bool ShowFPS { get; set; } = true;
        public float MasterVolume { get; set; } = 1.0f;
        public float MusicVolume { get; set; } = 0.8f;
        public float SFXVolume { get; set; } = 1.0f;
        public string Language { get; set; } = "en-US";
    }
}
