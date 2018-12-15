using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using _Scripts._External;

namespace _Scripts
{
    public class LightBarController : MonoBehaviour
    {
        private const string ONSET_DETECTION = "onset";
        private const string SUBBASE_THRESHOLD = "sub-base-threshold";
        private const string BASS_THRESHOLD = "bass-threshold";
        private const string LOW_MID_THRESHOLD = "low-mid-threshold";
        private const string FADE_OUT_DURATION = "fade-out";

        public bool UseOnsetAnalyser = true;
        public float ThresholdSubBase = 0.6f;
        public float ThresholdBass = 0.6f;
        public float ThresholdLowMid = 0.6f;
        private AudioSource _audioSource;
        private Renderer[] _renderers = new Renderer[30];
        private Light[] _lights = new Light[30];
        public float FadeParam = 1;
        private static float t = 0.0f;
        private float _startColor = 40 / 360;
        private float _targetColor = 16 / 360;

        // Use this for initialization
        void Start()
        {
            int offset = -15;
            for (int i = 0; i < 30; i++)
            {
                int onset = 0;
                GameObject lightbar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                lightbar.AddComponent<Light>();
                _lights[i] = lightbar.GetComponent<Light>();
                _lights[i].type = LightType.Point;
                _lights[i].renderMode = LightRenderMode.ForcePixel;
                _lights[i].intensity = 0;
                _lights[i].bounceIntensity = 0;
                _lights[i].range = 3;
                _renderers[i] = lightbar.GetComponent<Renderer>();
                _renderers[i].material.EnableKeyword("_EMISSION");
                _renderers[i].material.SetColor("_EmissionColor", Color.HSVToRGB(1, 1, 0));
                lightbar.transform.parent = transform;
                lightbar.transform.position = new Vector3(offset + i, 0.5f, -1);
                if (PlayerPrefs.HasKey(ONSET_DETECTION))
                {
                    onset = PlayerPrefs.GetInt(ONSET_DETECTION);
                }

                if (onset == 1)
                {
                    UseOnsetAnalyser = true;
                }
                else if (PlayerPrefs.HasKey(SUBBASE_THRESHOLD) &&
                         PlayerPrefs.HasKey(BASS_THRESHOLD) &&
                         PlayerPrefs.HasKey(LOW_MID_THRESHOLD) &&
                         PlayerPrefs.HasKey(FADE_OUT_DURATION))
                {
                    ThresholdSubBase = PlayerPrefs.GetFloat(SUBBASE_THRESHOLD);
                    ThresholdBass = PlayerPrefs.GetFloat(BASS_THRESHOLD);
                    ThresholdLowMid = PlayerPrefs.GetFloat(LOW_MID_THRESHOLD);
                    FadeParam = PlayerPrefs.GetFloat(FADE_OUT_DURATION);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            ChangeRange();
            if (t < 1)
            {
                t += Time.deltaTime * FadeParam;
            }

            if (!UseOnsetAnalyser)
            {
                if (Audio.AudioBandBuffer[0] > ThresholdSubBase ||
                    Audio.AudioBandBuffer[1] > ThresholdBass ||
                    Audio.AudioBandBuffer[2] > ThresholdLowMid)
                {
                    FireUp();
                }
                else
                {
                    FadeOut();
                }
            }
        }

        private void FadeOut()
        {
            float hue, saturation, value, intensity, bounceIntensity;
            hue = Mathf.Lerp(_startColor, _targetColor, t);
            saturation = Mathf.Lerp(0, 1, t);
            value = Mathf.Lerp(1, 0, t);
            intensity = Mathf.Lerp(1.25f, 0, t);
            bounceIntensity = Mathf.Lerp(1.1f, 0, t);
            for (int i = 0; i < 30; i++)
            {
                _renderers[i].material.SetColor("_EmissionColor", Color.HSVToRGB(hue, saturation, value));
                _lights[i].color = Color.HSVToRGB(hue, saturation, value);
                _lights[i].intensity = intensity;
                _lights[i].bounceIntensity = bounceIntensity;
            }
        }

        private void FireUp()
        {
            for (int i = 0; i < 30; i++)
            {
                _renderers[i].material.SetColor("_EmissionColor", Color.HSVToRGB(_startColor, 0, 1));
                _lights[i].color = Color.HSVToRGB(_startColor, 0, 1);
                _lights[i].intensity = 1.25f;
                _lights[i].bounceIntensity = 1.1f;
            }

            t = 0;
        }

        public void UpdateState(List<SpectralFluxInfo> samples, int index = -1)
        {
            if (UseOnsetAnalyser)
            {
                int windowStart = 0;
                int windowEnd = 0;

                if (index > 0)
                {
                    windowStart = Mathf.Max(0, index - 300 / 2);
                    windowEnd = Mathf.Min(index + 300 / 2, samples.Count - 1);
                }
                else
                {
                    windowStart = Mathf.Max(0, samples.Count - 300 - 1);
                    windowEnd = Mathf.Min(windowStart + 300, samples.Count);
                }

                for (int i = windowStart; i < windowEnd; i++)
                {
                    if (samples[i].isPeak)
                    {
                        FireUp();
                    }
                    else
                    {
                        FadeOut();
                    }
                }
            }
        }

        void ChangeRange()
        {
            for (int i = 0; i < 30; i++)
            {
                _lights[i].range += Input.GetAxis("Vertical");   
                if (_lights[i].range < 0)
                {
                    _lights[i].range = 0;
                }
            }
        }
    }
}