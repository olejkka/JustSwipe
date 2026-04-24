using UnityEngine;
using VContainer;

namespace _Project.Scripts.Configs
{
    [CreateAssetMenu(
        menuName = "Gameplay Configs/Gameplay Configs",
        fileName = "Gameplay Configs"
    )]
    public class GameplayConfigs : ScriptableObject
    {
        [SerializeField] private TilesGenerationConfig _tilesGeneration;
        [SerializeField] private CharactersConfig _characters;
        [SerializeField] private InitialGameplayConfig _initialGameplay;
        [SerializeField] private GameplayEconomyConfig _gameplayEconomy;
        [SerializeField] private AudioConfig _audioConfig;
        [SerializeField] private CharacterSpawnChancesConfig _characterSpawnChances;

        
        public void RegisterAll(IContainerBuilder builder)
        {
            builder.RegisterInstance(_tilesGeneration);
            builder.RegisterInstance(_characters);
            builder.RegisterInstance(_initialGameplay);
            builder.RegisterInstance(_gameplayEconomy);
            builder.RegisterInstance(_audioConfig);
            builder.RegisterInstance(_characterSpawnChances);
        }
    }
}