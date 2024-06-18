using HarmonyLib;
using System;
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
            AddBlindableEnemy(ref __instance);
        }

        [HarmonyPatch(typeof(MaskedPlayerEnemy), nameof(MaskedPlayerEnemy.Start))]
        [HarmonyPostfix]
        private static void StartMaskedEnemy(ref MaskedPlayerEnemy __instance)
        {
            EnemyAI enemy = __instance;
            AddBlindableEnemy(ref enemy);
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

        private static void AddBlindableEnemy(ref EnemyAI enemy)
        {
            if (enemy.enemyType != null
                && enemy.enemyType.canBeStunned
                && enemy.eye != null
                && (string.IsNullOrEmpty(ConfigManager.exclusions.Value) || !ConfigManager.exclusions.Value.Contains(enemy.enemyType.enemyName)))
            {
                FlashlightItemPatch.blindableEnemies.Add(enemy);
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
