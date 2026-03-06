using System;
//[Serializable]
public class GameMetrics
{
    public string playerId;
    public int score;           // rename from B
    public int hits;
    public int misses;
    public float accuracy;
    public float sessionTime;
    public float speed;         // latest speed (f/s) or average speed
    public float distance;      // distance travelled
    public float spin;          // angular velocity magnitude
    public string timestamp;

    public GameMetrics(string playerId, int score, int hits, int misses, float sessionTime,
                       float speed = 0f, float distance = 0f, float spin = 0f)
    {
        this.playerId = playerId;
        this.score = score;
        this.hits = hits;
        this.misses = misses;
        this.sessionTime = sessionTime;
        this.speed = speed;
        this.distance = distance;
        this.spin = spin;
        this.accuracy = (hits + misses) > 0 ? (float)hits / (hits + misses) : 0f;
        this.timestamp = System.DateTime.UtcNow.ToString("o");
    }
}