using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class ScreenScanFeature : ScriptableRendererFeature
{

    class ScreenCustomRenderPass : ScriptableRenderPass
    {
        public Material material;
        public RenderTargetIdentifier source;
        private RenderTargetHandle temp_render_target;
        private string profilerTag;

        public ScreenCustomRenderPass(Material material, string tag)
        {
            this.material = material;
            this.profilerTag = tag;
            temp_render_target.Init("_TemporaryColorTexture");
        }
        public void Setup(RenderTargetIdentifier source)
        {
            this.source = source;
        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ConfigureInput(ScriptableRenderPassInput.Depth);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!material)
                return;

            Camera cam = renderingData.cameraData.camera;
            Matrix4x4 mtx_view_inv = cam.worldToCameraMatrix.inverse;
            Matrix4x4 mtx_proj_inv = cam.projectionMatrix.inverse;

            material.SetMatrix("_mtx_view_inv", mtx_view_inv);
            material.SetMatrix("_mtx_proj_inv", mtx_proj_inv);

            Vector4 box_min = new Vector4(-15, -1000, -50, 0);
            Vector4 box_max = new Vector4(15, 1000, 50, 0);
            material.SetVector("_scan_box_min", box_min);
            material.SetVector("_scan_box_max", box_max);
            material.SetVector("_scan_color", new Vector4(179.0f / 255.0f, 224.0f / 255.0f, 230.0f / 255.0f, 1.0f));

            Matrix4x4 box_mtx = Matrix4x4.Rotate(Quaternion.Euler(0, 45 - 30 * Time.time, 0)) * Matrix4x4.Translate(new Vector3(-400, 0, -380));
            material.SetMatrix("_scan_box_world_mtx", box_mtx);

            const string CommandBufferTag = "Screen Scan Pass";
            var cmd = CommandBufferPool.Get(CommandBufferTag);

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(temp_render_target.id, opaqueDesc);

            cmd.Blit(source, temp_render_target.Identifier(), material);
            cmd.Blit(temp_render_target.Identifier(), source);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
            cmd.ReleaseTemporaryRT(temp_render_target.id);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            if (cmd == null)
                throw new System.ArgumentNullException("cmd");
            cmd.ReleaseTemporaryRT(temp_render_target.id);
        }
    }

    public class ScreenScannerSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        public Material material = null;
    }
    public ScreenScannerSettings settings = new ScreenScannerSettings();
    private ScreenCustomRenderPass m_ScriptablePass;
    public override void Create()
    {
        m_ScriptablePass = new ScreenCustomRenderPass(settings.material, name);
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;

    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {

        renderer.EnqueuePass(m_ScriptablePass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        m_ScriptablePass.Setup(renderer.cameraColorTarget);  // use of target after allocation

    }
   
    
      
  

}



