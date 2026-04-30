using System;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.PlayButton
{
    public class PlayButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;

        
        public void Initialize(Lifetime lifetime, Action playClicked)
        {
            lifetime.BracketButton(_button, () => playClicked?.Invoke());
        }
    }
}