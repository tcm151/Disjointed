using System;
using System.Collections;
using System.Collections.Generic;
using OGAM.Tools;
using UnityEngine;

namespace OGAM.Environment
{
    public class DeathZone : MonoBehaviour
    {
        public LayerMask playerMask;
        public SceneSwitch Switcher;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!playerMask.Contains(collider.gameObject.layer)) return;

            //Debug.Log("<color=red>PLAYER DEATH!</color>");
            //collider.transform.position = new Vector3(0, 1, 0);
            Switcher.RestartScene();
        }
    }
}
