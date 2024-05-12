using BepInEx.Configuration;
using System.Collections.Generic;

namespace BetterFlashlight
{
    internal class ConfigManager
    {
        // ENEMY VALUES
        public static ConfigEntry<float> enemyFlashTime;
        public static ConfigEntry<float> enemyStunTime;
        public static ConfigEntry<float> enemyLightAngle;
        public static ConfigEntry<float> enemyAngle;
        public static ConfigEntry<float> enemyDistance;
        public static ConfigEntry<string> enemyValues;
        public static ConfigEntry<string> exclusions;
        // PLAYER VALUES
        public static ConfigEntry<bool> isPlayerBlind;
        public static ConfigEntry<float> playerFlashTime;
        public static ConfigEntry<float> playerLightAngle;
        public static ConfigEntry<float> playerAngle;
        public static ConfigEntry<float> playerDistance;

        internal static void Load()
        {
            // ENEMY VALUES
            enemyFlashTime = BetterFlashlight.configFile.Bind<float>("Enemy values", "Flash Time", 2f, "Default time required to blind an enemy.");
            enemyStunTime = BetterFlashlight.configFile.Bind<float>("Enemy values", "Stun Time", 3f, "Default duration of the stun.");
            enemyLightAngle = BetterFlashlight.configFile.Bind<float>("Enemy values", "Light Angle", 15f, "Default angle of the light in relation to the enemy's eyes.\nIncreasing this value makes aiming easier.");
            enemyAngle = BetterFlashlight.configFile.Bind<float>("Enemy values", "Light Angle", 120f, "Default angle of the enemy in relation to the flashlight.\nIncreasing this value makes blinding from an angle easier.");
            enemyDistance = BetterFlashlight.configFile.Bind<float>("Enemy values", "Distance", 10f, "Default distance between the enemy and the flashlight.");
            enemyValues = BetterFlashlight.configFile.Bind<string>("Enemy values", "Values", "ForestGiant:2:4:30:120:20", "Values per enemy, the format is EnemyName:FlashTime:StunTime:LightAngle:EnemyAngle:EnemyDistance.");
            exclusions = BetterFlashlight.configFile.Bind<string>("Enemy values", "Exclusion list", null, "List of creatures that will not be affected by the stun.");
            // PLAYER VALUES
            isPlayerBlind = BetterFlashlight.configFile.Bind<bool>("Player values", "Enable", true, "Can the player be blinded?");
            playerFlashTime = BetterFlashlight.configFile.Bind<float>("Player values", "Flash Time", 2f, "Time required to blind a player.");
            playerLightAngle = BetterFlashlight.configFile.Bind<float>("Player values", "Light Angle", 15f, "Angle of the light in relation to the player's eyes.\nIncreasing this value makes aiming easier.");
            playerAngle = BetterFlashlight.configFile.Bind<float>("Player values", "Light Angle", 120f, "Angle of the player in relation to the flashlight.\nIncreasing this value makes blinding from an angle easier.");
            playerDistance = BetterFlashlight.configFile.Bind<float>("Enemy values", "Distance", 10f, "Default distance between the aimed player and the flashlight.");
        }

        internal static List<FlashlightStun> GetFlashlightStunsFromConfig()
        {
            List<FlashlightStun> flashlightStuns = new List<FlashlightStun>();
            string[] enemies = enemyValues.Value.Split(',');
            foreach (string enemyValue in enemies)
            {
                string[] values = enemyValue.Split(':');
                if (values.Length == 6)
                {
                    flashlightStuns.Add(new FlashlightStun(values[0], float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5])));
                }
            }
            return flashlightStuns;
        }
    }
}
