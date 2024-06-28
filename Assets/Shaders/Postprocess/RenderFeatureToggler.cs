using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class RenderFeatureToggle
{
    public ScriptableRendererFeature feature;
    public bool isEnabled;
    public int id;



}

[ExecuteAlways]
public class RenderFeatureToggler : MonoBehaviour
{
    [SerializeField]
    private List<RenderFeatureToggle> renderFeatures = new List<RenderFeatureToggle>();

    private int currentActiveId = 1;

    public  void ToggleFeatures(int featureId)
    {
        if (currentActiveId != 1)
        {
            var currentFeature = renderFeatures.Find(rf => rf.id == currentActiveId);
            if (currentFeature != null)
            {
                currentFeature.isEnabled = false;
                currentFeature.feature.SetActive(false);
            }

           
        }
        currentActiveId = featureId;
        StartCoroutine(ActivateFeatureCoroutine(currentActiveId));
        Debug.Log("NormalFeature");
    }

    private IEnumerator ActivateFeatureCoroutine(int id)
    {
        var nextFeature = renderFeatures.Find(rf => rf.id == id);
        if (nextFeature != null)
        {
            nextFeature.isEnabled = true;
            nextFeature.feature.SetActive(true);

            yield return new WaitForSeconds(10);

            nextFeature.isEnabled = false;
            nextFeature.feature.SetActive(false);
        }

       
    }


}


