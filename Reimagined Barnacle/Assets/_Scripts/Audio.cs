using UnityEngine;

namespace _Scripts
{
    public class Audio : MonoBehaviour
    {
        private AudioSource _audioSource;
        public static float[] Samples = new float[512];
        public static float[] FrequencyBand = new float[8];
        public static float[] BandBuffer = new float[8];
        private float[] _bufferDecrease = new float[8];

        public float[] FrequencyBandDebug;
        public float[] SamplesDebug;

        // Use this for initialization
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            GetSpectrumData();
            MakeFrequencyBand();
            BandBufferMethod();
            FrequencyBandDebug = FrequencyBand;
            SamplesDebug = Samples;
        }

        void GetSpectrumData()
        {
            _audioSource.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
        }

        void BandBufferMethod()
        {
            for (int i = 0; i < 8; i++)
            {
                if (FrequencyBand[i] > BandBuffer[i])
                {
                    BandBuffer[i] = FrequencyBand[i];
                    _bufferDecrease[i] = 0.005f;
                }

                if (FrequencyBand[i] < BandBuffer[i])
                {
                    BandBuffer[i] -= _bufferDecrease[i];
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
                    average += Samples[count] * (count + 1);
                    count++;
                }

                average /= count;

                FrequencyBand[i] = average * 10;
            }
        }
    }
}