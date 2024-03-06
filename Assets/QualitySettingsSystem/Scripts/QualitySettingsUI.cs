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
        [SerializeField] private ToggleComponent _castShadowsAddLights;
        [SerializeField] private DropdownComponent _shadowAtlasResolution;
        
        [Space][Header("Shadows")]
        [SerializeField] private SliderComponent _maxDistance;
        [SerializeField] private SliderComponent _depthBias;
        [SerializeField] private SliderComponent _normalBias;
        [SerializeField] private ToggleComponent _softShadows;
        
        [Space][Header("Post-processing")]
        [SerializeField] private DropdownComponent _gradingMode;
        [SerializeField] private ToggleComponent _fastConvertions;

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
            _castShadowsAddLights.IsOn = _currentAsset.supportsAdditionalLightShadows;
            _shadowAtlasResolution.ChangeValue((ShadowResolution) _currentAsset.additionalLightsShadowmapResolution);

            // Shadows
            _maxDistance.Value = _currentAsset.shadowDistance;
            _depthBias.Value = _currentAsset.shadowDepthBias;
            _normalBias.Value = _currentAsset.shadowNormalBias;
            _softShadows.IsOn = _currentAsset.supportsSoftShadows;

            // Post-Processing
            _gradingMode.ChangeValue(_currentAsset.colorGradingMode);
            _fastConvertions.IsOn = _currentAsset.useFastSRGBLinearConversion;
        }
    }
}