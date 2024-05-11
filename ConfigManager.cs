using BepInEx.Configuration;

namespace BetterFlashlight
{
    internal class ConfigManager
    {
        // GLOBAL
        public static ConfigEntry<float> flashTime;
        public static ConfigEntry<float> stunTime;
        public static ConfigEntry<float> lightAngle;
        public static ConfigEntry<float> enemyAngle;

        internal static void Load()
        {
            // GLOBAL
            flashTime = BetterFlashlight.configFile.Bind<float>("_Global_", "Flash Time", 2f, "Overall chance of scrap appearing.\nThis value does not replace the chance of appearance for each curse; the latter are considered after the overall chance to determine which curse is chosen.\nYou can adjust this value according to the moon by adding its name along with its value (moon:value). Each key/value pair should be separated by a comma.");
            stunTime = BetterFlashlight.configFile.Bind<float>("_Global_", "Stun Time", 3f, "Overall chance of scrap appearing.\nThis value does not replace the chance of appearance for each curse; the latter are considered after the overall chance to determine which curse is chosen.\nYou can adjust this value according to the moon by adding its name along with its value (moon:value). Each key/value pair should be separated by a comma.");
            lightAngle = BetterFlashlight.configFile.Bind<float>("_Global_", "Light Angle", 15f, "Overall chance of scrap appearing.\nThis value does not replace the chance of appearance for each curse; the latter are considered after the overall chance to determine which curse is chosen.\nYou can adjust this value according to the moon by adding its name along with its value (moon:value). Each key/value pair should be separated by a comma.");
            enemyAngle = BetterFlashlight.configFile.Bind<float>("_Global_", "Light Angle", 120f, "Overall chance of scrap appearing.\nThis value does not replace the chance of appearance for each curse; the latter are considered after the overall chance to determine which curse is chosen.\nYou can adjust this value according to the moon by adding its name along with its value (moon:value). Each key/value pair should be separated by a comma.");
        }
    }
}
