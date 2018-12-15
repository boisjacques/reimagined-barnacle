using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    public class Menu : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("Main Scene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

