using UnityEngine;

public class GemCollectable : CollectableItem
{
    public static event System.Action gemCollected; // should not create a new delegate every time there is a gem and instead use a GameEvent.GemCollected

    public override void OnCollect()
    {
        base.OnCollect();
        gemCollected?.Invoke();
    }

    private void Update() {
        
    }
}

