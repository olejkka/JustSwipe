using System;
using System.Collections.Generic;
using _Project.Scripts.Characters;
using _Project.Scripts.Characters.Storages;
using _Project.Scripts.Configs;
using _Project.Scripts.Infrastructure.EventBus;
using _Project.Scripts.Infrastructure.EventBus.Events;
using _Project.Scripts.Infrastructure.LifetimesExtensions;
using _Project.Scripts.Instantiators;
using DG.Tweening;
using JetBrains.Lifetimes;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer.Unity;

namespace _Project.Scripts.Board
{
    public class TileHighlightService : IStartable, IDisposable
    {
        private static readonly Vector2Int[] Directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        private readonly EventBus _eventBus;
        private readonly TilesStorage _tilesStorage;
        private readonly CharactersStorage _charactersStorage;
        private readonly TileHighlightConfig _config;
        private readonly Tilemap _tilemap;
        private readonly LifetimeDefinition _lifetimeDefinition = new();
        private readonly Dictionary<int, Session> _sessions = new();
        private readonly Dictionary<Vector2Int, TileBase> _originalTiles = new();

        private int _nextOrder;


        public TileHighlightService(
            EventBus eventBus,
            TilesStorage tilesStorage,
            CharactersStorage charactersStorage,
            TileHighlightConfig config,
            TileInstantiator tileInstantiator)
        {
            _eventBus = eventBus;
            _tilesStorage = tilesStorage;
            _charactersStorage = charactersStorage;
            _config = config;
            _tilemap = tileInstantiator.Tilemap;
        }

        public void Start()
        {
            _eventBus.SubscribeWithLifetime<CharacterAttackHighlightRequestedEvent>(
                _lifetimeDefinition.Lifetime,
                OnHighlightRequested);
            _eventBus.SubscribeWithLifetime<SwipeEvent>(
                _lifetimeDefinition.Lifetime,
                _ => StopAll());
            _eventBus.SubscribeWithLifetime<BotMoveCompletedEvent>(
                _lifetimeDefinition.Lifetime,
                _ => StopAll());
            _eventBus.SubscribeWithLifetime<CharacterDiedEvent>(
                _lifetimeDefinition.Lifetime,
                OnCharacterDied);
        }

        public void Dispose()
        {
            StopAll();
            _lifetimeDefinition.Terminate();
        }

        private void OnHighlightRequested(CharacterAttackHighlightRequestedEvent e) =>
            StartSession(e.Character);

        private void OnCharacterDied(CharacterDiedEvent e) =>
            StopSession(e.Character.InstanceId);

        private void StartSession(Character character)
        {
            StopSession(character.InstanceId);

            var type = character.CanAttack
                ? TileHighlightType.CharacterAttack
                : TileHighlightType.CharacterAttackCooldown;
            var entry = _config.Get(type);
            var tiles = CollectTiles(character);

            if (tiles.Count == 0)
                return;

            var session = new Session
            {
                Tiles = tiles,
                TargetColor = entry.Color,
                Order = _nextOrder++
            };

            _sessions[character.InstanceId] = session;

            for (var i = 0; i < tiles.Count; i++)
                EnsureOverlay(tiles[i]);

            ApplySession(session);

            var instanceId = character.InstanceId;

            session.Tween = DOTween.Sequence()
                .AppendInterval(entry.Duration)
                .SetUpdate(UpdateType.Normal, isIndependentUpdate: false)
                .OnComplete(() => CompleteSession(instanceId));
        }

        private void CompleteSession(int instanceId)
        {
            if (!_sessions.TryGetValue(instanceId, out var session))
                return;

            _sessions.Remove(instanceId);
            session.Tween = null;
            RefreshTiles(session.Tiles);
        }

        private void StopSession(int instanceId)
        {
            if (!_sessions.TryGetValue(instanceId, out var session))
                return;

            _sessions.Remove(instanceId);
            session.Tween?.Kill();
            session.Tween = null;
            RefreshTiles(session.Tiles);
        }

        private void StopAll()
        {
            var sessions = new List<Session>(_sessions.Values);
            _sessions.Clear();

            for (var i = 0; i < sessions.Count; i++)
            {
                sessions[i].Tween?.Kill();
                sessions[i].Tween = null;
                RestoreTiles(sessions[i].Tiles);
            }
        }

        private void ApplySession(Session session)
        {
            for (var i = 0; i < session.Tiles.Count; i++)
            {
                var pos = session.Tiles[i];

                if (GetOwner(pos) != session)
                    continue;

                SetOverlayColor(pos, session.TargetColor);
            }
        }

        private void RefreshTiles(List<Vector2Int> tiles)
        {
            for (var i = 0; i < tiles.Count; i++)
            {
                var pos = tiles[i];
                var owner = GetOwner(pos);

                if (owner == null)
                {
                    RestoreOriginal(pos);
                    continue;
                }

                EnsureOverlay(pos);
                SetOverlayColor(pos, owner.TargetColor);
            }
        }

        private void RestoreTiles(List<Vector2Int> tiles)
        {
            for (var i = 0; i < tiles.Count; i++)
                RestoreOriginal(tiles[i]);
        }

        private Session GetOwner(Vector2Int pos)
        {
            Session owner = null;

            foreach (var session in _sessions.Values)
            {
                if (!ContainsTile(session.Tiles, pos))
                    continue;

                if (owner == null || session.Order > owner.Order)
                    owner = session;
            }

            return owner;
        }

        private List<Vector2Int> CollectTiles(Character character)
        {
            var tiles = new List<Vector2Int>();
            var range = character.AttackRange == 0 ? 1 : character.AttackRange;
            var occupants = new Dictionary<Vector2Int, Character>();

            foreach (var occupant in _charactersStorage.GetAllCharacters())
                occupants[occupant.Position] = occupant;

            for (var i = 0; i < Directions.Length; i++)
            {
                var direction = Directions[i];

                for (var step = 1; step <= range; step++)
                {
                    var pos = character.Position + direction * step;

                    if (!_tilesStorage.TryGet(pos, out var tile))
                        continue;

                    if (!tile.IsWalkable)
                        continue;

                    if (occupants.TryGetValue(pos, out var occupant))
                    {
                        if (occupant.Team == character.Team)
                            break;

                        tiles.Add(pos);
                        break;
                    }

                    tiles.Add(pos);
                }
            }

            return tiles;
        }

        private void EnsureOverlay(Vector2Int pos)
        {
            if (_originalTiles.ContainsKey(pos))
                return;

            var cell = ToCell(pos);
            _originalTiles[pos] = _tilemap.GetTile(cell);
            _tilemap.SetTile(cell, _config.OverlayTile);
            _tilemap.SetTileFlags(cell, TileFlags.None);
        }

        private void RestoreOriginal(Vector2Int pos)
        {
            if (!_originalTiles.TryGetValue(pos, out var original))
                return;

            _tilemap.SetTile(ToCell(pos), original);
            _originalTiles.Remove(pos);
        }

        private void SetOverlayColor(Vector2Int pos, Color color) =>
            _tilemap.SetColor(ToCell(pos), color);

        private static bool ContainsTile(List<Vector2Int> tiles, Vector2Int pos)
        {
            for (var i = 0; i < tiles.Count; i++)
            {
                if (tiles[i] == pos)
                    return true;
            }

            return false;
        }

        private static Vector3Int ToCell(Vector2Int pos) =>
            new(pos.x, pos.y, 0);

        private sealed class Session
        {
            public List<Vector2Int> Tiles;
            public Color TargetColor;
            public int Order;
            public Tween Tween;
        }
    }
}
