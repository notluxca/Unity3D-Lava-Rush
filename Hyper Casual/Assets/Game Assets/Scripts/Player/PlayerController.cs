using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Main Player Settings")]
    public InputHandler playerInput { get; private set; }
    public MovementHandler playerMovement { get; private set; } 
    public CollisionHandler playerCollision { get; private set; }

    private void Start()
    {
        playerInput = GetComponent<InputHandler>();
        playerMovement = GetComponent<MovementHandler>();
        playerCollision = GetComponent<CollisionHandler>();
        playerMovement.Initialize();
    }

    private void Update()
    {
        playerInput.HandleInput();
        // speedManager.UpdateSpeed(); //! Player shouldn't be responsabile for managing game speed, create a centralizedClass for that
    }
}

