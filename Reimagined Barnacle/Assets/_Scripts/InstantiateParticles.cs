using UnityEngine;

namespace _Scripts
{
    public class InstantiateParticles : MonoBehaviour
    {
        public Material TrailMaterial;
        public Material ParticleMaterial;
        public Light ParticleLight;
        private ParticleSystem.EmissionModule[] _emissionModules;
        private ParticleSystem.ShapeModule[] _shapeModules;
        private ParticleSystem.MainModule[] _mainModules;
        private ParticleSystem.NoiseModule[] _noiseModules;
        public static GameObject[] _particleGenerators;
        private int _numberOfParticleGenerators = Audio.FrequencyBand.Length;

        // Use this for initialization
        void Start()
        {
            _emissionModules = new ParticleSystem.EmissionModule[_numberOfParticleGenerators];
            _shapeModules = new ParticleSystem.ShapeModule[_numberOfParticleGenerators];
            _mainModules = new ParticleSystem.MainModule[_numberOfParticleGenerators];
            _particleGenerators = new GameObject[_numberOfParticleGenerators];
            _noiseModules = new ParticleSystem.NoiseModule[_numberOfParticleGenerators];
            float offset = -16;
            for (int i = 0; i < _numberOfParticleGenerators; i++)
            {
                GameObject go = new GameObject();
                ParticleSystem particleGenerator = go.AddComponent<ParticleSystem>();
                ParticleSystemRenderer particleRenderer =  (ParticleSystemRenderer)particleGenerator.GetComponent<Renderer>();
                
                ExtractModules(i, particleGenerator);

                ParticleSystem.LightsModule lm = particleGenerator.lights;
                lm.enabled = true;
                lm.light = ParticleLight;
                lm.ratio = 1;
                lm.useParticleColor = true;

                ParticleSystem.TrailModule trm = particleGenerator.trails;
                trm.enabled = true;
                trm.lifetime = 0.1f;

                ParticleSystem.ExternalForcesModule em = particleGenerator.externalForces;
                em.enabled = true;
                em.multiplier = 0.5f;

                particleRenderer.renderMode = ParticleSystemRenderMode.Mesh;
                particleRenderer.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
                particleRenderer.material = ParticleMaterial;
                particleRenderer.trailMaterial = TrailMaterial;

                Vector3 pos = new Vector3(offset + 4 * i, 0, 0);
                go.transform.position = transform.TransformPoint(transform.position + pos);
                go.transform.parent = transform;
                _particleGenerators[i] = go;
            }
            InitializeMainModules();
            InitializeEmissionModules();
            InitializeNoiseModules();
            InitializeShapeModules();
            GameObject windGenerator = new GameObject();
            windGenerator.AddComponent<WindController>();
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < _numberOfParticleGenerators; i++)
            {
                _mainModules[i].startColor = Color.HSVToRGB(Audio.FrequencyBand[i], 1, 1);
                _mainModules[i].startSpeed = Audio.FrequencyBand[i] * 20;
                _emissionModules[i].rateOverTime = Audio.BandBuffer[i] * 100;
                _noiseModules[i].strength = Audio.BandBuffer[i];
                _noiseModules[i].frequency = Audio.BandBuffer[i] * 0.5f;
            }
        }

        void InitializeMainModules()
        {
            for (int i = 0; i < _numberOfParticleGenerators; i++)
            {
                _mainModules[i].gravityModifier = 0.5f;
                _mainModules[i].startSize = 0.1f;            
            }
        }

        void InitializeEmissionModules()
        {
            for (int i = 0; i < _numberOfParticleGenerators; i++)
            {
                _emissionModules[i].rateOverTime = 0;
                _emissionModules[i].enabled = true;
            }
        }

        void InitializeShapeModules()
        {
            for (int i = 0; i < _numberOfParticleGenerators; i++)
            {
                _shapeModules[i].angle = 1;
                _shapeModules[i].rotation = new Vector3(-90, 0, 0);
            }
        }
        
        void InitializeNoiseModules()
        {
            for (int i = 0; i < _numberOfParticleGenerators; i++)
            {
                _noiseModules[i].enabled = true;
            }
        }

        void ExtractModules(int i, ParticleSystem particleGenerator)
        {
            _mainModules[i] = particleGenerator.main;
            _emissionModules[i] = particleGenerator.emission;
            _shapeModules[i] = particleGenerator.shape;
            _noiseModules[i] = particleGenerator.noise;
        }
    }
}