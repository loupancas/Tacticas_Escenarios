using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorDispersionRenderer : ScriptableRendererFeature
{
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
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            temporaryRT.Init("_TemporaryColorTexture");
            cmd.GetTemporaryRT(temporaryRT.id, cameraTextureDescriptor);
            ConfigureTarget(temporaryRT.Identifier());
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
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
            cmd.ReleaseTemporaryRT(temporaryRT.id);
        }
    }

    [System.Serializable]
    public class ColorDispersionSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Material material = null;
    }

    public ColorDispersionSettings settings = new ColorDispersionSettings();
    private ColorDispersionRenderPass renderPass;

    public override void Create()
    {
        renderPass = new ColorDispersionRenderPass(settings.material, name);
        renderPass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderPass.Setup(renderer.cameraColorTarget);
        renderer.EnqueuePass(renderPass);
    }
}
