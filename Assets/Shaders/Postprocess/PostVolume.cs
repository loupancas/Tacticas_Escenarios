

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PostVolume : MonoBehaviour
{
    private void Awake()
    {
        // This is the correct way to get the VolumeProfile component.
        // You can also use GetComponentInChildren if the VolumeProfile is a child of the GameObject.
        UnityEngine.Rendering.VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        // You can leave this variable out of your function, so you can reuse it throughout your class.
        UnityEngine.Rendering.Universal.Vignette vignette;

        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

        vignette.intensity.Override(0.5f);

    }

    private void Update()
    {
       
    }
   
   
}

