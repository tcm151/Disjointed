using Disjointed.Player;
using TMPro;


namespace Disjointed.UI
{
    public class HealthPanel : UI_Panel
    {
        public TextMeshProUGUI health;
        
        override protected void Awake()
        {
            base.Awake();

            ThePlayer.healthChanged += SetHealth;
        }

        private void SetHealth(float value) => health.text = $"Health: {(int)value}";
    }
}