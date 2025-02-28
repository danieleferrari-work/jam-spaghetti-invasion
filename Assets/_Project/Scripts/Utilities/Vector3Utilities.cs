using UnityEngine;

public static class Vector3Utilities
{
   public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
   {
       return new Vector3(
           Mathf.Clamp(vector.x, min.x, max.x),
           Mathf.Clamp(vector.y, min.y, max.y),
           Mathf.Clamp(vector.z, min.z, max.z)
       );
   }

   public static Vector3 ToAngles180(this Vector3 vector)
   {
       return new Vector3(
           Mathf.DeltaAngle(0, vector.x),
           Mathf.DeltaAngle(0, vector.y),
           Mathf.DeltaAngle(0, vector.z)
       );
   }

   public static Vector3 ToAngle360(this Vector3 vector)
   {
       return new Vector3(
           vector.x < 0 ? vector.x + 360 : vector.x,
           vector.y < 0 ? vector.y + 360 : vector.y,
           vector.z < 0 ? vector.z + 360 : vector.z
       );
   }
}
