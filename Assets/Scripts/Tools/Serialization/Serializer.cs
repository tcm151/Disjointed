using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Disjointed.Player;
using Disjointed.Combat.Enemies;
using Disjointed.Tools.ObjectCreation;


namespace Disjointed.Tools.Serialization
{
    [CreateAssetMenu(fileName = "Serializer", menuName = "Tools/Serializer")]
    public class Serializer : ScriptableObject
    {
        public string saveExtension = "save";
        
        public Factory factory;
        
        [Serializable] public class SaveData
        {
            public ThePlayer.Data playerData;
            public List<Enemy.Data> enemyData;
        }

        private static BinaryFormatter GetFormatter()
        {
            var formatter = new BinaryFormatter();
            var surrogateSelector = new SurrogateSelector();
            var v3Surrogate = new Vector3SerializationSurrogate();
            var v2Surrogate = new Vector2SerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), v3Surrogate);
            surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), v2Surrogate);
            formatter.SurrogateSelector = surrogateSelector;
            return formatter;
        }

        public void SaveGame(string saveName = "test")
        {
            var formatter = GetFormatter();
            var path = Path.Combine(Application.dataPath, $"Saves/{saveName}.{saveExtension}");
            var file = File.Create(path);

            var player = FindObjectOfType<ThePlayer>();
            var playerData = player.data;

            var enemies = FindObjectsOfType<Enemy>();
            var enemyData = enemies.Select(e => e.data).ToList();

            var save = new SaveData
            {
                playerData = playerData,
                enemyData = enemyData,
            };
            
            formatter.Serialize(file, save);
            file.Close();
        }

        public void LoadGame(string saveName = "test")
        {
            var oldPlayer = FindObjectOfType<ThePlayer>();
            var oldEnemies = FindObjectsOfType<Enemy>();
            Destroy(oldPlayer.gameObject);
            foreach (var enemy in oldEnemies) Destroy(enemy.gameObject);
            
            var formatter = GetFormatter();

            var path = Path.Combine(Application.dataPath, $"Saves/{saveName}.{saveExtension}");
            var file = File.Open(path, FileMode.Open);
            
            var load = (SaveData)formatter.Deserialize(file);

            //> SPAWN IN ENEMIES
            foreach (var enemyData in load.enemyData)
            {
                var enemy = Factory.Spawn(factory.enemies[(int)enemyData.type], enemyData.position);
                enemy.data = enemyData;
            }
            
            var player = Factory.Spawn(factory.player, load.playerData.position);
            player.data = load.playerData;
            
            var camera = FindObjectOfType<FollowCamera>();
            camera.SetTarget(player.transform);
        }
    }
}