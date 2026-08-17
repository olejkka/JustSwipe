using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Characters.Health
{
    public class HealthChangeService
    {
        private readonly List<HealthChangeRequest> _queue = new();
        private bool _isApplying;

        
        public void Enqueue(HealthChangeRequest request)
        {
            if (request.Target == null || request.Amount <= 0)
                return;

            if (request.Type != HealthChangeType.Damage && request.Type != HealthChangeType.Heal)
                return;

            _queue.Add(request);
        }

        public void Apply()
        {
            if (_isApplying)
                return;

            _isApplying = true;

            try
            {
                while (_queue.Count > 0)
                {
                    var batch = _queue.ToArray();
                    _queue.Clear();

                    for (var i = 0; i < batch.Length; i++)
                        ApplyRequest(batch[i]);
                }
            }
            finally
            {
                _isApplying = false;
            }
        }

        private static void ApplyRequest(HealthChangeRequest request)
        {
            var target = request.Target;

            if (target.Health <= 0)
                return;

            var amount = Mathf.RoundToInt(request.Amount * request.DamageMultiplier);

            if (amount <= 0)
                return;

            if (request.Type == HealthChangeType.Damage)
            {
                target.ChangeHealth(-amount, request.Source);

                if (request.Source.Type == HealthChangeSourceType.Character)
                    request.Source.Character.PerformMeleeAttack();

                return;
            }

            if (request.Type == HealthChangeType.Heal)
                target.ChangeHealth(amount, request.Source);
        }
    }
}
