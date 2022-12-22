using UnityEngine;
using View;

namespace Infrastructure
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private ScenePopulator scenePopulator;
    
        private DependencyInjector _di;
    
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<Bootstrapper>();
            _di = bootstrapper.Game.DependencyInjector;
        }
    
        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            scenePopulator.Init(_di);
        }
    }
}
