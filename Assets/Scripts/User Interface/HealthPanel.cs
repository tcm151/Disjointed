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

            PlayerData.healthChanged += SetHealth;
        }

        private void SetHealth(int value) => health.text = $"Health: {value}";
    }
}