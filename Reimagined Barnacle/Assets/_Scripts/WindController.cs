using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class WindController : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            GenerateWindArray();
        }

        // Update is called once per frame
        void Update()
        {
        }

        void GenerateWindArray()
        {
            int numberOfWindArrays = InstantiateParticles._particleGenerators.Length;

            for (int i = 0; i < numberOfWindArrays; i++)
            {
                GameObject wz1 = new GameObject();
                GameObject wz2 = new GameObject();
                GameObject wz3 = new GameObject();
                GameObject wz4 = new GameObject();

                wz1.AddComponent<WindZone>();
                wz2.AddComponent<WindZone>();
                wz3.AddComponent<WindZone>();
                wz4.AddComponent<WindZone>();

                Vector3 pos1 = new Vector3(0.5f, 15, 0.5f);
                Vector3 pos2 = new Vector3(-0.5f, 15, 0.5f);
                Vector3 pos3 = new Vector3(-0.5f, 15, -0.5f);
                Vector3 pos4 = new Vector3(0.5f, 15, -0.5f);
                
                wz1.transform.Rotate(wz1.transform.right, 75);
                wz2.transform.Rotate(wz2.transform.forward, -75);
                wz3.transform.Rotate(wz3.transform.right, -75);
                wz4.transform.Rotate(wz4.transform.forward, 75);

                wz1.transform.Rotate(wz1.transform.up, -45);
                wz2.transform.Rotate(wz2.transform.up, -135);
                wz3.transform.Rotate(wz3.transform.up, 135);
                wz4.transform.Rotate(wz4.transform.up, 45);

                wz1.transform.parent = InstantiateParticles._particleGenerators[i].transform;
                wz2.transform.parent = InstantiateParticles._particleGenerators[i].transform;
                wz3.transform.parent = InstantiateParticles._particleGenerators[i].transform;
                wz4.transform.parent = InstantiateParticles._particleGenerators[i].transform;

                wz1.transform.position = transform.TransformPoint(wz1.transform.parent.position + pos1);
                wz2.transform.position = transform.TransformPoint(wz2.transform.parent.position + pos2);
                wz3.transform.position = transform.TransformPoint(wz3.transform.parent.position + pos3);
                wz4.transform.position = transform.TransformPoint(wz4.transform.parent.position + pos4);
            }
        }
    }
}