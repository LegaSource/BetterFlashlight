using BepInEx.Configuration;
using BepInEx;
using HarmonyLib;
using BetterFlashlight.Patches;
using System.Collections.Generic;

namespace BetterFlashlight
{
    [BepInPlugin(modGUID, modName, modVersion)]
    internal class BetterFlashlight : BaseUnityPlugin
    {
        private const string modGUID = "Lega.BetterFlashlight";
        private const string modName = "Better Flashlight";
        private const string modVersion = "1.0.5";

        private readonly Harmony harmony = new Harmony(modGUID);

        public static ConfigFile configFile;
        public static List<FlashlightStun> flashlightStuns = new List<FlashlightStun>();

        void Awake()
        {
            configFile = Config;
            ConfigManager.Load();
            flashlightStuns = ConfigManager.GetFlashlightStunsFromConfig();

            harmony.PatchAll(typeof(BetterFlashlight));
            harmony.PatchAll(typeof(FlashlightItemPatch));
            harmony.PatchAll(typeof(EnemyAIPatch));
            harmony.PatchAll(typeof(RoundManagerPatch));
        }
    }
}
