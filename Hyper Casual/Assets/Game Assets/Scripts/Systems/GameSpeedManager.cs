using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class GameSpeedData
{
    public float playerSpeed;
    public float cameraSpeed;
    public int requiredTiles;
}

public class GameSpeedManager : MonoBehaviour
{
    [SerializeField] private MovementHandler _player;
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private GameSpeedData[] gameSpeedDatas;
    private int difficultIndex = 0;
 
    public float playerInitialSpeed;
    public float cameraInitialSpeed;
    private bool gameStarted = false;
    private int tiles = 0;

    private void Start()
    {
        PlayerEvents.OnPlayerFirstMove += PlayerFirstMove;
        Platform.OnPlatformJump += CheckDificulty;
        _player.moveDuration = playerInitialSpeed;
        _camera.currentVelocity.z = cameraInitialSpeed;
    }

    public void CheckDificulty(){
        tiles += 1;
        if(tiles >= gameSpeedDatas[difficultIndex].requiredTiles){
            difficultIndex++;
            _player.moveDuration = gameSpeedDatas[difficultIndex].playerSpeed;
            _camera.currentVelocity.z = gameSpeedDatas[difficultIndex].cameraSpeed;
            Debug.Log("Difficulty changed");
        }
    }   

    private void PlayerFirstMove(Vector3 position){
        if(!gameStarted) gameStarted = true;
        _camera.StartCamera();
        PlayerEvents.OnPlayerFirstMove -= PlayerFirstMove; 
        
    }
    
}

