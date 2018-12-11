using UnityEngine;

namespace _Scripts
{
    public class InstantiateParticles : MonoBehaviour
    {
        public Material TrailMaterial;
        private ParticleSystem.EmissionModule[] _emissionModules;
        private ParticleSystem.ShapeModule[] _shapeModules;
        private ParticleSystem.MainModule[] _mainModules;
        private ParticleSystem.NoiseModule[] _noiseModules;
        private ParticleSystem[] _particleGenerators;
        private int _numberOfParticleGenerators = Audio.FrequencyBand.Length;

        // Use this for initialization
        void Start()
        {
            _emissionModules = new ParticleSystem.EmissionModule[_numberOfParticleGenerators];
            _shapeModules = new ParticleSystem.ShapeModule[_numberOfParticleGenerators];
            _mainModules = new ParticleSystem.MainModule[_numberOfParticleGenerators];
            _particleGenerators = new ParticleSystem[_numberOfParticleGenerators];
            _noiseModules = new ParticleSystem.NoiseModule[_numberOfParticleGenerators];
            float offset = -16;
            for (int i = 0; i < _numberOfParticleGenerators; i++)
            {
                GameObject go = new GameObject();
                ParticleSystem particleGenerator = go.AddComponent<ParticleSystem>();
                ParticleSystemRenderer renderer =  (ParticleSystemRenderer)particleGenerator.GetComponent<Renderer>();
                _mainModules[i] = particleGenerator.main;
                _mainModules[i].gravityModifier = 0.5f;
                _mainModules[i].startSize = 0.1f;
                _emissionModules[i] = particleGenerator.emission;
                _emissionModules[i].rateOverTime = 0;
                _emissionModules[i].enabled = true;

                _shapeModules[i] = particleGenerator.shape;
                _shapeModules[i].angle = 1;
                _shapeModules[i].rotation = new Vector3(-90, 0, 0);

                _noiseModules[i] = particleGenerator.noise;
                _noiseModules[i].enabled = true;

                ParticleSystem.TrailModule trm = particleGenerator.trails;
                trm.enabled = true;
                trm.lifetime = 0.1f;

                renderer.trailMaterial = TrailMaterial;

                Vector3 pos = new Vector3(offset + 4 * i, 0, 0);
                go.transform.position = transform.TransformPoint(pos);
                go.transform.parent = transform;
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < _numberOfParticleGenerators; i++)
            {
                _mainModules[i].startSpeed = Audio.FrequencyBand[i] * 20;
                _emissionModules[i].rateOverTime = Audio.BandBuffer[i] * 100;
                _noiseModules[i].strength = Audio.BandBuffer[i];
                _noiseModules[i].frequency = Audio.BandBuffer[i] * 0.5f;
            }
        }
    }
}