

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


        //Visto en Unity Forum
        //// This is the correct way to get the VolumeProfile component.
        //// You can also use GetComponentInChildren if the VolumeProfile is a child of the GameObject.
        //UnityEngine.Rendering.VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        //if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        //// You can leave this variable out of your function, so you can reuse it throughout your class.
        //UnityEngine.Rendering.Universal.Vignette vignette;

        //if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

        //vignette.intensity.Override(0.5f);

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
                break;
            case 1:
                volume.profile = profile2;
                break;
            case 2:
                volume.profile = profile3;
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

