using UnityEngine;

namespace _Project.Scripts.UI.CharacterCase
{
    public class CharacterCasesContainerView : MonoBehaviour
    {
        [SerializeField] private CharacterCaseUIView _casePrefab;
        [SerializeField] private Transform _container;

        public CharacterCaseUIView[] CreateCases(int count)
        {
            var cases = new CharacterCaseUIView[count];

            for (int i = 0; i < count; i++)
                cases[i] = Instantiate(_casePrefab, _container);

            return cases;
        }
    }
}