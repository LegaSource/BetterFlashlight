using HarmonyLib;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace BetterFlashlight.Patches
{
    internal class EnemyAIPatch
    {
        [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.Start))]
        [HarmonyPostfix]
        private static void StartEnemy(ref EnemyAI __instance)
        {
            if (__instance.enemyType != null
                && __instance.enemyType.canBeStunned
                && __instance.eye != null
                && (string.IsNullOrEmpty(ConfigManager.exclusions.Value) || !ConfigManager.exclusions.Value.Contains(__instance.enemyType.enemyName)))
            {
                FlashlightItemPatch.blindableEnemies.Add(__instance);
            }
        }

        [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemy))]
        [HarmonyPostfix]
        private static void EndEnemy(ref EnemyAI __instance)
        {
            FlashlightItemPatch.blindableEnemies.Remove(__instance);
        }

        [HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.SetEnemyStunned))]
        [HarmonyPostfix]
        private static void PostSetEnemyStunned(ref EnemyAI __instance)
        {
            if (!__instance.isEnemyDead && __instance.enemyType.canBeStunned)
            {
                float immunityTime = ConfigManager.enemyImmunityTime.Value;
                EnemyAI enemy = __instance;
                FlashlightStun flashlightStun = BetterFlashlight.flashlightStuns.Where(v => v.EnemyName.Equals(enemy.enemyType.enemyName)).FirstOrDefault();
                if (flashlightStun != null) immunityTime = flashlightStun.ImmunityTime;
                __instance.StartCoroutine(ImmuneCoroutine(__instance, immunityTime));
            }
        }

        private static IEnumerator ImmuneCoroutine(EnemyAI enemy, float immunityTime)
        {
            FlashlightItemPatch.blindableEnemies.Remove(enemy);
            yield return new WaitForSeconds(immunityTime);
            FlashlightItemPatch.blindableEnemies.Add(enemy);
        }
    }
}
