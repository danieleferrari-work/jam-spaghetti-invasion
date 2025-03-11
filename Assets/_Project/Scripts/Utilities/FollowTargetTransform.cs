using UnityEngine;

public class FollowTargetTransform : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] bool lockX;
    [SerializeField] bool lockY;
    [SerializeField] bool lockZ;

    void Update()
    {
        var targetPosition = target.transform.position;
        var currentPosition = transform.position;

        var x = lockX ? currentPosition.x : targetPosition.x;
        var y = lockY ? currentPosition.y : targetPosition.y;
        var z = lockZ ? currentPosition.z : targetPosition.z;

        transform.position = new Vector3(x, y, z);
    }
}
