using System;
using JetBrains.Lifetimes;
using VContainer.Unity;

namespace _Project.Scripts.UI.CheatsPanel
{
    public class CheatsPanelPresenter : IStartable, IDisposable
    {
        private readonly CheatsPanelView _view;
        private readonly LifetimeDefinition _lifetimeDefinition = new();

        public CheatsPanelPresenter(CheatsPanelView view)
        {
            _view = view;
        }

        public void Start() =>
            _view.Initialize(_lifetimeDefinition.Lifetime, OnButtonClicked);

        public void Dispose() =>
            _lifetimeDefinition.Terminate();

        private void OnButtonClicked() =>
            _view.Toggle();
    }
}