using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        public Game Game;
        
        private void Awake()
        {
            Game = new Game();
            Game.Initialize();
            
            DontDestroyOnLoad(gameObject);
            
            SceneManager.LoadScene(1);
        }
    }
}
