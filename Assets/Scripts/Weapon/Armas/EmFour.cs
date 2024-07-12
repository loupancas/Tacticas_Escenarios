using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmFour : WeaponBase
{
    public Vector2[] phaseSpeeds; // Array de velocidades para cada fase
    public Color[] phaseColors; // Array de colores para cada fase

    public override void Fases(int fase)
    {
        switch (fase)
        {
            case 0:
                _modifiedDmg = dmg;
                _modifiedCooldown = shotCooldown;
                UpdateChildMaterials(phaseSpeeds[0], phaseColors[0]);
                break;
            case 1:
                _modifiedCooldown = 0.07f;
                _modifiedDmg = 7;
                UpdateChildMaterials(phaseSpeeds[1], phaseColors[1]);
                break;
            case 2:
                _modifiedCooldown = 0.068f;
                _modifiedDmg = 7;
                UpdateChildMaterials(phaseSpeeds[2], phaseColors[2]);
                break;
            case 3:
                _modifiedDmg = 10;
                UpdateChildMaterials(phaseSpeeds[3], phaseColors[3]);
                break;
        }
    }

    private void UpdateChildMaterials(Vector2 newSpeed, Color newColor)
    {
        foreach (Transform child in transform)
        {
            Renderer childRenderer = child.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                foreach (Material mat in childRenderer.materials)
                {
                    mat.SetVector("_Speed", newSpeed); // Cambia "_Speed" al nombre de la propiedad del shader
                    mat.SetColor("_Color", newColor); // Cambia "_Color" al nombre de la propiedad del shader
                }
            }
        }
    }

    protected override void FireBehaviour()
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        //_ray = new Ray(_shotTransform.position, _shotTransform.forward);

        if (Physics.Raycast(ray, out _rayHit, _shootableLayers))
        {
            print("Detecto m4");
            _rayHit.collider.GetComponent<EnemigoVolador>()?.TakeDamage(_modifiedDmg);
            //_rayHit.collider.GetComponent<EnemigoVolador>()?.AddDamage(_modifiedDmg);
            _rayHit.collider.GetComponent<PuntosDebiles>()?.OnHit(_modifiedDmg);
        }
    }
}