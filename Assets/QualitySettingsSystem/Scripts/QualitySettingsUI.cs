using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using ShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution;

namespace QualitySettings
{
    public class QualitySettingsUI : MonoBehaviour
    {
        [Header("Rendering")]
        [SerializeField] private Toggle _depthTexture;
        [SerializeField] private Toggle _opaqueTexture;
        [SerializeField] private DropdownComponent _opaqueDownsampling;
        
        [Space][Header("Quality")]
        [SerializeField] private Toggle _hdr;
        [SerializeField] private DropdownComponent _antiAliasing;
        [SerializeField] private SliderComponent _renderScale;
        [SerializeField] private DropdownComponent _upscalingFilter;
        
        [Space][Header("Lighting")]
        [SerializeField] private DropdownComponent _mainLight;
        [SerializeField] private Toggle _castShadows;
        [SerializeField] private DropdownComponent _shadowResolution;
        
        [SerializeField] private DropdownComponent _additionalLights;
        [SerializeField] private Toggle _castShadowsAddLights;
        [SerializeField] private DropdownComponent _shadowAtlasResolution;
        
        [Space][Header("Shadows")]
        [SerializeField] private SliderComponent _maxDistance;
        [SerializeField] private SliderComponent _depthBias;
        [SerializeField] private SliderComponent _normalBias;
        [SerializeField] private Toggle _softShadows;
        
        [Space][Header("Post-processing")]
        [SerializeField] private DropdownComponent _gradingMode;
        [SerializeField] private Toggle _fastConvertions;

        [Space] [Header("Managing buttons")] 
        [SerializeField] private Button _saveBtn;
        [SerializeField] private Button _discardBtn;
        
        private UniversalRenderPipelineAsset _currentAsset;

        private void Awake()
        {
            InitializeValues();
            UpdateRenderPipelineAsset();
        }

        private void OnDestroy()
        {
            _saveBtn.onClick.RemoveListener(OnApplyClickBtn);
            _discardBtn.onClick.RemoveListener(OnDiscardClickBtn);
        }

        private void InitializeValues()
        {
            _opaqueDownsampling.InitializeValues<Downsampling>();
            _antiAliasing.InitializeValues<MsaaQuality>();
            _upscalingFilter.InitializeValues<UpscalingFilterSelection>();

            _mainLight.InitializeValues<LightRenderingMode>();
            _shadowResolution.InitializeValues<ShadowResolution>();
            _additionalLights.InitializeValues<LightRenderingMode>();
            _shadowAtlasResolution.InitializeValues<ShadowResolution>();
            _gradingMode.InitializeValues<ColorGradingMode>();
            
            _saveBtn.onClick.AddListener(OnApplyClickBtn);
            _discardBtn.onClick.AddListener(OnDiscardClickBtn);
        }

        private void OnApplyClickBtn()
        {
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
            // Rendering
            _depthTexture.isOn = _currentAsset.supportsCameraDepthTexture;
            _opaqueTexture.isOn = _currentAsset.supportsCameraOpaqueTexture;
            _opaqueDownsampling.ChangeValue(_currentAsset.opaqueDownsampling);
            
            // Quality
            _hdr.isOn = _currentAsset.supportsHDR;
            _antiAliasing.ChangeValue((MsaaQuality) _currentAsset.msaaSampleCount);
            _renderScale.value = _currentAsset.renderScale;
            _upscalingFilter.ChangeValue(_currentAsset.upscalingFilter);

            // Lighting
            _mainLight.ChangeValue(_currentAsset.mainLightRenderingMode);
            _castShadows.isOn = _currentAsset.supportsMainLightShadows;
            _shadowResolution.ChangeValue((ShadowResolution) _currentAsset.mainLightShadowmapResolution);
            _additionalLights.ChangeValue(_currentAsset.additionalLightsRenderingMode);
            _castShadowsAddLights.isOn = _currentAsset.supportsAdditionalLightShadows;
            _shadowAtlasResolution.ChangeValue((ShadowResolution) _currentAsset.additionalLightsShadowmapResolution);

            // Shadows
            _maxDistance.value = _currentAsset.shadowDistance;
            _depthBias.value = _currentAsset.shadowDepthBias;
            _normalBias.value = _currentAsset.shadowNormalBias;
            _softShadows.isOn = _currentAsset.supportsSoftShadows;

            // Post-Processing
            _gradingMode.ChangeValue(_currentAsset.colorGradingMode);
            _fastConvertions.isOn = _currentAsset.useFastSRGBLinearConversion;
        }
    }
}