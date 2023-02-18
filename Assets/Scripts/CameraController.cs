using UnityEngine;
using Unity.Mathematics;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        public enum MouseButton
        {
            Left = 0,
            Right = 1,
            Middle = 3
        }
        public MouseButton dragButton;
        public float scrollSpeed;
        public float scrollLimit;
        public float dragSpeed;
        Vector3 previousPosition;
        Camera cam;

        void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void LateUpdate()
        {
            if (Input.GetMouseButtonDown((int)dragButton))
            {
                previousPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton((int)dragButton))
            {
                var delta = Input.mousePosition - previousPosition;
                delta = cam.cameraToWorldMatrix * delta;
                transform.position += delta * dragSpeed;
                previousPosition = Input.mousePosition;
            }
            var scroll = Input.mouseScrollDelta.y;
            cam.orthographicSize = math.clamp(cam.orthographicSize - scroll * scrollSpeed, scrollLimit, float.MaxValue);
        }
    }
}
