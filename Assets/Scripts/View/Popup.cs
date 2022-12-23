using Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class Popup : MonoBehaviour
    {
        [SerializeField] private Button nextButton;
        [SerializeField] private Loader loader;

        private void OnEnable()
        {
            if (loader.CurrentLevel == loader.Bootstrapper.Game.MaxLevels - 1) 
                nextButton.gameObject.SetActive(false);
        }
    }
}
