using Infrastructure;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        var bootstrapper = FindObjectOfType<Bootstrapper>();
        var di = bootstrapper.Game.DependencyInjector;
    }
}
