using UnityEngine;

public class CanCollectable : CollectableItem
{
    public float speedBoostValue;
    public float Duration;

    public override void OnCollect()
    {
        base.OnCollect();
        FindAnyObjectByType<UpgradeController>().ApplyTempSpeedUpgrade(speedBoostValue, Duration);
    }

    private void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0.5f, 0.5f), 10);
    }


}
