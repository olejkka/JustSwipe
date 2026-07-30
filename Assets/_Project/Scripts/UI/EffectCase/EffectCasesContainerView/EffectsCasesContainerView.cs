using UnityEngine;

namespace _Project.Scripts.UI.EffectCase.EffectCasesContainerView
{
    public class EffectsCasesContainerView : MonoBehaviour
    {
        [SerializeField] private EffectCaseUIView _casePrefab;
        [SerializeField] private Transform _playerContainer;
        [SerializeField] private Transform _botContainer;

        
        public EffectCaseUIView[] CreatePlayerCases(int count) =>
            CreateCases(count, _playerContainer);
        
        public EffectCaseUIView[] CreateBotCases(int count) =>
            CreateCases(count, _botContainer);
        
        private EffectCaseUIView[] CreateCases(int count, Transform container)
        {
            var cases = new EffectCaseUIView[count];
            
            for (var i = 0; i < count; i++)
                cases[i] = Instantiate(_casePrefab, container);
            
            return cases;
        }
    }
}