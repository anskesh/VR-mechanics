using QualitySettings.UIComponents;
using QualitySettings.Utility;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;

namespace QualitySettings
{
    public class QualitySettingsUI : MonoBehaviour
    {
        [Header("Rendering")]
        [SerializeField] private ToggleComponent _depthTexture;
        [SerializeField] private ToggleComponent _opaqueTexture;
        [SerializeField] private DropdownComponent _opaqueDownsampling;
        
        [Space][Header("Quality")]
        [SerializeField] private ToggleComponent _hdr;
        [SerializeField] private DropdownComponent _antiAliasing;
        [SerializeField] private SliderComponent _renderScale;
        [SerializeField] private DropdownComponent _upscalingFilter;
        
        [Space][Header("Lighting")]
        [SerializeField] private DropdownComponent _mainLight;
        [SerializeField] private ToggleComponent _castShadows;
        [SerializeField] private DropdownComponent _shadowResolution;
        
        [SerializeField] private DropdownComponent _additionalLights;
        [SerializeField] private SliderComponent _perObjectLimit;
        [SerializeField] private ToggleComponent _castShadowsAddLights;
        [SerializeField] private DropdownComponent _shadowAtlasResolution;
        
        [Space][Header("Shadows")]
        [SerializeField] private SliderComponent _maxDistance;
        [SerializeField] private SliderComponent _cascadeCount;
        [SerializeField] private SliderComponent _depthBias;
        [SerializeField] private SliderComponent _normalBias;
        [SerializeField] private ToggleComponent _softShadows;
        
        [Space][Header("Post-processing")]
        [SerializeField] private DropdownComponent _gradingMode;
        [SerializeField] private ToggleComponent _fastConvertions;

        [Space][Header("Managing buttons")] 
        [SerializeField] private Button _saveBtn;
        [SerializeField] private Button _discardBtn;
        
        private UniversalRenderPipelineAsset _currentAsset;

        private void Awake()
        {
            _saveBtn.onClick.AddListener(OnApplyClickBtn);
            _discardBtn.onClick.AddListener(OnDiscardClickBtn);
            
            UpdateRenderPipelineAsset();
        }

        private void OnDestroy()
        {
            _saveBtn.onClick.RemoveListener(OnApplyClickBtn);
            _discardBtn.onClick.RemoveListener(OnDiscardClickBtn);
        }

        private void OnApplyClickBtn()
        {
            SetValues();
        }

        private void OnDiscardClickBtn()
        {
            UpdateDefaultValues();
        }
        
        private void UpdateRenderPipelineAsset()
        {
            _currentAsset = UnityEngine.QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
            UpdateDefaultValues();
        }

        private void UpdateDefaultValues()
        {
            if (!_currentAsset)
                return;
            
            // Rendering
            _depthTexture.IsOn = _currentAsset.supportsCameraDepthTexture;
            _opaqueTexture.IsOn = _currentAsset.supportsCameraOpaqueTexture;
            _opaqueDownsampling.ChangeValue(_currentAsset.opaqueDownsampling);
            
            // Quality
            _hdr.IsOn = _currentAsset.supportsHDR;
            _antiAliasing.ChangeValue((MsaaQuality) _currentAsset.msaaSampleCount);
            _renderScale.Value = _currentAsset.renderScale;
            _upscalingFilter.ChangeValue(_currentAsset.upscalingFilter);

            // Lighting
            _mainLight.ChangeValue(_currentAsset.mainLightRenderingMode);
            _castShadows.IsOn = _currentAsset.supportsMainLightShadows;
            _shadowResolution.ChangeValue((ShadowResolution) _currentAsset.mainLightShadowmapResolution);
            _additionalLights.ChangeValue(_currentAsset.additionalLightsRenderingMode);
            _perObjectLimit.Value = _currentAsset.maxAdditionalLightsCount;
            _castShadowsAddLights.IsOn = _currentAsset.supportsAdditionalLightShadows;
            _shadowAtlasResolution.ChangeValue((ShadowResolution) _currentAsset.additionalLightsShadowmapResolution);

            // Shadows
            _maxDistance.Value = _currentAsset.shadowDistance;
            _cascadeCount.Value = _currentAsset.shadowCascadeCount;
            _depthBias.Value = _currentAsset.shadowDepthBias;
            _normalBias.Value = _currentAsset.shadowNormalBias;
            _softShadows.IsOn = _currentAsset.supportsSoftShadows;

            // Post-Processing
            _gradingMode.ChangeValue(_currentAsset.colorGradingMode);
            _fastConvertions.IsOn = _currentAsset.useFastSRGBLinearConversion;
        }

        private void SetValues()
        {
            if (!_currentAsset)
                return;
            
            // Rendering
            _currentAsset.supportsCameraDepthTexture = _depthTexture.IsOn;
            _currentAsset.supportsCameraOpaqueTexture = _opaqueTexture.IsOn;
            _currentAsset.SetFieldValue("m_OpaqueDownsampling", (Downsampling) _opaqueDownsampling.GetEnum());
            
            // Quality
            _currentAsset.supportsHDR = _hdr.IsOn;
            _currentAsset.msaaSampleCount = _antiAliasing.GetValue();
            _currentAsset.renderScale = _renderScale.Value;
            _currentAsset.upscalingFilter = (UpscalingFilterSelection) _upscalingFilter.GetEnum();

            // Lighting
            _currentAsset.SetPropertyValue("mainLightRenderingMode", (LightRenderingMode) _mainLight.GetEnum());
            _currentAsset.SetPropertyValue("supportsMainLightShadows", _castShadows.IsOn);
            _currentAsset.SetPropertyValue("mainLightShadowmapResolution", _shadowResolution.GetValue());
            _currentAsset.SetPropertyValue("additionalLightsRenderingMode", (LightRenderingMode) _additionalLights.GetEnum());
            _currentAsset.SetPropertyValue("maxAdditionalLightsCount", (int) _perObjectLimit.Value);
            _currentAsset.SetPropertyValue("supportsAdditionalLightShadows", _castShadowsAddLights.IsOn);
            _currentAsset.SetPropertyValue("additionalLightsShadowmapResolution", _shadowAtlasResolution.GetValue());

            // Shadows
            _currentAsset.shadowDistance = _maxDistance.Value;
            _currentAsset.shadowCascadeCount = (int) _cascadeCount.Value;
            _currentAsset.shadowDepthBias = _depthBias.Value;
            _currentAsset.shadowNormalBias = _normalBias.Value;
            _currentAsset.SetPropertyValue("supportsSoftShadows", _softShadows.IsOn);;

            // Post-Processing
            _currentAsset.colorGradingMode = (ColorGradingMode) _gradingMode.GetEnum();
            _currentAsset.SetFieldValue("m_UseFastSRGBLinearConversion", _fastConvertions.IsOn);
        }
    }
}