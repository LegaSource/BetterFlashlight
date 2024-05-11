using HarmonyLib;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace BetterFlashlight.Patches
{
    internal class FlashlightItemPatch
    {
        private static bool isFlashing = false;
        private static float maxSpotAngle;

        [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.Start))]
        [HarmonyPostfix]
        private static void StartPatch(ref FlashlightItem __instance)
        {
            maxSpotAngle = __instance.flashlightBulb.spotAngle;
        }

        [HarmonyPatch(typeof(FlashlightItem), nameof(FlashlightItem.Update))]
        [HarmonyPostfix]
        private static void StunFlash(ref FlashlightItem __instance)
        {
            if (__instance.isBeingUsed && !isFlashing)
            {
                FlashlightItem flashlightItem = __instance;
                foreach (EnemyAI enemy in Object.FindObjectsOfType<EnemyAI>().ToList()
                    .Where(e => e.eye != null && MeetConditions(ref e, ref flashlightItem)))
                {
                    __instance.StartCoroutine(StunEnemyCoroutine(enemy, flashlightItem));
                }
            }
        }

        private static IEnumerator StunEnemyCoroutine(EnemyAI enemy, FlashlightItem flashlightItem)
        {
            float timePassed = 0f;
            float minSpotAngle = maxSpotAngle / 3f;
            while (MeetConditions(ref enemy, ref flashlightItem))
            {
                yield return new WaitForSeconds(0.1f);
                timePassed += 0.1f;
                flashlightItem.flashlightBulb.spotAngle = Mathf.Lerp(maxSpotAngle, minSpotAngle, timePassed / ConfigManager.flashTime.Value);

                if (timePassed >= ConfigManager.flashTime.Value) break;
            }

            if (isFlashing)
            {
                flashlightItem.flashlightBulb.spotAngle = maxSpotAngle * 2;
                enemy.SetEnemyStunned(setToStunned: true, ConfigManager.stunTime.Value, flashlightItem.playerHeldBy);
                yield return new WaitForSeconds(0.1f);
                isFlashing = false;
            }
            flashlightItem.flashlightBulb.spotAngle = maxSpotAngle;
        }

        private static bool MeetConditions(ref EnemyAI enemy, ref FlashlightItem flashlightItem)
        {
            if (Vector3.Distance(enemy.eye.position, flashlightItem.flashlightBulb.transform.position) <= 10f
                && Mathf.Abs(Vector3.Angle(flashlightItem.flashlightBulb.transform.forward, enemy.eye.position - flashlightItem.transform.position)) < ConfigManager.lightAngle.Value
                && Mathf.Abs(Vector3.Angle(enemy.eye.transform.forward, flashlightItem.transform.position - enemy.eye.position)) < ConfigManager.enemyAngle.Value)
            {
                isFlashing = true;
                return true;
            }
            isFlashing = false;
            return false;
        }
    }
}
