using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.CharacterUI
{
    public class CharacterHPView : MonoBehaviour
    {
        [SerializeField] private Image _hpImage;
        
        
        public void UpdateHealth(float healthPercentage)
        {
            _hpImage.fillAmount = healthPercentage;
        }
    }
}