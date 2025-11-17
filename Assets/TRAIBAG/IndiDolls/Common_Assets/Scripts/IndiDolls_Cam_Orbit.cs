using UnityEngine;
using UnityEngine.InputSystem;

namespace TRAIBAG
{
    public class CameraOrbit : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform target; // 카메라가 중심으로 둘러볼 대상

        [Header("Rotation Settings")]
        public float rotationSpeed = 3f; // 마우스 드래그 회전 속도

        [Header("Vertical Limits (-12, 60 / 10, 10)")]
        public float minVerticalAngle = -12f; // 위쪽 제한
        public float maxVerticalAngle = 60f;  // 아래쪽 제한

        [Header("Auto Rotate (15)")]
        public float autoRotateSpeed = 0f; // 자동 회전 속도 (0 = 회전 없음)

        private float yaw;
        private float pitch;

        void Start()
        {
            if (target == null)
            {
                Debug.LogWarning("타겟이 지정되지 않았습니다.");
                enabled = false;
                return;
            }

            Vector3 angles = transform.eulerAngles;
            yaw = angles.y;
            pitch = angles.x;
        }

        void LateUpdate()
        {
            var mouse = Mouse.current;

            // 마우스 드래그 회전
            if (mouse.leftButton.isPressed)
            {
                float mouseX = mouse.delta.x.ReadValue();
                float mouseY = mouse.delta.y.ReadValue();

                yaw += mouseX * rotationSpeed * Time.deltaTime * 10f;
                pitch -= mouseY * rotationSpeed * Time.deltaTime * 10f;
            }
            else
            {
                // 자동 회전 (좌우)
                if (autoRotateSpeed != 0f)
                    yaw += autoRotateSpeed * Time.deltaTime;
            }

            // 위아래 각도 제한
            pitch = Mathf.Clamp(pitch, minVerticalAngle, maxVerticalAngle);

            // 카메라 회전·위치 적용
            transform.rotation = Quaternion.Euler(pitch, yaw, 0);
            transform.position = target.position - transform.forward * Vector3.Distance(transform.position, target.position);
        }
    }
}
