using UnityEngine;


namespace OGAM.Tools
{
    public static class Extensions
    {
        //> LAYER MAKS
        public static bool Contains(this LayerMask layerMask, int layer) => (layerMask == (layerMask | (1 << layer)));
        
        // //> VECTOR2
        public static float Angle(this Vector2 direction)
        {
            direction.Normalize();

            var angle = Mathf.Acos(direction.x) * Mathf.Rad2Deg;
            return (direction.y > 0f) ? angle : -angle;
        }
        
        
        
    }
}