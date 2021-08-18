using System;
using System.Collections;
using System.Collections.Generic;
using Disjointed.UI;
using UnityEngine;

namespace Disjointed
{
    public class InteractionPrompt : UI_Panel
    {
        public static Action ShowPrompt;
        public static Action HidePrompt;

        override protected void Awake()
        {
            base.Awake();

            ShowPrompt += Show;
            HidePrompt += Hide;
        }
    }
}
