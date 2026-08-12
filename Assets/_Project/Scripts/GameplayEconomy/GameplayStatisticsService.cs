using System;
using _Project.Scripts.Characters;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.GameplayEconomy
{
    public class GameplayStatisticsService : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        private int _turnsCount;
        private int _enemiesKilledUntilBoss;
        private int _enemiesKilled;
        private int _goldEarned;
        
        public int EnemiesKilledUntilBoss => _enemiesKilledUntilBoss;
        
        
        public GameplayStatisticsService(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<TurnEndedEvent>(_lifetimeDefinition.Lifetime, OnTurnEnded);
        }

        public void Dispose()
        {
            _lifetimeDefinition.Terminate();
        }

        public void AddEnemyKillReward(int reward)
        {
            _enemiesKilledUntilBoss++;
            _enemiesKilled++;
            _goldEarned += reward;
        }

        public GameplayStatisticsSnapshot GetSnapshot()
        {
            return new GameplayStatisticsSnapshot(
                _enemiesKilled,
                _goldEarned,
                _turnsCount);
        }
        
        public void ResetEnemiesKilledUntilBoss()
        {
            _enemiesKilledUntilBoss = 0;
        }

        public void Reset()
        {
            _turnsCount = 0;
            _enemiesKilledUntilBoss = 0;
            _enemiesKilled = 0;
            _goldEarned = 0;
        }

        private void OnTurnEnded(TurnEndedEvent e)
        {
            if (e.Team != Team.Player)
                return;

            _turnsCount++;
        }
    }
    
    public readonly struct GameplayStatisticsSnapshot
    {
        public int EnemiesKilled { get; }
        public int GoldEarned { get; }
        public int TurnsCount { get; }
        
        
        public GameplayStatisticsSnapshot(int enemiesKilled, int goldEarned, int turnsCount)
        {
            EnemiesKilled = enemiesKilled;
            GoldEarned = goldEarned;
            TurnsCount = turnsCount;
        }
    }
}