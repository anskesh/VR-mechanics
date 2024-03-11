using System;
using QualitySettings.UIComponents;
using QualitySettings.Utility;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;

namespace QualitySettings
{
    public class QualitySettingsUI : MonoBehaviour
    {
        [Header("Rendering")]
        [SerializeField] private ToggleComponent _depthTextureToggle;
        [SerializeField] private ToggleComponent _opaqueTextureToggle;
        [SerializeField] private EnumDropdownComponent _opaqueDownsamplingDropdown;
        
        [Space]
        [Header("Quality")]
        [SerializeField] private ToggleComponent _hdrToggle;
        [SerializeField] private EnumDropdownComponent _antiAliasingDropdown;
        [SerializeField] private SliderComponent _renderScale;
        [SerializeField] private EnumDropdownComponent _upscalingFilterDropdown;
        
        [Space]
        [Header("Lighting")]
        [SerializeField] private EnumDropdownComponent _mainLightEnumDropdown;
        [SerializeField] private ToggleComponent _castShadowsToggle;
        [SerializeField] private EnumDropdownComponent _shadowResolutionDropdown;
        [Space]
        [SerializeField] private EnumDropdownComponent _additionalLightsDropdown;
        [SerializeField] private SliderComponent _perObjectLimitSlider;
        [SerializeField] private ToggleComponent _castShadowsAddLightsToggle;
        [SerializeField] private EnumDropdownComponent _shadowAtlasResolutionDropdown;
        
        [Space]
        [Header("Shadows")]
        [SerializeField] private SliderComponent _maxDistanceSlider;
        [SerializeField] private SliderComponent _cascadeCountSlider;
        [SerializeField] private SliderComponent _depthBiasSlider;
        [SerializeField] private SliderComponent _normalBiasSlider;
        [SerializeField] private ToggleComponent _softShadowsToggle;
        
        [Space]
        [Header("Post-processing")]
        [SerializeField] private EnumDropdownComponent _gradingModeDropdown;
        [SerializeField] private ToggleComponent _fastConvertionsToggle;
        
        [Space]
        [Header("Managing buttons")] 
        [SerializeField] private Button _saveBtn;
        [SerializeField] private Button _discardBtn;
        
        private UniversalRenderPipelineAsset _currentAsset;

        private void Awake()
        {
            _saveBtn.onClick.AddListener(SetValues);
            _discardBtn.onClick.AddListener(ResetValues);
            
            UpdateRenderPipelineAsset();
        }

        private void OnDestroy()
        {
            _saveBtn.onClick.RemoveListener(SetValues);
            _discardBtn.onClick.RemoveListener(ResetValues);
        }
        
        private void UpdateRenderPipelineAsset()
        {
            _currentAsset = UnityEngine.QualitySettings.renderPipeline as UniversalRenderPipelineAsset;

            if (!_currentAsset)
                throw new Exception("Universal render pipeline doesn't selected.");
            
            ResetValues();
        }

        private void ResetValues()
        {
            // Rendering
            _depthTextureToggle.IsOn = _currentAsset.supportsCameraDepthTexture;
            _opaqueTextureToggle.IsOn = _currentAsset.supportsCameraOpaqueTexture;
            _opaqueDownsamplingDropdown.ChangeValue(_currentAsset.opaqueDownsampling);
            
            // Quality
            _hdrToggle.IsOn = _currentAsset.supportsHDR;
            _antiAliasingDropdown.ChangeValue((MsaaQuality) _currentAsset.msaaSampleCount);
            _renderScale.Value = _currentAsset.renderScale;
            _upscalingFilterDropdown.ChangeValue(_currentAsset.upscalingFilter);

            // Lighting
            _mainLightEnumDropdown.ChangeValue(_currentAsset.mainLightRenderingMode);
            _castShadowsToggle.IsOn = _currentAsset.supportsMainLightShadows;
            _shadowResolutionDropdown.ChangeValue((ShadowResolution) _currentAsset.mainLightShadowmapResolution);
            _additionalLightsDropdown.ChangeValue(_currentAsset.additionalLightsRenderingMode);
            _perObjectLimitSlider.ChangeMinMax(0, UniversalRenderPipeline.maxPerObjectLights);
            _perObjectLimitSlider.Value = _currentAsset.maxAdditionalLightsCount;
            _castShadowsAddLightsToggle.IsOn = _currentAsset.supportsAdditionalLightShadows;
            _shadowAtlasResolutionDropdown.ChangeValue((ShadowResolution) _currentAsset.additionalLightsShadowmapResolution);

            // Shadows
            _maxDistanceSlider.Value = _currentAsset.shadowDistance;
            _cascadeCountSlider.Value = _currentAsset.shadowCascadeCount;
            _depthBiasSlider.Value = _currentAsset.shadowDepthBias;
            _normalBiasSlider.Value = _currentAsset.shadowNormalBias;
            _softShadowsToggle.IsOn = _currentAsset.supportsSoftShadows;

            // Post-Processing
            _gradingModeDropdown.ChangeValue(_currentAsset.colorGradingMode);
            _fastConvertionsToggle.IsOn = _currentAsset.useFastSRGBLinearConversion;
        }

        private void SetValues()
        {
            // Rendering
            _currentAsset.supportsCameraDepthTexture = _depthTextureToggle.IsOn;
            _currentAsset.supportsCameraOpaqueTexture = _opaqueTextureToggle.IsOn;
            _currentAsset.SetFieldValue("m_OpaqueDownsampling", _opaqueDownsamplingDropdown.GetEnumValue<Downsampling>());
            
            // Quality
            _currentAsset.supportsHDR = _hdrToggle.IsOn;
            _currentAsset.msaaSampleCount = _antiAliasingDropdown.GetIntValue();
            _currentAsset.renderScale = _renderScale.Value;
            _currentAsset.upscalingFilter = _upscalingFilterDropdown.GetEnumValue<UpscalingFilterSelection>();

            // Lighting
            _currentAsset.SetPropertyValue("mainLightRenderingMode", _mainLightEnumDropdown.GetEnumValue<LightRenderingMode>());
            _currentAsset.SetPropertyValue("supportsMainLightShadows", _castShadowsToggle.IsOn);
            _currentAsset.SetPropertyValue("mainLightShadowmapResolution", _shadowResolutionDropdown.GetIntValue());
            _currentAsset.SetPropertyValue("additionalLightsRenderingMode", _additionalLightsDropdown.GetEnumValue<LightRenderingMode>());
            _currentAsset.SetPropertyValue("maxAdditionalLightsCount", (int) _perObjectLimitSlider.Value);
            _currentAsset.SetPropertyValue("supportsAdditionalLightShadows", _castShadowsAddLightsToggle.IsOn);
            _currentAsset.SetPropertyValue("additionalLightsShadowmapResolution", _shadowAtlasResolutionDropdown.GetIntValue());

            // Shadows
            _currentAsset.shadowDistance = _maxDistanceSlider.Value;
            _currentAsset.shadowCascadeCount = (int) _cascadeCountSlider.Value;
            _currentAsset.shadowDepthBias = _depthBiasSlider.Value;
            _currentAsset.shadowNormalBias = _normalBiasSlider.Value;
            _currentAsset.SetPropertyValue("supportsSoftShadows", _softShadowsToggle.IsOn);;

            // Post-Processing
            _currentAsset.colorGradingMode = _gradingModeDropdown.GetEnumValue<ColorGradingMode>();
            _currentAsset.SetFieldValue("m_UseFastSRGBLinearConversion", _fastConvertionsToggle.IsOn);
        }
    }
}