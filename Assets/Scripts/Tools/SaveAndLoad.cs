using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OGAM.Tools
{
    public class SaveAndLoad : MonoBehaviour
    {

        public void SavePlayer()
        {
            SaveSystem.SavePlayer(this.gameObject);
        }

        public void LoadPlayer()
        {
            PlayerData playerData = SaveSystem.LoadPlayer();

            Vector3 position;
            position.x = playerData.position[0];
            position.y = playerData.position[1];
            position.z = 0f;
            transform.position = position;
        }
    }
}
