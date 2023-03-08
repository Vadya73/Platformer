using System.Security.Cryptography;
using Scripts.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>();
            Destroy(session);
            var currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

}