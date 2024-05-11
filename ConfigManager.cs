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
            flashTime = BetterFlashlight.configFile.Bind<float>("_Global_", "Flash Time", 2f, "Time required to blind an enemy.");
            stunTime = BetterFlashlight.configFile.Bind<float>("_Global_", "Stun Time", 3f, "Duration of the stun.");
            lightAngle = BetterFlashlight.configFile.Bind<float>("_Global_", "Light Angle", 15f, "Angle of the light in relation to the enemy's eyes.\nIncreasing this value makes aiming easier.");
            enemyAngle = BetterFlashlight.configFile.Bind<float>("_Global_", "Light Angle", 120f, "Angle of the enemy in relation to the flashlight.\nIncreasing this value makes blinding from an angle easier.");
        }
    }
}
