using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class RenderFeatureToggle
{
    public ScriptableRendererFeature feature;
    public bool isEnabled;
}

[ExecuteAlways]
public class RenderFeatureToggler : MonoBehaviour
{
    [SerializeField]
    private List<RenderFeatureToggle> renderFeatures = new List<RenderFeatureToggle>();

    private int currentActiveIndex = -1;

    private void Update()
    {
        // Al presionar la tecla "X", se intercambia el estado de las características
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleFeatures();
        }
    }

    private void ToggleFeatures()
    {
        // Desactivar la característica actual
        if (currentActiveIndex >= 0 && currentActiveIndex < renderFeatures.Count)
        {
            renderFeatures[currentActiveIndex].isEnabled = false;
            renderFeatures[currentActiveIndex].feature.SetActive(false);
        }

        // Activar la siguiente característica en la lista
        currentActiveIndex = (currentActiveIndex + 1) % renderFeatures.Count;

        renderFeatures[currentActiveIndex].isEnabled = true;
        renderFeatures[currentActiveIndex].feature.SetActive(true);
    }
}