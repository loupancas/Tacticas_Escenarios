

using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostVolume : MonoBehaviour
{
    public VolumeProfile profile1;
    public VolumeProfile profile2;
    public VolumeProfile profile3;

    private Volume volume;
    private int currentProfile = 0;
    private void Awake()
    {
        volume = GetComponent<Volume>();
        if (!volume) throw new System.NullReferenceException(nameof(volume));
        if (profile1 == null || profile2 == null || profile3 == null) throw new System.NullReferenceException("Profiles are null");
        SetProfile(currentProfile);
     
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            currentProfile = (currentProfile + 1) % 3;
            SetProfile(currentProfile);
        }
    }

    private void SetProfile(int index)
    {
        switch (index)
        {
            case 0:
                volume.profile = profile1;
                Debug.Log("No Postprocess");
                break;
            case 1:
                volume.profile = profile2;
                Debug.Log("Scan");
                break;
            case 2:
                volume.profile = profile3;
                Debug.Log("Time");
                break;
            default:
                throw new System.ArgumentOutOfRangeException();
        }

        if (volume.profile.TryGet(out Vignette vignette))
        {
            vignette.intensity.Override(0.5f);  
        }

    }
}

