using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorDispersionRenderer : ScriptableRendererFeature
{
    [System.Serializable]
    public class ColorDispersionSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Material material = null;
    }

    public ColorDispersionSettings settings = new ColorDispersionSettings();
    private ColorDispersionRenderPass m_ScriptablePass;

    public override void Create()
    {
        m_ScriptablePass = new ColorDispersionRenderPass(settings.material, name);
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass); // letting the renderer know which passes will be used before allocation
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        m_ScriptablePass.Setup(renderer.cameraColorTarget);  // use of target after allocation
    }

    //public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    //{
    //    renderPass.Setup(renderer.cameraColorTarget);
    //    renderer.EnqueuePass(renderPass);
    //}

    class ColorDispersionRenderPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle temporaryRT;
        private string profilerTag;

        public ColorDispersionRenderPass(Material material, string tag)
        {
            this.material = material;
            this.profilerTag = tag;
            temporaryRT.Init("_TemporaryColorTexture");
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(temporaryRT.id, cameraTextureDescriptor);
            ConfigureTarget(temporaryRT.Identifier());
            ConfigureClear(ClearFlag.None, Color.clear);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
            {
                Debug.LogWarningFormat("Missing material. {0} render pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
            var stack = VolumeManager.instance.stack;
            var colorDispersionEffect = stack.GetComponent<ColorDispersionEffect>();

            if (colorDispersionEffect == null || !colorDispersionEffect.IsActive())
            {
                CommandBufferPool.Release(cmd);
                return;
            }

            material.SetFloat("_ColorDispersionStrength", colorDispersionEffect.dispersionStrength.value);
            material.SetFloat("_ColorDispersionU", colorDispersionEffect.dispersionU.value);
            material.SetFloat("_ColorDispersionV", colorDispersionEffect.dispersionV.value);
            material.SetFloat("_BlackWhiteThreshold", colorDispersionEffect.blackWhiteThreshold.value);
            material.SetFloat("_BlackWhiteWidth", colorDispersionEffect.blackWhiteWidth.value);
            material.SetColor("_BlackWhiteWhiteColor", colorDispersionEffect.blackWhiteWhiteColor.value);
            material.SetColor("_BlackWhiteBlackColor", colorDispersionEffect.blackWhiteBlackColor.value);
            material.SetFloat("_EnableBlackWhite", colorDispersionEffect.enableBlackWhite.value ? 1f : 0f);

            Blit(cmd, source, temporaryRT.Identifier(), material);
            Blit(cmd, temporaryRT.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

    

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (temporaryRT != RenderTargetHandle.CameraTarget)
            {
                cmd.ReleaseTemporaryRT(temporaryRT.id);
            }
        }

      

      

    }
}
