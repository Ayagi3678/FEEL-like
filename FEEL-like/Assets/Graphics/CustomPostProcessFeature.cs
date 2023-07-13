using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Graphics
{
    public abstract class CustomPostProcessFeature<T> : ScriptableRendererFeature where T : VolumeComponent,IPostProcessComponent
    {
        [System.Serializable]
        public class CustomPostProcessSettings
        {
            //パスの実行タイミング
            public RenderPassEvent _Event = RenderPassEvent.BeforeRenderingPostProcessing;
            //使用するシェーダー
            public Shader _Shader;
        }
        [SerializeField] private CustomPostProcessSettings settings = new CustomPostProcessSettings();
        delegate void MaterialPropertyHandler(ref RTHandle source,ref CommandBuffer cmd,ref Material material,ref T t);
        private class CustomPostProcessPass : ScriptableRenderPass
        {
            private const string RenderPassName = nameof(CustomPostProcessPass);
            private const string ProfilingSamplerName = "SrcToDest";
            private readonly ProfilingSampler _profilingSampler;
            
            private RTHandle passSource;
            private RTHandle passDestination;

            //Blitに使用するマテリアル
            private Material _material;

            //一時的なレンダーターゲット（パスの入出力が同一の場合、いちど中間バッファを挟んでBlitする必要があるため）
            RTHandle m_TemporaryColorTexture;

            private T _volume = default;

            private MaterialPropertyHandler _materialPropertyHandler;
            
            public CustomPostProcessPass(RenderPassEvent renderPassEvent, Shader shader,MaterialPropertyHandler materialPropertyHandler)
            {
                if (shader == null)
                {
                    return;
                }

                _materialPropertyHandler = materialPropertyHandler;
                
                _profilingSampler = new ProfilingSampler(ProfilingSamplerName);
                this.renderPassEvent = renderPassEvent;
                // マテリアルを作成
                _material = CoreUtils.CreateEngineMaterial(shader);
                
                m_TemporaryColorTexture=RTHandles.Alloc(k_CameraTarget, name: "_TemporaryColorTexture");
            }
            public void Setup(RTHandle source, RTHandle destination,in RenderingData renderingData)
            {
                this.passSource = source;
                this.passDestination = destination;

                var volumeStack = VolumeManager.instance?.stack;

                if (volumeStack != null) _volume = volumeStack.GetComponent<T>();
            }
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (_material == null) return;
                if (!renderingData.cameraData.postProcessEnabled) return;
                
                RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
                opaqueDesc.depthBufferBits = 0;
                
                var cmd = CommandBufferPool.Get(RenderPassName);
                
                Render(cmd, ref renderingData, opaqueDesc);
                
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            private void Render(CommandBuffer cmd, ref RenderingData renderingData, RenderTextureDescriptor opaqueDesc)
            {

                cmd.GetTemporaryRT(m_TemporaryColorTexture.GetInstanceID(), opaqueDesc, FilterMode.Bilinear);
                
                var cameraData = renderingData.cameraData;
                
                using (new ProfilingScope(cmd, _profilingSampler))
                {
                    
                    _materialPropertyHandler.Invoke(ref passSource,ref cmd,ref _material,ref _volume);


                    DoColorOverlay(cmd, passSource, m_TemporaryColorTexture, opaqueDesc);
                    // 元のテクスチャから一時的なテクスチャにエフェクトを適用しつつ描画
                    
                }

                cmd.Blit(m_TemporaryColorTexture.nameID,
                    passDestination == k_CameraTarget ? passSource : passDestination.nameID);
            }
            private void DoColorOverlay(CommandBuffer cmd, RenderTargetIdentifier source, RTHandle destination,
                RenderTextureDescriptor opaqueDesc)
            {
                cmd.Blit( source, destination, _material, 0);
            }
            public override void FrameCleanup(CommandBuffer cmd)
            {
                if (passDestination == k_CameraTarget)
                    cmd.ReleaseTemporaryRT(m_TemporaryColorTexture.GetInstanceID());
            }
            
        }
        
        private CustomPostProcessPass _scriptablePass;
        
        /// <inheritdoc/>
        public override void Create()
        {
            _scriptablePass = new(settings._Event,settings._Shader,SetMaterialProperty);
        }

        protected abstract void SetMaterialProperty(ref RTHandle source,ref CommandBuffer cmd,ref Material material,ref T t);

        // Here you can inject one or multiple render passes in the renderer.
        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_scriptablePass);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            _scriptablePass.Setup(renderer.cameraColorTargetHandle,renderer.cameraColorTargetHandle,renderingData);
        }
    }
}