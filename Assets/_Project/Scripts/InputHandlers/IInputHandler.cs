using System;
using UnityEngine;

namespace _Project.Scripts.InputHandlers
{
    public interface IInputHandler
    {
        event Action<Vector2Int> OnPressed;
    }
}