using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
using UnityEngine.Audio;
using System;

public class OptionsManager : MonoBehaviour {

    public PostProcessingProfile profile;
    public AudioMixer Mixer;

    public Slider MusicSlider;
    public Slider EffectsSlider;

    public Dropdown AntiAliasingDropdown;
    public Toggle AmbientOcclusionToggle;
    public Toggle BloomToggle;
    public Toggle ColourGradingToggle;
    public Toggle VignetteToggle;
    public Toggle MotionBlurToggle;
    public Toggle ScreenSpaceReflectionsToggle;

    private EventHandler<EventArgs> _optionsEventhandle;

    public void Awake()
    {
        LoadValues();
        _optionsEventhandle = HideOptionsManager;
    }

    public void OnEnable()
    {
        LoadValues();
        if(Game.CurrentScene != "MainMenu")
        {
            Ui.EscapeEvents.Add(_optionsEventhandle);
        }
    }

    public void HideOptionsMono()
    {
        HideOptionsManager(this, EventArgs.Empty);
    }

    public void HideOptionsManager(object sender, EventArgs e)
    {
        if (Ui.EscapeEvents.Contains(_optionsEventhandle)) Ui.EscapeEvents.Remove(_optionsEventhandle);
        gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        SaveValues();
        LoadValues();
    }

    private void LoadValues()
    {
        //Restore values from config
        Configuration config = Configuration.GetConfigInstance();

        MusicSlider.value = config.MusicVolume;
        SetMusicVol(config.MusicVolume);
        EffectsSlider.value = config.EffectsVolume;
        SetEffectsVol(config.EffectsVolume);

        //AA
        if (config.AntiAliasingEnabled)
        {
            AntialiasingModel.Settings aaset = new AntialiasingModel.Settings();
            aaset.method = config.AAMode;
            profile.antialiasing.settings = aaset;
            AntiAliasingDropdown.value = config.AAMode == AntialiasingModel.Method.Taa ? 1 : 2;
        }
        else
        {
            profile.antialiasing.enabled = false;
            AntiAliasingDropdown.value = 0;
        }

        //Ambient Occlusion
        profile.ambientOcclusion.enabled = config.AmbientOcclusion;
        AmbientOcclusionToggle.isOn = config.AmbientOcclusion;

        //Bloom
        profile.bloom.enabled = config.Bloom;
        BloomToggle.isOn = config.Bloom;

        //Colour Grading
        profile.colorGrading.enabled = config.ColourGrading;
        ColourGradingToggle.isOn = config.ColourGrading;

        //Vignette
        profile.vignette.enabled = config.Vignette;
        VignetteToggle.isOn = config.Vignette;

        //Motion Blur
        profile.motionBlur.enabled = config.MotionBlur;
        MotionBlurToggle.isOn = config.MotionBlur;

        //Screen Space Reflections
        profile.screenSpaceReflection.enabled = config.ScreenSpaceReflections;
        ScreenSpaceReflectionsToggle.isOn = config.ScreenSpaceReflections;
    }

    public void SaveValues()
    {
        Configuration config = Configuration.GetConfigInstance();

        config.MusicVolume = MusicSlider.value;
        config.EffectsVolume = EffectsSlider.value;

        if(AntiAliasingDropdown.value == 0)
        {
            config.AntiAliasingEnabled = false;
        }
        else
        {
            config.AntiAliasingEnabled = true;
            config.AAMode = AntiAliasingDropdown.value == 1 ? AntialiasingModel.Method.Taa : AntialiasingModel.Method.Fxaa;
        }

        config.AmbientOcclusion = AmbientOcclusionToggle.isOn;
        config.Bloom = BloomToggle.isOn;
        config.ColourGrading = ColourGradingToggle.isOn;
        config.Vignette = VignetteToggle.isOn;
        config.MotionBlur = MotionBlurToggle.isOn;
        config.ScreenSpaceReflections = ScreenSpaceReflectionsToggle.isOn;

        Configuration.SaveConfig();
    }

    public void SetMusicVol(float val)
    {
        Mixer.SetFloat("MusicVol", val);
    }

    public void SetEffectsVol(float val)
    {
        Mixer.SetFloat("EffectsVol", val);
    }
}
