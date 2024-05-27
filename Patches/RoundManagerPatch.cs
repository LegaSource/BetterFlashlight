using HarmonyLib;
using UnityEngine;

namespace BetterFlashlight.Patches
{
    internal class RoundManagerPatch
    {
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.DetectElevatorIsRunning))]
        [HarmonyPostfix]
        private static void EndGame()
        {
            FlashlightItemPatch.blindableEnemies.Clear();
        }
    }
}
