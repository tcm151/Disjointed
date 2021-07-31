using UnityEngine;
using UnityEngine.UI;


namespace Disjointed.UI
{
    public class OptionsPanel : UI_Panel
    {
        public Slider volumeSlider;

        override protected void Awake()
        {
            base.Awake();

            volumeSlider.value = PlayerPrefs.GetFloat("GlobalVolume", 1);
        }
    }
}