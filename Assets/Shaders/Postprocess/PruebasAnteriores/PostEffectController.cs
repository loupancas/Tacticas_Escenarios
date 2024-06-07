using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode,ImageEffectAllowedInSceneView]
public class PostEffectController : MonoBehaviour
{
    public Shader postShader;
    Material postEffectMaterial;
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if(postEffectMaterial == null)
        {
            postEffectMaterial = new Material(postShader);
            //postEffectMaterial.hideFlags = HideFlags.HideAndDontSave;
        }
        RenderTexture renderTexture = RenderTexture.GetTemporary(src.width, src.height,0,src.format); // 0 es el pass

        Graphics.Blit(src, renderTexture, postEffectMaterial);
        Graphics.Blit(renderTexture, dest);
        RenderTexture.ReleaseTemporary(renderTexture);
    }
}
