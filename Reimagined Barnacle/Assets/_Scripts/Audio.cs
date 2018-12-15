using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    public class Audio : MonoBehaviour
    {
        private AudioSource _audioSource;
        private float[] _samplesLeft = new float[512];
        private float[] _samplesRight = new float[512];
        private float[] _frequencyBand = new float[8];
        private float[] _bandBuffer = new float[8];
        private float[] _bufferDecrease = new float[8];

        float[] _freqBandHighest = new float[8];
        public static float[] AudioBand = new float[8];
        public static float[] AudioBandBuffer = new float[8];

        public AudioClip[] AudioClips;

        public static float Amplitude, AmplitudeBuffer;
        private float _amplitudeHighest;
        private int _activeClip;

        public int sampleRate;

        // Use this for initialization
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            if (AudioClips.Length > 0)
            {
                if (PlayerPrefs.HasKey("song"))
                {
                    _activeClip = PlayerPrefs.GetInt("song");
                    _audioSource.clip = AudioClips[_activeClip];
                }
            }

            InitialiseFreq();
        }

        // Update is called once per frame
        void Update()
        {
            GetSpectrumData();
            MakeFrequencyBand();
            BandBufferMethod();
            CreateAudioBands();
            CalculateAmplitude();
            ChangeTrack();
            Quit();
            sampleRate = AudioSettings.outputSampleRate;
        }

        void GetSpectrumData()
        {
            _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.BlackmanHarris);
            _audioSource.GetSpectrumData(_samplesRight, 1, FFTWindow.BlackmanHarris);
        }

        void InitialiseFreq()
        {
            for (int i = 0; i < 8; i++)
            {
                _freqBandHighest[i] = 1;
            }
        }

        void BandBufferMethod()
        {
            for (int i = 0; i < 8; i++)
            {
                if (_frequencyBand[i] > _bandBuffer[i])
                {
                    _bandBuffer[i] = _frequencyBand[i];
                    _bufferDecrease[i] = 0.005f;
                }

                if (_frequencyBand[i] < _bandBuffer[i])
                {
                    _bandBuffer[i] -= _bufferDecrease[i];
                    _bufferDecrease[i] *= 1.2f;
                }
            }
        }

        void MakeFrequencyBand()
        {
            int count = 0;

            for (int i = 0; i < 8; i++)
            {
                float average = 0;
                int sampleCount = (int) Mathf.Pow(2, i) * 2;
                if (i == 7)
                {
                    sampleCount += 2;
                }

                for (int j = 0; j < sampleCount; j++)
                {
                    average += _samplesLeft[count] + _samplesRight[count] * (count + 1);
                    count++;
                }

                average /= count;

                _frequencyBand[i] = average * 10;
            }
        }

        void CalculateAmplitude()
        {
            float currentAmp = 0;
            float currentBuffer = 0;
            for (int i = 0; i < 8; i++)
            {
                currentAmp = AudioBand[i];
                currentBuffer = AudioBandBuffer[i];
            }

            if (currentAmp > _amplitudeHighest)
            {
                _amplitudeHighest = currentAmp;
            }

            Amplitude = currentAmp / _amplitudeHighest;
            AmplitudeBuffer = currentBuffer / _amplitudeHighest;
        }

        void CreateAudioBands()
        {
            for (int i = 0; i < 8; i++)
            {
                if (_frequencyBand[i] > _freqBandHighest[i])
                {
                    _freqBandHighest[i] = _frequencyBand[i];
                }

                AudioBand[i] = _frequencyBand[i] / _freqBandHighest[i];
                AudioBandBuffer[i] = _bandBuffer[i] / _freqBandHighest[i];
            }
        }

        void ChangeTrack()
        {
            bool changed = false;
            if (Application.platform == RuntimePlatform.OSXPlayer &&
                Input.GetButtonDown(KeyCode.JoystickButton16.ToString()) ||
                Application.platform == RuntimePlatform.WindowsPlayer &&
                Input.GetButtonDown(KeyCode.JoystickButton0.ToString()))
            {
                _activeClip += 1 % AudioClips.Length;
                changed = true;
            } else if (Application.platform == RuntimePlatform.OSXPlayer &&
                       Input.GetButtonDown(KeyCode.JoystickButton17.ToString()) ||
                       Application.platform == RuntimePlatform.WindowsPlayer &&
                       Input.GetButtonDown(KeyCode.JoystickButton1.ToString()))
            {
                _activeClip -= 1 % AudioClips.Length;
                changed = true;
            }

            if (changed)
            {
                _audioSource.clip = AudioClips[_activeClip];
                _audioSource.Play();
            }
        }

        void Quit()
        {
            if (Input.GetButton("Escape"))
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }
}