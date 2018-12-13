using System;
using UnityEngine;

namespace _Scripts
{
    public class InstantiateWall : MonoBehaviour
    {
        public int Width = 8;
        public int Height = 8;

        public GameObject Prefab;

        private GameObject[,] _bricks;


        // Use this for initialization
        void Start()
        {
            _bricks = new GameObject[Height, Width];
            Vector3 start = -Vector3.left * Width * 0.5f;
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    GameObject cube = Instantiate(Prefab);
                    cube.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                    Vector3 pos = start + new Vector3(col, row, 0);
                    cube.transform.position = transform.TransformPoint(pos);
                    cube.transform.parent = transform;
                    _bricks[row, col] = cube;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    if (boost(Audio.BandBuffer[col]) > row)
                    {
                        _bricks[row, col].GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                        _bricks[row, col].GetComponent<Renderer>().material.SetColor("_EMISSION",
                            Color.HSVToRGB(col / Width, 1, 1));
                    }
                    else if (boost(Audio.BandBuffer[col]) <= row)
                    {
                        _bricks[row, col].GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                    }
                }
            }
        }

        float boost(float i)
        {
            return i * Height;
        }
    }
}