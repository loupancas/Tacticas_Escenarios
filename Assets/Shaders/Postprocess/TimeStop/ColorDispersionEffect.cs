using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
[VolumeComponentMenu("Custom/ColorDispersionEffect")]
public class ColorDispersionEffect : VolumeComponent, IPostProcessComponent
{
    public ClampedFloatParameter dispersionStrength = new ClampedFloatParameter(0.1f, 0f, 1f);
    public FloatParameter dispersionU = new FloatParameter(0.5f);
    public FloatParameter dispersionV = new FloatParameter(0.5f);
    public ClampedFloatParameter blackWhiteThreshold = new ClampedFloatParameter(0.5f, 0f, 1f);
    public ClampedFloatParameter blackWhiteWidth = new ClampedFloatParameter(0.1f, 0f, 1f);
    public ColorParameter blackWhiteWhiteColor = new ColorParameter(Color.white);
    public ColorParameter blackWhiteBlackColor = new ColorParameter(Color.black);
    public BoolParameter enableBlackWhite = new BoolParameter(false);

    public bool IsActive() => dispersionStrength.value > 0f || enableBlackWhite.value;
    public bool IsTileCompatible() => false;
}
