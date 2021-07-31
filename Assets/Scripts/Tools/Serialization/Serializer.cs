using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Disjointed.Combat.Enemies;
using Disjointed.Tools.ObjectCreation;
using UnityEngine;


namespace Disjointed.Tools.Serialization
{
    public class Serializer : MonoBehaviour
    {
        public Factory factory;
        
        [Serializable] public class SaveData
        {
            public List<Enemy.Data> enemies;
        }

        private void Awake()
        {
            StartCoroutine(CR_SaveGame());
        }

        public static BinaryFormatter GetFormatter()
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

        public IEnumerator CR_SaveGame()
        {
            yield return new WaitForSeconds(1f);
            
            var formatter = GetFormatter();
            
            var file = File.Create(Path.Combine(Application.dataPath, "test.save"));
            var save = new SaveData();
            var enemies = FindObjectsOfType<Enemy>();
            save.enemies = enemies.Select(e => e.data).ToList();

            formatter.Serialize(file, save);
            file.Close();

            foreach (var enemy in enemies)
            {
                Destroy(enemy.gameObject);
            }

            yield return new WaitForSeconds(5f);
            
            file = File.Open(Path.Combine(Application.dataPath, "test.save"), FileMode.Open);
            var load = (SaveData)formatter.Deserialize(file);

            foreach (var enemy in save.enemies)
            {
                Factory.Spawn(factory.enemyPrefabs[(int)enemy.type], enemy.position);
            }
            
            
        }
    }
}