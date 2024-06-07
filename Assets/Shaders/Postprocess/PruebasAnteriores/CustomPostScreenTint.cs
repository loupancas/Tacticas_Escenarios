using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("CustomPostScreenTint", typeof(UniversalRenderPipeline))]
public class CustomPostScreenTint : VolumeComponent, IPostProcessComponent
{
    public FloatParameter tintIntensity = new FloatParameter(1f);
    public ColorParameter tintColor = new ColorParameter(Color.white);
    public bool IsActive() => true;
    public bool IsTileCompatible() => true;

}

