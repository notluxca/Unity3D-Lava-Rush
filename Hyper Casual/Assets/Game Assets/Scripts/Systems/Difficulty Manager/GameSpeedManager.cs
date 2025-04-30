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

        _player.baseMoveDuration = playerInitialSpeed;
        _player.currentMoveDuration = playerInitialSpeed;
        _camera.currentVelocity.z = cameraInitialSpeed;
    }

    public void CheckDificulty()
    {
        tiles += 1;
        if (tiles >= gameSpeedDatas[difficultIndex].requiredTiles)
        {
            difficultIndex++;
            _camera.currentVelocity.z = gameSpeedDatas[difficultIndex].cameraSpeed;

            // se o player estiver no boost de velocidade, apenas altere a referencia de velocidade, caso não altere sua velocidade e a referencia
            if (UpgradeController.UpgradeHapenning) _player.baseMoveDuration = gameSpeedDatas[difficultIndex].playerSpeed;
            else
            {
                _player.baseMoveDuration = gameSpeedDatas[difficultIndex].playerSpeed;
                _player.currentMoveDuration = gameSpeedDatas[difficultIndex].playerSpeed;
            }



            Debug.Log("Difficulty changed");
        }
    }

    private void PlayerFirstMove(Vector3 position)
    {
        if (!gameStarted) gameStarted = true;
        _camera.StartCamera();
        PlayerEvents.OnPlayerFirstMove -= PlayerFirstMove;

    }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 30, 1500, 300), "<size=50>Current Speed: " + _player.currentMoveDuration + "</size>");
        GUI.Label(new Rect(10, 75, 1500, 300), "<size=50>Stage Player Speed: " + _player.baseMoveDuration + "</size> <size=25> - Controlled by game</size>");
    }

}

