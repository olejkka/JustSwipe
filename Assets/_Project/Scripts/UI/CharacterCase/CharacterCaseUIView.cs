using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.CharacterCase
{
    public class CharacterCaseUIView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _hp;
        [SerializeField] private TMP_Text _attack;
        
        public void SetIcon(Sprite sprite)
        {
            if (_icon != null)
                _icon.sprite = sprite;
        }
        
        public void SetHealth(int health)
        {
            if (_hp != null)
                _hp.text = health.ToString();
        }
        
        public void SetDamage(int damage)
        {
            if (_attack != null)
                _attack.text = damage.ToString();
        }
        
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}