using System.Collections;
using UnityEngine;


public class UpgradeController : MonoBehaviour
{
    MovementHandler movementHandler;
    [SerializeField] private float maxSpeed;
    [SerializeField] TrailRenderer SpeedTrail;
    // [SerializeField] private float initialSpeed;


    public static bool UpgradeHapenning = false;

    private Coroutine speedCoroutine;

    private void Start()
    {
        movementHandler = GetComponent<MovementHandler>();
        SpeedTrail.enabled = false;
        // initialSpeed = movementHandler.currentMoveDuration;
    }

    // Apply Speed Boost for a given time considering max speed player can reach
    public void ApplyTempSpeedUpgrade(float speedBost, float duration)
    {
        if (UpgradeHapenning) return;
        speedCoroutine = StartCoroutine(ApplyTempSpeedUpgradeRoutine(speedBost, duration));
    }
    IEnumerator ApplyTempSpeedUpgradeRoutine(float speedBost, float duration)
    {
        UpgradeHapenning = true;
        // also turn on rails 
        if (movementHandler.currentMoveDuration - speedBost < maxSpeed)
        {
            // only sets player to the max speed
            movementHandler.currentMoveDuration = maxSpeed;
            yield break;
        }


        SpeedTrail.enabled = true;
        movementHandler.currentMoveDuration -= speedBost;
        yield return new WaitForSeconds(duration);
        SpeedTrail.enabled = false;
        UpgradeHapenning = false;

        movementHandler.currentMoveDuration = movementHandler.baseMoveDuration;

    }
}
