using System;

/// <summary>
/// Represents performance metrics collected during a player's training session,
/// including scoring, accuracy, movement, timing, and swing-type distribution.
/// </summary>
namespace VRTraining
{
    public class GameMetrics
    {
        public string playerId;
        public int score;
        public int hits;
        public int misses;
        public float accuracy;
        public float sessionTime;
        public float speed;
        public float distance;
        public float spin;
    
        // NEW — swing-type metrics
        public int forehands;
        public int backhands;
        public int smashes;
        public int slices;
    
        public string timestamp;
    
        /// <summary>
        /// Initializes a new instance of the GameMetrics class with full session data.
        /// </summary>
        public GameMetrics(
            string playerId,
            int score,
            int hits,
            int misses,
            float sessionTime,
            float speed = 0f,
            float distance = 0f,
            float spin = 0f,
            int forehands = 0,
            int backhands = 0,
            int smashes = 0,
            int slices = 0
        )
        {
            this.playerId = playerId;
            this.score = score;
            this.hits = hits;
            this.misses = misses;
            this.sessionTime = sessionTime;
            this.speed = speed;
            this.distance = distance;
            this.spin = spin;
    
            // Assign swing-type metrics
            this.forehands = forehands;
            this.backhands = backhands;
            this.smashes = smashes;
            this.slices = slices;
    
            // Derived metrics
            this.accuracy = (hits + misses) > 0 ? (float)hits / (hits + misses) : 0f;
            this.timestamp = DateTime.UtcNow.ToString("o");
        }
    }
}
