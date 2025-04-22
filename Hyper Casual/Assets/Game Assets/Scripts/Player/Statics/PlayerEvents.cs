using System;
using UnityEngine;

public static class PlayerEvents
{
    public static event Action<Vector3> OnPlayerMove;
    public static event Action<Vector3> OnPlayerFirstMove;
    public static event Action onPlayerDied;
    public static event Action onPlayerDiedByPlataformFall;
    public static event Action onPlayerRevived;
    public static event Action OnPlayerSwipeLeft;
    public static event Action OnPlayerSwipeRight;
    public static event Action OnPlayerTap;
    public static event Action OnCharacterModelChanged;
    public static event Action OnPlayerCollidedWithPlatform;
    public static event Action OnCharacterLoaded;


    public static void PlayerFirstMove(Vector3 newPosition)
    {
        OnPlayerFirstMove?.Invoke(newPosition);
        // Debug.Log("Player First Move");
    }

    public static void PlayerMoved(Vector3 newPosition)
    {
        OnPlayerMove?.Invoke(newPosition);
    }

    public static void PlayerDied()
    {
        onPlayerDied?.Invoke();
    }

    public static void PlayerRevived()
    {
        onPlayerRevived?.Invoke();
    }

    public static void PlayerDiedOnPlataformFall()
    {
        onPlayerDiedByPlataformFall?.Invoke();
    }

    public static void PlayerSwipe(Vector2 swipeDirection)
    {
        Vector3 horizontal = swipeDirection.x > 0 ? Vector3.right : Vector3.left;
        if (horizontal == Vector3.right)
        {
            // Debug.Log("Swipe Right");
            OnPlayerSwipeRight?.Invoke();
        }
        else
        {
            // Debug.Log("Swipe Left");
            OnPlayerSwipeLeft?.Invoke();
        }
    }

    public static void PlayerTap()
    {
        OnPlayerTap?.Invoke();
    }

    public static void CharacterModelChanged()
    {
        OnCharacterModelChanged?.Invoke();
    }

    public static void PlayerColidedWithPlatform()
    {
        OnPlayerCollidedWithPlatform?.Invoke();
    }

    public static void CharacterLoaded() { OnCharacterLoaded?.Invoke(); }

}
