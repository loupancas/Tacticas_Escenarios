using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgPopUp : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;
    private TextMeshProUGUI tmp;
    private float time = 0;
    public Transform origin;
    EnemigoVolador _enemigo;
    private void Start()
    {
        tmp= transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //origin = transform.position;
        //_enemigo = new EnemigoVolador();
    }

    private void Update()
    {
        if(_enemigo!=null)
        //origin = _enemigo.transform.position;

        tmp.color = new Color (1,1,1,opacityCurve.Evaluate (time));
        transform.localScale= Vector3.one*scaleCurve.Evaluate (time);
        transform.position= origin.position + new Vector3 (0,1*heightCurve.Evaluate (time),0);
        time += Time.deltaTime;

    }

    


    

}
