using UnityEngine;

public class Event_Hands : MonoBehaviour
{
    [SerializeField] float zoneRadius;
    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] int spawnCount;

    [SerializeField] Transform handsContainer;
    [SerializeField] GameObject handsPrefab;
    [SerializeField] WatchEvent watchEvent;

    Vector3 minSpawnPosition;
    Vector3 maxSpawnPosition;
    Vector3 minSpawnRotation;
    Vector3 maxSpawnRotation;
    float zoneDiameter;


    void Awake()
    {
        zoneDiameter = zoneRadius * 2;

        minSpawnPosition = new Vector3(-zoneRadius, minHeight, -zoneRadius);
        maxSpawnPosition = new Vector3(zoneRadius, maxHeight, zoneRadius);

        minSpawnRotation = Vector3.zero;
        maxSpawnRotation = new Vector3(0, 360, 0);

        watchEvent.transform.localScale = new Vector3(zoneDiameter, 1, zoneDiameter);
        watchEvent.OnEventFailed += OnWatchEventFailed;
    }

    void SpawnHands()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var handsPosition = RandomUtils.Vector3(minSpawnPosition, maxSpawnPosition);
            var hansRotation = Quaternion.Euler(RandomUtils.Vector3(minSpawnRotation, maxSpawnRotation));
            Instantiate(handsPrefab, handsPosition, hansRotation, handsContainer);
        }
    }

    void OnWatchEventFailed()
    {
        SpawnHands();
    }
}
