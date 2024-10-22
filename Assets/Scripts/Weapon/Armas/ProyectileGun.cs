using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileGun : WeaponBase
{
    public Vector2[] phaseSpeeds; // Array de velocidades para cada fase
    public Color[] phaseColors; // Array de colores para cada fase
    [SerializeField] ProyectilesBase proyectil;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] float _proyectileSpeed;
    [SerializeField] private BuffFase _buff;
    public float[] phaseFresnel;

    
    public override void Fases(int fase)
    {
        switch(fase)
        {
            case 0:
                proyectil.SetDistance(10);
                proyectil.SetDmg(dmg);
                proyectil.SetSpeed(_proyectileSpeed);
                _modifiedCooldown = shotCooldown;
                UpdateChildMaterials(phaseSpeeds[0], phaseColors[0], phaseFresnel[0]);
                _buff.StartCoroutine(_buff.LerpTestStat(_buff._lastTestStat, 0, _buff.fadeOutTime));
                _buff._lastTestStat = 0;
                break;
            case 1:
                proyectil.SetDistance(10);
                proyectil.SetDmg(dmg + 5);
                proyectil.SetSpeed(_proyectileSpeed + 5);
                _modifiedCooldown = shotCooldown;
                UpdateChildMaterials(phaseSpeeds[1], phaseColors[1], phaseFresnel[1]);
                _buff._testStat = 0.03f;
                _buff.StartCoroutine(_buff.LerpTestStat(_buff._lastTestStat, _buff._testStat, _buff.fadeOutTime));
                _buff._lastTestStat = _buff._testStat;

                break;
            case 2:
                proyectil.SetDistance(10);
                proyectil.SetDmg(dmg + 15);
                proyectil.SetSpeed(_proyectileSpeed + 10);
                _modifiedCooldown = 0.3f;
                UpdateChildMaterials(phaseSpeeds[2], phaseColors[2], phaseFresnel[2]);
                _buff._testStat = 0.12f;
                _buff.StartCoroutine(_buff.LerpTestStat(_buff._lastTestStat, _buff._testStat, _buff.fadeOutTime));
                _buff._lastTestStat = _buff._testStat;
                break;
            case 3:
                proyectil.SetDistance(10);
                proyectil.SetDmg(dmg + 20);
                proyectil.SetSpeed(_proyectileSpeed + 20);
                _modifiedCooldown = 0.1f;
                UpdateChildMaterials(phaseSpeeds[3], phaseColors[3], phaseFresnel[3]);
                _buff._testStat = 1f;
                _buff.StartCoroutine(_buff.LerpTestStat(_buff._lastTestStat, _buff._testStat, _buff.fadeOutTime));
                _buff._lastTestStat = _buff._testStat;

                break;
        }
    }
    private void UpdateChildMaterials(Vector2 newSpeed, Color newColor, float newIntensity)
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
                    mat.SetFloat("_Fresnel", newIntensity); // Cambia "_Color" al nombre de la propiedad del shader

                }
            }
        }
    }
    protected override void FireBehaviour()
    {
        _particula.Play();
        _weaponAnimator.SetTrigger("Shoot");
        print("Disparo con Proyectil Gun");
        proyectil.SpawnProyectile(_spawnPoint);
        cameraTransform.localEulerAngles += new Vector3(-cameraRecoilAmount, 0, 0);
    }

}
