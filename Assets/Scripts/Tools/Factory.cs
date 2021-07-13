using UnityEngine;


namespace OGAM.Tools
{
    public class Factory : ScriptableObject
    {
        public static T Spawn<T>(T prefab, Vector3 position) where T : MonoBehaviour
            => Instantiate(prefab, position, Quaternion.identity);
    }
}