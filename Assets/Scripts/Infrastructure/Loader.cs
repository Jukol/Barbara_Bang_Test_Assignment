using UnityEngine;
using View;

namespace Infrastructure
{
    public class Loader : MonoBehaviour
    {
        public Bootstrapper Bootstrapper => _bootstrapper;
        public int CurrentLevel => _currentLevel;
        
        [SerializeField] private ScenePopulator scenePopulator;
        [SerializeField] private GameObject levelSelector;
        
        private DependencyInjector _di;
        private int _currentLevel;
        private Bootstrapper _bootstrapper;
    
        private void Awake()
        {
            _bootstrapper = FindObjectOfType<Bootstrapper>();
            _di = _bootstrapper.Game.DependencyInjector;
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
            _currentLevel++;
            scenePopulator.Init(_di, _currentLevel);
        }
    }
}
