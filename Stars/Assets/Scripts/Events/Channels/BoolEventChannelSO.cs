using UnityEngine;

[CreateAssetMenu(fileName = "Bool Event Channel", menuName = "Events/Bool Event Channel")]
public class BoolEventChannelSO : GenericEventChannelSO<bool>
{
    public void RaiseEvent(bool data)
    {
        base.RaiseEvent(data);
    }
}
