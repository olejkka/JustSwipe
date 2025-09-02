using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.UI.CharacterUI;
using UnityEngine;
using VContainer;

namespace _Project.Scripts.UI.CharacterHP
{
    public class CharacterHPViewInstantiator : MonoBehaviour
    {
        [SerializeField] private CharacterHPView _hpViewPrefab;
        
        [Inject] private CharactersViewsStorage _charactersViewsStorage;
        
        private readonly System.Collections.Generic.Dictionary<Character, CharacterHPPresenter> _hpPresenters = new();

        
        public void CreateHPView(Character character, CharacterView characterView)
        {
            
        }

        private void OnDestroy()
        {
            foreach (var presenter in _hpPresenters.Values)
                presenter.Dispose();
            
            _hpPresenters.Clear();
        }
    }
}