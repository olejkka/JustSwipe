using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void UpdateAmount(int amount)
        {
            if (_text != null)
                _text.text = amount.ToString();
        }

        public void UpdateAmountFormatted(int amount)
        {
            if (_text != null)
                _text.text = $"${amount:N0}";
        }
    }
}