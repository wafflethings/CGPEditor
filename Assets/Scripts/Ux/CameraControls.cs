using UnityEngine;

namespace CgpEditor.Ux
{
    public class CameraControls : MonoSingleton<CameraControls>
    {
        public float Sensitivity = 3;
        public float ScrollSensitivity = 10;
        public float AddedCameraLerpSpeed = 5;
        public float PanSpeed = 5;
        public Camera Camera;
        private float _targetCameraPosition;

        private void Start()
        {
            _targetCameraPosition = (transform.localPosition).magnitude;
            Debug.Log($"{Camera.transform.position} - {transform.position} = {Camera.transform.position - transform.position}, magni {(Camera.transform.position - transform.position).magnitude}");
        }
        
        private void Update()
        {
            if (Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                MouseLook();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }

            Scroll();
            Pan();
        }

        private void MouseLook()
        {
            transform.rotation *= Quaternion.Euler(Input.GetAxis("Mouse Y") * Sensitivity, -Input.GetAxis("Mouse X") * Sensitivity, 0);
            transform.rotation = Quaternion.Euler(Utils.ClampAngle(transform.rotation.eulerAngles.x, 270, 360), transform.rotation.eulerAngles.y, 0);
        }

        private void Scroll()
        {
            _targetCameraPosition -= Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;
            _targetCameraPosition = Mathf.Clamp(_targetCameraPosition, 50, 250);
            Camera.transform.localPosition = Vector3.MoveTowards(Camera.transform.localPosition, _targetCameraPosition * Vector3.up, 
                Time.deltaTime * ((Camera.transform.localPosition - _targetCameraPosition * -Camera.transform.forward).sqrMagnitude + AddedCameraLerpSpeed));
        }

        private void Pan()
        {
            Vector3 direction = new Vector3();

            if (Input.GetKey(KeyCode.Q))
            {
                direction += Vector3.up;
            }
            
            if (Input.GetKey(KeyCode.E))
            {
                direction += -Vector3.up;
            }

            if (Input.GetKey(KeyCode.W))
            {
                direction += Camera.transform.forward;
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                direction += -Camera.transform.forward;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                direction += -Camera.transform.right;
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                direction += Camera.transform.right;
            }

            transform.position += Time.deltaTime * PanSpeed * direction;
        }
    }
}