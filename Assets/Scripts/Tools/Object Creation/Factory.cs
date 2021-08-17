using UnityEngine;
using Disjointed.Player;
using Disjointed.Combat.Enemies;
using Disjointed.Environment;
using UnityEngine.Serialization;


namespace Disjointed.Tools.ObjectCreation
{
    [CreateAssetMenu(fileName = "Factory", menuName = "Tools/Factory")]
    public class Factory : ScriptableObject
    {
        public ThePlayer player;
        public Enemy[] enemies;
        public Crate crate;
        
        
        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : MonoBehaviour
            => Instantiate(prefab, position, rotation);

        public static T Spawn<T>(T prefab, Vector3 position) where T : MonoBehaviour
            => Spawn(prefab, position, Quaternion.identity);
        
        public static T Spawn<T>(T prefab) where T : MonoBehaviour
            => Spawn(prefab, Vector3.zero, Quaternion.identity);
    }
}