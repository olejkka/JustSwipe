using System;
using JetBrains.Lifetimes;
using UnityEngine;
using VContainer.Unity;

namespace _Project.Scripts.UI.CheatsPanel.SetTimescale
{
    public class SetTimescalePanelPresenter : IStartable, IDisposable
    {
        private readonly SetTimescalePanelView _view;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        
        public SetTimescalePanelPresenter(SetTimescalePanelView view)
        {
            _view = view;
        }

        public void Start() =>
            _view.Initialize(
                _lifetimeDefinition.Lifetime,
                () => SetTimescale(0.25f),
                () => SetTimescale(0.5f),
                () => SetTimescale(1f),
                () => SetTimescale(1.5f),
                () => SetTimescale(2f));

        public void Dispose() =>
            _lifetimeDefinition.Terminate();

        private void SetTimescale(float timescale) =>
            Time.timeScale = timescale;
    }
}
