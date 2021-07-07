using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace OGAM.Tools
{
    [System.Serializable]
    public class PlayerData
    {
        public Scene level;
        public float[] position;

        public PlayerData(GameObject player)
        {
            position = new float[2];
            position[0] = player.transform.position.x;
            position[1] = player.transform.position.y;
            level = SceneManager.GetActiveScene();
        }
    }
}
