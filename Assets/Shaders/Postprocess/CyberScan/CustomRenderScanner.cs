using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class CustomRenderScanner : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        private Material material;
        private RenderTargetIdentifier source;
        private RenderTargetHandle tempTexture;

        public CustomRenderPass(Material material)
        {
            this.material = material;
            tempTexture.Init("_TemporaryColorTexture");
        }

        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (material == null)
            {
                Debug.LogWarningFormat("Missing material. {0} render pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
                return;
            }

            CommandBuffer cmd = CommandBufferPool.Get("CustomRenderPass");

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            RenderTargetIdentifier destination = tempTexture.Identifier();

            cmd.GetTemporaryRT(tempTexture.id, opaqueDesc);
            Blit(cmd, source, destination);

            // Aquí podrías aplicar tu shader personalizado que genera el efecto de pantalla cyan con ruido
            cmd.SetGlobalTexture("_MainTex", destination);
            cmd.Blit(destination, source, material);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTexture.id);
        }
    }

    [System.Serializable]
    public class CustomRenderPassSettings
    {
        public Material material = null;
    }

    public CustomRenderPassSettings settings = new CustomRenderPassSettings();
    CustomRenderPass customRenderPass;

    public override void Create()
    {
        customRenderPass = new CustomRenderPass(settings.material);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(customRenderPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        customRenderPass.Setup(renderer.cameraColorTarget);
    }
}