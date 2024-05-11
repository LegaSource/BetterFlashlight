using BepInEx.Configuration;
using BepInEx;
using HarmonyLib;
using BetterFlashlight.Patches;

namespace BetterFlashlight
{
    [BepInPlugin(modGUID, modName, modVersion)]
    internal class BetterFlashlight : BaseUnityPlugin
    {
        private const string modGUID = "Lega.BetterFlashlight";
        private const string modName = "Better Flashlight";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static BetterFlashlight Instance;

        public static ConfigFile configFile;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            configFile = Config;
            ConfigManager.Load();

            harmony.PatchAll(typeof(BetterFlashlight));
            harmony.PatchAll(typeof(FlashlightItemPatch));
        }
    }
}
