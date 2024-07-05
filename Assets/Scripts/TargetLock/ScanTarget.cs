using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ScanTarget : MonoBehaviour
{
    [SerializeField]
    float updateRate;
    [SerializeField]
    Color normalColor;
    [SerializeField]
    Color lockColor;
    //[SerializeField] TMP_Text _text;

    [SerializeField]
    Transform targetBox;
    [SerializeField]
    TMP_Text targetName;
    [SerializeField]
    TMP_Text targetRange;
    [SerializeField]
    Transform missileLock;
    [SerializeField]
    Transform reticle;
    [SerializeField]
    RectTransform reticleLine;
  
    [SerializeField]
    RectTransform targetArrow;
    [SerializeField]
    RectTransform missileArrow;
    [SerializeField]
    float targetArrowThreshold;
    [SerializeField]
    float missileArrowThreshold;
    [SerializeField]
    float cannonRange;
    [SerializeField]
    float bulletSpeed;
  

 

    FirstPersonPlayer player;

    new Camera camera;
    Transform cameraTransform;


    GameObject targetBoxGO;
    Image targetBoxImage;
    GameObject missileLockGO;
    Image missileLockImage;
    GameObject reticleGO;
    GameObject targetArrowGO;
 

 



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateWeapons()
    {
        if (player.Target == null)
        {
            targetBoxGO.SetActive(false);
            missileLockGO.SetActive(false);
            return;
        }

        //update target box, missile lock
        var targetDistance = Vector3.Distance(player.Rigidbody.position, player.Target.Position);
        var targetPos = TransformToHUDSpace(player.Target.Position);
        var missileLockPos = player.MissileLocked ? targetPos : TransformToHUDSpace(player.Rigidbody.position + player.MissileLockDirection * targetDistance);

        if (targetPos.z > 0)
        {
            targetBoxGO.SetActive(true);
            targetBox.localPosition = new Vector3(targetPos.x, targetPos.y, 0);
        }
        else
        {
            targetBoxGO.SetActive(false);
        }

        if (player.MissileTracking && missileLockPos.z > 0)
        {
            missileLockGO.SetActive(true);
            missileLock.localPosition = new Vector3(missileLockPos.x, missileLockPos.y, 0);
        }
        else
        {
            missileLockGO.SetActive(false);
        }

        if (player.MissileLocked)
        {
            targetBoxImage.color = lockColor;
            targetName.color = lockColor;
            targetRange.color = lockColor;
            missileLockImage.color = lockColor;
        }
        else
        {
            targetBoxImage.color = normalColor;
            targetName.color = normalColor;
            targetRange.color = normalColor;
            missileLockImage.color = normalColor;
        }

        targetName.text = player.Target.Name;
        targetRange.text = string.Format("{0:0 m}", targetDistance);

        //update target arrow
        var targetDir = (player.Target.Position - player.Rigidbody.position).normalized;
        var targetAngle = Vector3.Angle(cameraTransform.forward, targetDir);

        if (targetAngle > targetArrowThreshold)
        {
            targetArrowGO.SetActive(true);
            //add 180 degrees if target is behind camera
            float flip = targetPos.z > 0 ? 0 : 180;
            targetArrow.localEulerAngles = new Vector3(0, 0, flip + Vector2.SignedAngle(Vector2.up, new Vector2(targetPos.x, targetPos.y)));
        }
        else
        {
            targetArrowGO.SetActive(false);
        }

        //update target lead
        var leadPos = Utilities.FirstOrderIntercept(player.Rigidbody.position, player.Rigidbody.velocity, bulletSpeed, player.Target.Position, player.Target.Velocity);
        var reticlePos = TransformToHUDSpace(leadPos);

        if (reticlePos.z > 0 && targetDistance <= cannonRange)
        {
            reticleGO.SetActive(true);
            reticle.localPosition = new Vector3(reticlePos.x, reticlePos.y, 0);

            var reticlePos2 = new Vector2(reticlePos.x, reticlePos.y);
            if (Mathf.Sign(targetPos.z) != Mathf.Sign(reticlePos.z)) reticlePos2 = -reticlePos2;    //negate position if reticle and target are on opposite sides
            var targetPos2 = new Vector2(targetPos.x, targetPos.y);
            var reticleError = reticlePos2 - targetPos2;

            var lineAngle = Vector2.SignedAngle(Vector3.up, reticleError);
            reticleLine.localEulerAngles = new Vector3(0, 0, lineAngle + 180f);
            reticleLine.sizeDelta = new Vector2(reticleLine.sizeDelta.x, reticleError.magnitude);
        }
        else
        {
            reticleGO.SetActive(false);
        }
    }

    Vector3 TransformToHUDSpace(Vector3 worldSpace)
    {
        var screenSpace = camera.WorldToScreenPoint(worldSpace);
        return screenSpace - new Vector3(camera.pixelWidth / 2, camera.pixelHeight / 2);
    }

    private void LateUpdate()
    {
        UpdateWeapons();
    }

}
