using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(LineRenderer))]
    public class LineRendererController : MonoBehaviour
    {
        private readonly int _numberOfPoints = Audio.AudioBandBuffer.Length;

        public float Length = 50;

        public float WaveHeight = 10;

        public float Tolerance = 0.1f;

        private LineRenderer _lineRenderer;

        private Vector3[] _points;

        // Use this for initialization
        void Start()
        {
            _points = new Vector3[_numberOfPoints];
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = _numberOfPoints;
        }

        // Update is called once per frame
        void Update()
        {
            GeneratePoints();
            _lineRenderer.SetPositions(_points);
        }

        void GeneratePoints()
        {
            float start = -Length * 0.5f;
            float step = Length / _numberOfPoints;
            for (int i = 0; i < _numberOfPoints; i++)
            {
                float x = start + i * step;
                _points[i] = new Vector3(x, Audio.AudioBandBuffer[i] * WaveHeight + 0.5f, 0);
            }
        }
    }
}