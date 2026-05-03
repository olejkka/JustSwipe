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
        [SerializeField] private TMP_Text _countEnemiesKilled;
        [SerializeField] private TMP_Text _goldEarned;
        
        
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
        
        public void SetCountEnemiesKilled(string countEnemiesKilled)
        {
            _countEnemiesKilled.text = $"Enemies killed: {countEnemiesKilled}";
        }

        public void SetGoldEarned(string goldEarned)
        {
            _goldEarned.text = $"Gold earned: {goldEarned}";
        }
        
    }
}