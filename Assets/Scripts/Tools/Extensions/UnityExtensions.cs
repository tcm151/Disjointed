using UnityEngine;


namespace Disjointed.Tools.Extensions
{
    public static class UnityExtensions
    {
        //> LAYER MAKS
        public static bool Contains(this LayerMask layerMask, int layer) => (layerMask == (layerMask | (1 << layer)));
        
        //> VECTOR2
        public static float Angle(this Vector2 direction)
        {
            direction.Normalize();

            var angle = Mathf.Acos(direction.x) * Mathf.Rad2Deg;
            return (direction.y > 0f) ? angle : -angle;
        }

        public static void MoveTowards(this ref Vector2 current, Vector2 target, float maxDelta)
            => current = Vector2.MoveTowards(current, target, maxDelta);

        //> VECTOR3
        public static float Angle(this Vector3 direction)
        {
            direction.Normalize();

            var angle = Mathf.Acos(direction.x) * Mathf.Rad2Deg;
            return (direction.y > 0f) ? angle : -angle;
        }
        
        //> MATHF
        public static float Clamp(this ref float value, float min, float max)
            => value = Mathf.Clamp(value, min, max);

        public static float MoveTowards(this ref float value, float target, float maxDelta)
            => value = Mathf.MoveTowards(value, target, maxDelta);



    }
}