using UnityEngine;

namespace _Project.Scripts.UI.EffectCase.EffectCasesContainerView
{
    public class EffectsCasesContainerView : MonoBehaviour
    {
        [SerializeField] private EffectCaseUIView _casePrefab;
        [SerializeField] private Transform _container;

        
        public EffectCaseUIView[] CreateCases(int count)
        {
            var cases = new EffectCaseUIView[count];

            for (var i = 0; i < count; i++)
                cases[i] = Instantiate(_casePrefab, _container);

            return cases;
        }
    }
}