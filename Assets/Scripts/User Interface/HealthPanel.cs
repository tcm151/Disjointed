using Disjointed.ThePlayer;
using TMPro;


namespace Disjointed.UI
{
    public class HealthPanel : UI_Panel
    {
        public TextMeshProUGUI health;
        
        override protected void Awake()
        {
            base.Awake();

            Player.healthChanged += SetHealth;
        }

        private void SetHealth(int value) => health.text = $"Health: {value}";
    }
}