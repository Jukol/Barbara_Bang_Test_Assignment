using System.Collections;
using UnityEngine;

namespace View
{
    public class Flasher : MonoBehaviour
    {
        [SerializeField] private float flashIntverval;
        [SerializeField] private GameObject flashingText;
        
        private WaitForSeconds _waitTime;
    
        private void OnEnable()
        {
            _waitTime = new WaitForSeconds(flashIntverval);
            StartCoroutine(Flash());
        }

        private IEnumerator Flash()
        {
            while (true)
            {
                flashingText.SetActive(true);
                yield return _waitTime;
                flashingText.SetActive(false);
                yield return _waitTime;
            }
        }
    
    }
}
