using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Disjointed.Player;
using Disjointed.Environment;
using Disjointed.Combat.Enemies;
using Disjointed.Tools.ObjectCreation;
using UnityEditor;


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
            public List<Door.Data> doorData;
            public List<Enemy.Data> enemyData;
            public List<Crate.Data> crateData;
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
            var savePath = Path.Combine(Application.persistentDataPath, $"Saves/{saveName}.{saveExtension}");

            var saveFolder = Path.Combine(Application.persistentDataPath, "Saves");
            if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
            var file = File.Create(savePath);

            var player = FindObjectOfType<ThePlayer>();
            var playerData = player.data;

            var doors = FindObjectsOfType<Door>();
            var doorData = doors.Select(d => d.data).ToList();

            var enemies = FindObjectsOfType<Enemy>();
            var enemyData = enemies.Select(e => e.data).ToList();

            var crates = FindObjectsOfType<Crate>();
            var crateData = crates.Select(c => c.data).ToList();

            var save = new SaveData
            {
                playerData = playerData,
                doorData = doorData,
                enemyData = enemyData,
                crateData = crateData,
            };
            
            formatter.Serialize(file, save);
            file.Close();

            Debug.Log("Saved Game!");
        }

        public void LoadGame(string saveName = "test")
        {
            var oldPlayer = FindObjectOfType<ThePlayer>();
            Destroy(oldPlayer.gameObject);
            var oldEnemies = FindObjectsOfType<Enemy>();
            foreach (var enemy in oldEnemies) Destroy(enemy.gameObject);

            var formatter = GetFormatter();

            var path = Path.Combine(Application.persistentDataPath, $"Saves/{saveName}.{saveExtension}");
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

            var doors = FindObjectsOfType<Door>();
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].data = load.doorData[i];
            }

            var crates = FindObjectsOfType<Crate>();
            for (int i = 0; i < crates.Length; i++)
            {
                crates[i].data = load.crateData[i];
            }
        }
    }
}