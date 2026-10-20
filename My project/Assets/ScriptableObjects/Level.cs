using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    [Range(1, 24)]
    [Tooltip("Number of paintings")]
    public int totalNumberOfPaintings;

    [Range(1, 12)]
    [Tooltip("Number of AI paintings")]
    public int numberOfAIPaintings;

    [Range(1, 600)]
    [Tooltip("Time in seconds")]
    public int time;

    [Tooltip("Spawn positions")]
    public Transform[] spawnPositions;

}
