using UnityEngine;
using System.Collections;

public class TransitionController : MonoBehaviour
{
    public Material material;

    void Start()
    {
        StartCoroutine(TransitionToBlackWhite());
    }

    IEnumerator TransitionToBlackWhite()
    {
        float duration = 2.0f; // Duration of the transition in seconds
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float transitionValue = Mathf.Lerp(0.0f, 1.0f, t / duration);
            material.SetFloat("_Transition", transitionValue);
            yield return null;
        }
        material.SetFloat("_Transition", 1.0f); // Ensure we end exactly at 1
    }
}
