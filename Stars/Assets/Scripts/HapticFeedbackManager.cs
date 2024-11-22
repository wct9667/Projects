using UnityEngine;

public class HapticFeedbackManager : MonoBehaviour
{
    private bool hapticsEnabled = true;
    public void TriggerHaptic()
    {
            if (!hapticsEnabled) return;
#if UNITY_IOS
        Device.PlayFeedbackGeneratorFeedback(FeedbackType.MediumImpact);
#elif UNITY_ANDROID
        Handheld.Vibrate();
#else
        Debug.Log("Haptic feedback not supported on this platform.");
#endif
    }

    public void ToggleHaptics(bool hapticsEnabled)
    {
        this.hapticsEnabled = hapticsEnabled;
        Debug.Log(this.hapticsEnabled);
    }

}
