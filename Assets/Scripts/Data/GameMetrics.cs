using System;

/// <summary>
/// Represents performance metrics collected during a player's training session,
/// including scoring, accuracy, movement, and timing data.
/// </summary>
public class GameMetrics
{
    /// <summary>
    /// Unique identifier for the player.
    /// </summary>
    public string playerId;

    /// <summary>
    /// The player's score for the session.
    /// </summary>
    public int score;

    /// <summary>
    /// The number of successful hits recorded.
    /// </summary>
    public int hits;

    /// <summary>
    /// The number of missed attempts.
    /// </summary>
    public int misses;

    /// <summary>
    /// The player's accuracy, calculated as hits divided by total attempts.
    /// </summary>
    public float accuracy;

    /// <summary>
    /// Total duration of the session in seconds.
    /// </summary>
    public float sessionTime;

    /// <summary>
    /// The player's speed, either the most recent or average value (units per second).
    /// </summary>
    public float speed;

    /// <summary>
    /// Total distance traveled during the session.
    /// </summary>
    public float distance;

    /// <summary>
    /// The magnitude of angular velocity (spin) during movement.
    /// </summary>
    public float spin;

    /// <summary>
    /// Timestamp of when the metrics were recorded, in UTC ISO 8601 format.
    /// </summary>
    public string timestamp;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameMetrics"/> class
    /// with player performance data and automatically calculates accuracy
    /// and timestamp.
    /// </summary>
    /// <param name="playerId">Unique identifier for the player.</param>
    /// <param name="score">The player's score.</param>
    /// <param name="hits">Number of successful hits.</param>
    /// <param name="misses">Number of missed attempts.</param>
    /// <param name="sessionTime">Total session duration in seconds.</param>
    /// <param name="speed">Optional speed value (default is 0).</param>
    /// <param name="distance">Optional distance traveled (default is 0).</param>
    /// <param name="spin">Optional angular velocity magnitude (default is 0).</param>
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

        /// <summary>
        /// Accuracy is computed as hits divided by total attempts.
        /// Returns 0 if there are no attempts.
        /// </summary>
        this.accuracy = (hits + misses) > 0 ? (float)hits / (hits + misses) : 0f;

        /// <summary>
        /// Timestamp is generated at object creation in UTC using ISO 8601 format.
        /// </summary>
        this.timestamp = System.DateTime.UtcNow.ToString("o");
    }
}