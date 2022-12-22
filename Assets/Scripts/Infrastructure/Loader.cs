using UnityEngine;
using View;

namespace Infrastructure
{
    public class Loader : MonoBehaviour
    {
        [SerializeField] private ScenePopulator scenePopulator;
        [SerializeField] private GameObject levelSelector;

        private DependencyInjector _di;
        private int _currentLevel;
    
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<Bootstrapper>();
            _di = bootstrapper.Game.DependencyInjector;
        }
    
        private void Start()
        {
            levelSelector.SetActive(true);
        }

        public void StartGame(int level)
        {
            _currentLevel = level;
            levelSelector.SetActive(false);
            scenePopulator.Init(_di, level);
        }

        public void StartOver()
        {
            levelSelector.SetActive(false);
            scenePopulator.Init(_di, _currentLevel);
        }

        public void StartNext()
        {
            levelSelector.SetActive(false);
            scenePopulator.Init(_di, _currentLevel + 1);
        }
    }
}
