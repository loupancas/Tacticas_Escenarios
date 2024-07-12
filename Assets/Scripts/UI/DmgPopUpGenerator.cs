using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgPopUpGenerator : MonoBehaviour
{
    public static DmgPopUpGenerator current;

    public GameObject prefab;

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {
        
    }
    public void CreatePopUp(Vector3 position, string _text, Color color)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = _text;
        temp.faceColor = color;
        Destroy(popup,1f);
    }
}
