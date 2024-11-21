using UnityEngine;

[CreateAssetMenu(fileName = "Vector2 Event Channel", menuName = "Events/Vector2 Event Channel")]

public class Vector2EventChannelSO : GenericEventChannelSO<Vector2>
{
    public void RaiseEvent(Vector2 data)
    {
        base.RaiseEvent(data);
    }
}
