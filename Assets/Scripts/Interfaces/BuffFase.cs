using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BuffFase : MonoBehaviour
{
    private int _voronoiPower = Shader.PropertyToID("_VoronoiPower");
    private int _intensidad1 = Shader.PropertyToID("_Intensidad1");
    private int _intensidad2 = Shader.PropertyToID("_Intensidad_2");
    private int _vectorTime = Shader.PropertyToID("_VectorTime");
    private int _color = Shader.PropertyToID("_Color");
    private int _width = Shader.PropertyToID("_Width");
    private int _scale = Shader.PropertyToID("_Scale");
    private int _voronoiSize = Shader.PropertyToID("_Color2");
    private int _timeScale = Shader.PropertyToID("_TimeScale");
    private int _angleOffset = Shader.PropertyToID("_AngleOffset");
    private int _test = Shader.PropertyToID("_Test");

    public float _elapsedTime;

    public float fadeOutTime = 1f;  // Asegúrate de asignar 1 aquí
    [SerializeField] public ScriptableRendererFeature _buff1;
    [SerializeField] public Material _buff;
    [SerializeField] public float _testStat = 0f;
    [SerializeField] public float _lastTestStat = 0f;  // Asegúrate de asignar 1 aquí
                                                   // Asegúrate de asignar 1 aquí

    // Start is called before the first frame update
    void Start()
    {
        _buff1.SetActive(false);
        _elapsedTime = 1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator LerpTestStat(float startValue, float endValue, float duration, System.Action onComplete = null)
    {
        if (_testStat != 0)
        {
            _buff1.SetActive(true);

        }
        float startTime = Time.time;
        _elapsedTime = 0; // Reset _elapsedTime for the coroutine

        while (_elapsedTime < duration)
        {
            _elapsedTime = Time.time - startTime;
            float lerpedTest = Mathf.Lerp(startValue, endValue, (_elapsedTime / duration));
            _buff.SetFloat(_test, lerpedTest);
            yield return null;
        }

        _buff.SetFloat(_test, endValue); // Ensure it ends at the final value
        onComplete?.Invoke(); // Call the onComplete action if provided
    }

    
}