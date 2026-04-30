using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.MoneyUI
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        
        public void UpdateAmount(int amount)
        {
            _text.text = amount.ToString();
        }
    }
}