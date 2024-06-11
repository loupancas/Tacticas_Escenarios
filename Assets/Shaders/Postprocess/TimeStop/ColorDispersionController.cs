using UnityEngine;

[ExecuteInEditMode]
public class ColorDispersionController : MonoBehaviour
{
    public Material material;

    [Range(0, 1)]
    public float colorDispersionStrength = 0.1f;
    [Range(-1, 1)]
    public float colorDispersionU = 0.5f;
    [Range(-1, 1)]
    public float colorDispersionV = 0.5f;
    [Range(0, 1)]
    public float blackWhiteThreshold = 0.5f;
    [Range(0, 1)]
    public float blackWhiteWidth = 0.1f;
    public Color blackWhiteWhiteColor = Color.white;
    public Color blackWhiteBlackColor = Color.black;
    public bool enableBlackWhite = false;

    void Update()
    {
        if (material != null)
        {
            material.SetFloat("_ColorDispersionStrength", colorDispersionStrength);
            material.SetFloat("_ColorDispersionU", colorDispersionU);
            material.SetFloat("_ColorDispersionV", colorDispersionV);
            material.SetFloat("_BlackWhiteThreshold", blackWhiteThreshold);
            material.SetFloat("_BlackWhiteWidth", blackWhiteWidth);
            material.SetColor("_BlackWhiteWhiteColor", blackWhiteWhiteColor);
            material.SetColor("_BlackWhiteBlackColor", blackWhiteBlackColor);
            material.SetFloat("_EnableBlackWhite", enableBlackWhite ? 1.0f : 0.0f);
        }
    }
}
