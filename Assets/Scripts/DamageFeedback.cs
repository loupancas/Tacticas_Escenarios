using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFeedback : MonoBehaviour
{
    private Material _material;
    private Color _originalColor;
    private bool _isFlashing;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int FlashColor = Shader.PropertyToID("_FlashColor");
    private static readonly int Extremos = Shader.PropertyToID("_Extremos");

    [SerializeField]
    [ColorUsage(true, true)] // Permite valores HDR y ajuste del alpha
    private Color hdrFlashColor = new Color(1f, 0.5f, 0.5f, 1f); // Color HDR de flash tomado desde el editor

    [SerializeField] private float flashDuration = 0.25f; // Duración de cada titilación
    private int flashCount = 4; // Cantidad de titilaciones

    private void Start()
    {
        InitializeMaterial();
    }

    private void InitializeMaterial()
    {
        // Buscar el objeto "drone" que es hijo del GameObject que contiene este script
        Transform droneTransform = transform.Find("drone");
        if (droneTransform != null)
        {
            // Buscar el Renderer en el objeto "drone"
            Renderer renderer = droneTransform.Find("textureEditorIsolateSelectSet_polySurface9_pCylinder10")?.GetComponent<Renderer>();
            if (renderer != null)
            {
                _material = renderer.material;
                _originalColor = _material.GetColor(BaseColor);
                _material.SetColor(FlashColor, hdrFlashColor); // Usar el color HDR como flash color
                _material.SetFloat(Extremos, 0f); // Inicializar el slider en 0
            }
            else
            {
                Debug.LogError("Renderer component not found on textureEditorIsolateSelectSet_polySurface9_pCylinder10.");
            }
        }
        else
        {
            Debug.LogError("Drone GameObject not found.");
        }
    }

    public void TakeDamage()
    {
        if (!_isFlashing)
        {
            StartCoroutine(Flash());
        }
    }

    public IEnumerator Flash()
    {
        _isFlashing = true;

        for (int i = 0; i < flashCount; i++)
        {
            float elapsedTime = 0f;
            while (elapsedTime < flashDuration)
            {
                float sliderValue = Mathf.PingPong(elapsedTime, flashDuration) / flashDuration;
                _material.SetFloat(Extremos, sliderValue);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _material.SetFloat(Extremos, 0f);

            yield return new WaitForSeconds(flashDuration); // Esperar entre titilaciones
        }

        _isFlashing = false;
    }
}