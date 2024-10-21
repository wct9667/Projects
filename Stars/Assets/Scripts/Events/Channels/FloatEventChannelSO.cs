using UnityEngine;

[CreateAssetMenu(fileName = "Float Event Channel", menuName = "Events/Float Event Channel")]
public class FloatEventChannelSO : GenericEventChannelSO<float>
{
    public void RaiseEvent(float data)
    {
        base.RaiseEvent(data);
    }
}
