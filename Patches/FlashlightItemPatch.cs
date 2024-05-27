using GameNetcodeStuff;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterFlashlight.Patches
{
    internal class FlashlightItemPatch
    {
        internal static List<EnemyAI> blindableEnemies = new List<EnemyAI>();
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
                foreach (EnemyAI enemy in blindableEnemies.Where(e => e.enemyType != null && e.enemyType.canBeStunned && e.eye != null))
                {
                    float flashTime = ConfigManager.enemyFlashTime.Value;
                    float stunTime = ConfigManager.enemyStunTime.Value;
                    float lightAngle = ConfigManager.enemyLightAngle.Value;
                    float angle = ConfigManager.enemyAngle.Value;
                    float distance = ConfigManager.enemyDistance.Value;
                    float batteryConsumption = ConfigManager.enemyBatteryConsumption.Value;

                    FlashlightStun flashlightStun = BetterFlashlight.flashlightStuns.Where(v => v.EnemyName.Equals(enemy.enemyType.enemyName)).FirstOrDefault();
                    if (flashlightStun != null)
                    {
                        flashTime = flashlightStun.FlashTime;
                        stunTime = flashlightStun.StunTime;
                        lightAngle = flashlightStun.LightAngle;
                        angle = flashlightStun.EnemyAngle;
                        distance = flashlightStun.EnemyDistance;
                        batteryConsumption = flashlightStun.EnemyDistance;
                    }

                    if (MeetConditions(ref enemy.eye, ref flashlightItem, lightAngle, angle, distance))
                    {
                        __instance.StartCoroutine(StunCoroutine(enemy.eye, flashlightItem, enemy, flashTime, lightAngle, angle, distance, batteryConsumption, stunTime));
                        break;
                    }
                }

                if (ConfigManager.isPlayerBlind.Value && !isFlashing)
                {
                    foreach (PlayerControllerB player in StartOfRound.Instance.allPlayerScripts.Where(p => p.isPlayerControlled))
                    {
                        float flashTime = ConfigManager.playerFlashTime.Value;
                        float lightAngle = ConfigManager.playerLightAngle.Value;
                        float angle = ConfigManager.playerAngle.Value;
                        float distance = ConfigManager.playerDistance.Value;
                        float batteryConsumption = ConfigManager.playerBatteryConsumption.Value;

                        if (MeetConditions(ref player.playerEye, ref flashlightItem, lightAngle, angle, distance))
                        {
                            __instance.StartCoroutine(StunCoroutine(player.playerEye, flashlightItem, player, flashTime, lightAngle, angle, distance, batteryConsumption));
                            break;
                        }
                    }
                }
            }
        }

        private static IEnumerator StunCoroutine<T>(Transform eye, FlashlightItem flashlightItem, T entity, float flashTime, float lightAngle, float angle, float distance, float batteryConsumption, float stunTime = 0f)
        {
            float timePassed = 0f;
            float minSpotAngle = maxSpotAngle / 3f;

            while (MeetConditions(ref eye, ref flashlightItem, lightAngle, angle, distance))
            {
                yield return new WaitForSeconds(0.1f);
                timePassed += 0.1f;
                flashlightItem.flashlightBulb.spotAngle = Mathf.Lerp(maxSpotAngle, minSpotAngle, timePassed / flashTime);

                if (entity is PlayerControllerB player && GameNetworkManager.Instance.localPlayerController == player && HUDManager.Instance.flashFilter < timePassed)
                {
                    HUDManager.Instance.flashFilter = timePassed;
                }

                if (timePassed >= flashTime) break;
            }

            if (isFlashing)
            {
                flashlightItem.flashlightBulb.spotAngle = maxSpotAngle * 2;
                if (entity is EnemyAI enemy) enemy.SetEnemyStunned(setToStunned: true, stunTime, flashlightItem.playerHeldBy);
                if (batteryConsumption > 0f) flashlightItem.insertedBattery.charge = flashlightItem.insertedBattery.charge * batteryConsumption / 100;
                yield return new WaitForSeconds(0.1f);
                isFlashing = false;
            }
            flashlightItem.flashlightBulb.spotAngle = maxSpotAngle;
        }

        private static bool MeetConditions(ref Transform eye, ref FlashlightItem flashlightItem, float lightAngle, float angle, float distance)
        {
            if (flashlightItem.isBeingUsed
                && Vector3.Distance(eye.position, flashlightItem.flashlightBulb.transform.position) <= distance
                && Mathf.Abs(Vector3.Angle(flashlightItem.flashlightBulb.transform.forward, eye.position - flashlightItem.transform.position)) < lightAngle
                && Mathf.Abs(Vector3.Angle(eye.transform.forward, flashlightItem.transform.position - eye.position)) < angle)
            {
                isFlashing = true;
                return true;
            }
            flashlightItem.flashlightBulb.spotAngle = maxSpotAngle;
            isFlashing = false;
            return false;
        }
    }
}
