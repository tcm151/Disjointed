using System;
using System.Collections;
using System.Collections.Generic;
using Disjointed.UI;
using UnityEngine;

namespace Disjointed
{
    public class InteractionPrompt : UI_Panel
    {
        
        public static Action onShowPrompt;
        public static Action onHidePrompt;

        override protected void Awake()
        {
            base.Awake();

            onShowPrompt += ShowPrompt;
            onHidePrompt += HidePrompt;
        }

        private void ShowPrompt()
        {
            Show();
        }

        private void HidePrompt()
        {
            Hide();
        }
    }
}
