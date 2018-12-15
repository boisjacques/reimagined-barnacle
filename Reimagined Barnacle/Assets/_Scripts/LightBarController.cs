using System.Collections.Generic;
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
        private Renderer _renderer;
        public float FadeParam = 1;
        private static float t = 0.0f;

        // Use this for initialization
        void Start()
        {
            int onset = 0;
            GameObject lightbar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _renderer = lightbar.GetComponent<Renderer>();
            _renderer.material.EnableKeyword("_EMISSION");
            _renderer.material.SetColor("_EmissionColor", Color.HSVToRGB(1, 1, 0));
            lightbar.transform.localScale = new Vector3(30, 1, 1);
            lightbar.transform.parent = transform;
            lightbar.transform.position = new Vector3(0, 0.5f, -1);
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

        // Update is called once per frame
        void Update()
        {
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
            float startColor = 40 / 360;
            float targetColor = 16 / 360;
            float hue, saturation, value;
            hue = Mathf.Lerp(startColor, targetColor, t);
            saturation = Mathf.Lerp(0, 1, t);
            value = Mathf.Lerp(1, 0, t);
            _renderer.material.SetColor("_EmissionColor", Color.HSVToRGB(hue, saturation, value));
        }

        private void FireUp()
        {
            _renderer.material.SetColor("_EmissionColor", Color.HSVToRGB(1, 0, 1));
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
    }
}