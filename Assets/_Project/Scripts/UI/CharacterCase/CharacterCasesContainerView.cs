using UnityEngine;

namespace _Project.Scripts.UI.CharacterCase
{
    public class CharacterCasesContainerView : MonoBehaviour
    {
        [SerializeField] private CharacterCaseUIView _casePrefab;
        [SerializeField] private Transform _playerContainer;
        [SerializeField] private Transform _botContainer;

        
        public CharacterCaseUIView[] CreatePlayerCases(int count) =>
            CreateCases(count, _playerContainer);
        
        public CharacterCaseUIView[] CreateBotCases(int count) =>
            CreateCases(count, _botContainer);
        
        private CharacterCaseUIView[] CreateCases(int count, Transform container)
        {
            var cases = new CharacterCaseUIView[count];

            for (var i = 0; i < count; i++)
                cases[i] = Instantiate(_casePrefab, container);

            return cases;
        }
    }
}