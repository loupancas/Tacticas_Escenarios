using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CondicionDeVictoria : MonoBehaviour
{
    public bool IsActive = false;
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.GetComponent<FirstPersonPlayer>() != null && IsActive)
            SceneManager.LoadScene(2);
    }
}
