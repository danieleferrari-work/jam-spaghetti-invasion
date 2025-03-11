using UnityEngine;

public static class RandomUtils
{
    public static bool FlipACoin() => Random.Range(0, 2) == 0;

    public static Vector3 Vector3(Vector3 min, Vector3 max)
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        float z = Random.Range(min.z, max.z);

        return new Vector3(x, y, z);
    }
}
