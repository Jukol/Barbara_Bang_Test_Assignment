using UnityEngine;

namespace View
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
    
        private Camera _camera;

        public void Init(Color color)
        {
            _camera = Camera.main;
            spriteRenderer.color = color;
            ResizeByScreen(_camera);
        }
    
        private void ResizeByScreen(Camera cam)
        {
            var myTransform = transform;

            myTransform.position = new Vector3(0, 0, 1);
            
            float screenHeight = cam.orthographicSize * 2;
            float screenWidth = screenHeight * cam.aspect;

            myTransform.localScale = new Vector3(screenWidth, screenHeight, 1);
        }
    }
}
