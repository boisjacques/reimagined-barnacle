using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class Settings : MonoBehaviour
    {
        private const string SONG = "song";

        private const string ONSET_DETECTION = "onset";
        private const string ONSET_THRESHOLD = "onset-threshold";

        private const string SUBBASE_THRESHOLD = "sub-base-threshold";
        private const string BASS_THRESHOLD = "bass-threshold";
        private const string LOW_MID_THRESHOLD = "low-mid-threshold";

        private const string FADE_OUT_DURATION = "fade-out";

        public Slider OnsetThreshold;
        public Slider SubBaseThreshold;
        public Slider BassThreshold;
        public Slider LowMidThreshold;
        public Slider FadeOutThreshold;
        public Dropdown Songs;
        private bool _onsetOn;
        private int _OnsetOnInt;
        
        public void ToggleOnset(int i)
        {
            _OnsetOnInt = i;
        }

        public void  SavePrefs()
        {
            PlayerPrefs.SetInt(SONG, Songs.value);
            PlayerPrefs.SetInt(ONSET_DETECTION, _OnsetOnInt);
            PlayerPrefs.SetFloat(ONSET_THRESHOLD, OnsetThreshold.value);
            PlayerPrefs.SetFloat(SUBBASE_THRESHOLD, SubBaseThreshold.value);
            PlayerPrefs.SetFloat(BASS_THRESHOLD, BassThreshold.value);
            PlayerPrefs.SetFloat(LOW_MID_THRESHOLD, LowMidThreshold.value);
            PlayerPrefs.SetFloat(FADE_OUT_DURATION, FadeOutThreshold.value);
        }
    }
}