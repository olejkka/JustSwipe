using System;
using _Project.Scripts.Characters;
using UnityEngine;

namespace _Project.Scripts.InputHandlers
{
    public interface IInputHandler
    {
        event Action<Vector2Int, Team> OnPressed;
    }
}