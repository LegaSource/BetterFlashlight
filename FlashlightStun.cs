namespace BetterFlashlight
{
    internal class FlashlightStun
    {
        public string EnemyName { get; private set; }
        public float FlashTime { get; private set; }
        public float StunTime { get; private set; }
        public float LightAngle { get; private set; }
        public float EnemyAngle { get; private set; }
        public float EnemyDistance { get; private set; }

        public FlashlightStun(string enemyName, float flashTime, float stunTime, float lightAngle, float enemyAngle, float enemyDistance)
        {
            this.EnemyName = enemyName;
            this.FlashTime = flashTime;
            this.StunTime = stunTime;
            this.LightAngle = lightAngle;
            this.EnemyAngle = enemyAngle;
            this.EnemyDistance = enemyDistance;
        }
    }
}
