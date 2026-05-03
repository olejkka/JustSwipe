using System;
using _Project.Scripts.Characters.Structs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.GameplayStatistic
{
    public class GameplayStatisticsService : IStartable, IDisposable
    {
        private readonly EventBus _eventBus;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        private int _turnsCount;
        private int _enemiesKilled;
        private int _goldEarned;
        
        
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

        public void Reset()
        {
            _enemiesKilled = 0;
            _goldEarned = 0;
            _turnsCount = 0;
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