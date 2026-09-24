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
        private int _defaultEnemiesKilled;
        private int _hardEnemiesKilled;
        private int _bossEnemiesKilled;
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

        public void AddDefaultEnemyKill(int reward)
        {
            _defaultEnemiesKilled++;
            _goldEarned += reward;
        }

        public void AddHardEnemyKill(int reward)
        {
            _hardEnemiesKilled++;
            _goldEarned += reward;
        }

        public void AddBossEnemyKill(int reward)
        {
            _bossEnemiesKilled++;
            _goldEarned += reward;
        }

        public GameplayStatisticsSnapshot GetSnapshot()
        {
            return new GameplayStatisticsSnapshot(
                _defaultEnemiesKilled,
                _hardEnemiesKilled,
                _bossEnemiesKilled,
                _goldEarned,
                _turnsCount);
        }

        public void Reset()
        {
            _turnsCount = 0;
            _defaultEnemiesKilled = 0;
            _hardEnemiesKilled = 0;
            _bossEnemiesKilled = 0;
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
        public int DefaultEnemiesKilled { get; }
        public int HardEnemiesKilled { get; }
        public int BossEnemiesKilled { get; }
        public int GoldEarned { get; }
        public int TurnsCount { get; }


        public GameplayStatisticsSnapshot(
            int defaultEnemiesKilled,
            int hardEnemiesKilled,
            int bossEnemiesKilled,
            int goldEarned,
            int turnsCount)
        {
            DefaultEnemiesKilled = defaultEnemiesKilled;
            HardEnemiesKilled = hardEnemiesKilled;
            BossEnemiesKilled = bossEnemiesKilled;
            GoldEarned = goldEarned;
            TurnsCount = turnsCount;
        }
    }
}
