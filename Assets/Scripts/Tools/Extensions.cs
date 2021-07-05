using UnityEngine;


namespace OGAM.Tools
{
    public static class Extensions
    {
        //> LAYER MAKS
        public static bool Contains(this LayerMask layerMask, int layer) => (layerMask == (layerMask | (1 << layer)));
        
        
    }
}