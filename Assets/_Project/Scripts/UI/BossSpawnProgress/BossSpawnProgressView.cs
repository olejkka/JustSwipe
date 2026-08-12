using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.BossSpawnProgress
{
    public class BossSpawnProgressView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _killedCount;
        [SerializeField] private TMP_Text _threshold;
        [SerializeField] private Image _fillImage;

        public void SetProgress(int killed, int threshold)
        {
            _killedCount.text = killed.ToString();
            _threshold.text = threshold.ToString();
            _fillImage.fillAmount = threshold > 0 ? (float)killed / threshold : 0f;
        }
    }
}