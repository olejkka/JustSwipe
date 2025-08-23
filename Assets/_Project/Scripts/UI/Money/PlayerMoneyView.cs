using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Money
{
    public class PlayerMoneyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;
        
        public void UpdateMoneyDisplay(int money) =>
            _moneyText.text = money.ToString();
    }
}