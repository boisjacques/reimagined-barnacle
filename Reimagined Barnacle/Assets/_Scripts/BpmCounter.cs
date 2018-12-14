using UnityEngine;

namespace _Scripts
{
	public class BpmCounter : MonoBehaviour
	{
		private static BpmCounter _instance;

		private void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(this.gameObject);
			}
			else
			{
				_instance = this;
				DontDestroyOnLoad(this.gameObject);
			}
		}

		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update () {
		
		}
	}
}
