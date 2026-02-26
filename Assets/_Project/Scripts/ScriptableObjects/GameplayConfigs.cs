using UnityEngine;
using VContainer;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(
        menuName = "Gameplay/GameplayConfigs",
        fileName = "GameplayConfigs"
    )]
    public class GameplayConfigs : ScriptableObject
    {
        [SerializeField] private TilesGenerationConfig _tilesGeneration;
        [SerializeField] private CharactersConfig _characters;
        [SerializeField] private InitialGameplayConfig _initialGameplay;
        [SerializeField] private GameplayEconomyConfig _gameplayEconomy;

        public void RegisterAll(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tilesGeneration);
            builder.RegisterInstance(_characters);
            builder.RegisterInstance(_initialGameplay);
            builder.RegisterInstance(_gameplayEconomy);
        }
    }
}