using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TintRenderFeature : ScriptableRendererFeature
{
    private TintPass tintPass;

    public override void Create()
    {
        tintPass = new TintPass();
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(tintPass);
    }

    class TintPass : ScriptableRenderPass
    {
        private Material _mat;
        int tintID = Shader.PropertyToID("_Temp");
        RenderTargetIdentifier scr, tint;
        public TintPass()
        {
            if (!_mat)
            {
                _mat = CoreUtils.CreateEngineMaterial("CustomPost/ScreenTint");

            }

            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            scr = renderingData.cameraData.renderer.cameraColorTarget;
            cmd.GetTemporaryRT(tintID, desc, FilterMode.Bilinear);
            tint = new RenderTargetIdentifier(tintID);
        }


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get("TintRenderfeature");
            VolumeStack volumes = VolumeManager.instance.stack;
            CustomPostScreenTint tintData = volumes.GetComponent<CustomPostScreenTint>();
            if (tintData.IsActive())
            {
                _mat.SetColor("_OverlayColor", (Color)tintData.tintColor);
                _mat.SetFloat("_Intensity", (float)tintData.tintIntensity);
                Blit(commandBuffer, scr, tint, _mat, 0);
                Blit(commandBuffer, tint, scr);
            }

            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tintID);
        }

    }

}
