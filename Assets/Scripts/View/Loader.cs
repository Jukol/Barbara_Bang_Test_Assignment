using Infrastructure;
using UnityEngine;
using View;

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
        scenePopulator.Init(_di);
    }
}
