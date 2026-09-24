using System;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.GameplayStatistic
{
    public class GameplayStatisticView : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _turnsCount;
        [SerializeField] private TMP_Text _goldEarned;
        [SerializeField] private TMP_Text _defaultEnemiesKilled;
        [SerializeField] private TMP_Text _hardEnemiesKilled;
        [SerializeField] private TMP_Text _bossEnemiesKilled;


        public void Initialize(Lifetime lifetime, Action applicationQuitClicked)
        {
            lifetime.BracketButton(_button, () => applicationQuitClicked?.Invoke());
            
            _container.gameObject.SetActive(false);
        }

        public void SetContainerActive(bool isActive)
        {
            _container.gameObject.SetActive(isActive);
        }
        
        public void SetTurnsCount(string turnsCount)
        {
            _turnsCount.text = $"Turns count: {turnsCount}";
        }
        
        public void SetDefaultEnemiesKilled(string count)
        {
            _defaultEnemiesKilled.text = $"Default enemies killed: {count}";
        }

        public void SetHardEnemiesKilled(string count)
        {
            _hardEnemiesKilled.text = $"Hard enemies killed: {count}";
        }

        public void SetBossEnemiesKilled(string count)
        {
            _bossEnemiesKilled.text = $"Boss enemies killed: {count}";
        }

        public void SetGoldEarned(string goldEarned)
        {
            _goldEarned.text = $"Gold earned: {goldEarned}";
        }
        
    }
}