using System.Collections;
using UnityEngine;

public class Event_Hands : MonoBehaviour
{
    [SerializeField] float zoneRadius; // TODO mettere in Params
    [SerializeField] float minHeight; // TODO mettere in Params
    [SerializeField] float maxHeight; // TODO mettere in Params
    [SerializeField] int spawnCount; // TODO mettere in params
    [SerializeField] int spawnDelay; // TODO mettere in params

    [SerializeField] Transform followPlayer;
    [SerializeField] GameObject handsPrefab;

    Vector3 minSpawnPosition;
    Vector3 maxSpawnPosition;
    Vector3 minSpawnRotation;
    Vector3 maxSpawnRotation;


    void Awake()
    {
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
        return Physics.Raycast(raycastStartPoint, Vector3.down, 200, mask);
    }
}
