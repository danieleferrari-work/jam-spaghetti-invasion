using System.Collections;
using UnityEngine;

public class Event_Hands : MonoBehaviour
{
    [SerializeField] Transform followPlayer;
    [SerializeField] GameObject handsPrefab;

    private float zoneRadius;
    private float minHeight;
    private float maxHeight;
    private int spawnCount;
    private int spawnDelay;

    Vector3 minSpawnPosition;
    Vector3 maxSpawnPosition;
    Vector3 minSpawnRotation;
    Vector3 maxSpawnRotation;


    void Awake()
    {
        zoneRadius = Params.instance.handsEventRadius;
        minHeight = Params.instance.handsEventMinHeight;
        maxHeight = Params.instance.handsEventMaxHeight;
        spawnCount = Params.instance.handsEventSpawnCount;
        spawnDelay = Params.instance.handsEventSpawnDelay;

        minSpawnPosition = new Vector3(-zoneRadius, minHeight, -zoneRadius);
        maxSpawnPosition = new Vector3(zoneRadius, maxHeight, zoneRadius);

        minSpawnRotation = Vector3.zero;
        maxSpawnRotation = new Vector3(0, 360, 0);

        StartCoroutine(SpawnHands());
    }

    IEnumerator SpawnHands()
    {
        yield return new WaitForSeconds(spawnDelay);

        var handsToSpawn = spawnCount;
        while (handsToSpawn > 0)
        {
            var handsPosition = RandomUtils.Vector3(followPlayer.position + minSpawnPosition, followPlayer.position + maxSpawnPosition);

            if (!IsValidPosition(handsPosition))
                continue;

            var handsRotation = Quaternion.Euler(RandomUtils.Vector3(minSpawnRotation, maxSpawnRotation));
            Instantiate(handsPrefab, handsPosition, handsRotation);

            handsToSpawn--;
        }
    }

    bool IsValidPosition(Vector3 position)
    {
        var raycastStartPoint = position + Vector3.up * 100;
        LayerMask mask = LayerMask.GetMask("Water");
        return Physics.Raycast(raycastStartPoint, Vector3.down, 500, mask);
    }
}