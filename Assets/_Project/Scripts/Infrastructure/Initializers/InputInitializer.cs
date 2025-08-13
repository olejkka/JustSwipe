using System;
using _Project.Scripts.Characters;
using _Project.Scripts.InputHandlers;
using VContainer.Unity;

namespace _Project.Scripts.Infrastructure.Initializers
{
    public class InputInitializer : IStartable, IDisposable
    {
    //     private readonly KeyboardInputHandler _keyboardInputHandler;
    //     private readonly SwipeInputHandler _swipeInputHandler;
    //     private readonly BotInputHandler _botInputHandler;
    //     
    //     private readonly CharactersMover _charactersMover;
    //     private readonly CharacterSpawnController _characterSpawnController;
    //
    //
    //     public InputInitializer(
    //         KeyboardInputHandler keyboardInputHandler,
    //         SwipeInputHandler swipeInputHandler,
    //         BotInputHandler botInputHandler,
    //         CharactersMover charactersMover,
    //         CharacterSpawnController characterSpawnController
    //     )
    //     {
    //         _keyboardInputHandler = keyboardInputHandler;
    //         _swipeInputHandler = swipeInputHandler;
    //         _botInputHandler = botInputHandler;
    //         _charactersMover = charactersMover;
    //         _characterSpawnController = characterSpawnController;
    //     }
    //
    public void Start()
    {
    //         // _keyboardInputHandler.OnPressed += _charactersMover.Move;
    //         // _swipeInputHandler.OnPressed += _charactersMover.Move;
    //         // _botInputHandler.OnPressed += _charactersMover.Move;
    //         //
    //         // _keyboardInputHandler.OnPressed += _characterSpawnController.HandleInput;
    //         // _swipeInputHandler.OnPressed += _characterSpawnController.HandleInput;
    }
    //
    public void Dispose()
    {
    //         _keyboardInputHandler.OnPressed -= _charactersMover.Move;
    //         _swipeInputHandler.OnPressed -= _charactersMover.Move;
    //         _botInputHandler.OnPressed -= _charactersMover.Move;
    //         
    //         _keyboardInputHandler.OnPressed -= _characterSpawnController.HandleInput;
    //         _swipeInputHandler.OnPressed -= _characterSpawnController.HandleInput;
    }
    }
}