using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
	private AudioSource _audioSource;
	public float[] Samples = new float[512]; 

	// Use this for initialization
	void Start ()
	{
		_audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		GetSpectrumData();
	}

	void GetSpectrumData()
	{
		_audioSource.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
	}
}
