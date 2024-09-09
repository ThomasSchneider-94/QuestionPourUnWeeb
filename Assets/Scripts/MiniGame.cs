using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MiniGame : MonoBehaviour
{
    [SerializeField] protected float gameDuration = 40f;
    [SerializeField] private InputActionReference resetGameAction;

#region Enable/Disable
    public virtual void OnEnable()
    {
        resetGameAction.action.started += resetGame;
        enableActions();
    }
    public virtual void OnDisable()
    {
        resetGameAction.action.started -= resetGame;
        disableActions();
    }

    // Manage the possible actions
    public virtual void enableActions() { resetGameAction.action.Enable(); }
    public virtual void disableActions() { resetGameAction.action.Disable(); }
#endregion Enable/Disable

    // Reset Game
    public abstract void resetGame();
    private void resetGame(InputAction.CallbackContext context) { resetGame(); }

    }
