using System.Collections.Generic;
using UnityEngine;
using _Scripts._External;

namespace _Scripts
{
	public class LightBarController : MonoBehaviour
	{
		public bool UseOnsetAnalyser = true;
		public float ThresholdSubBase = 0.6f;
		public float ThresholdBass = 0.6f;
		public float ThresholdLowMid = 0.6f;
		private AudioSource _audioSource;

		// Use this for initialization
		void Start ()
		{
			
		}
	
		// Update is called once per frame
		void Update () {
			if (!UseOnsetAnalyser)
			{
				if (Audio.AudioBandBuffer[0] > ThresholdSubBase ||
				    Audio.AudioBandBuffer[1] > ThresholdBass || 
				    Audio.AudioBandBuffer[2] > ThresholdLowMid)
				{
					FireUp();
				} else
				{
					FadeOut();
				}
			}
			
		}

		private void FadeOut()
		{
			GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
		}

		private void FireUp()
		{
			
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
