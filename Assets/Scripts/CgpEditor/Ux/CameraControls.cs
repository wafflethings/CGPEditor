using UnityEngine;

namespace CgpEditor.Ux
{
    public class CameraControls : MonoSingleton<CameraControls>
    {
        public float Sensitivity = 3;
        public float ScrollSensitivity = 10;
        public float AddedCameraLerpSpeed = 5;
        public float ZoomSpeedMultiplier = 0.5f;
        public float PanSpeed = 5;
        public float DefaultZoom;
        public Camera Camera;
        [SerializeField] private float _targetCameraPosition;
        private float _startZoomOut;
        private bool _enabled;
        private Vector3 _realRotation;

        private void Start()
        {
            _realRotation = transform.rotation.eulerAngles;
            _targetCameraPosition = (Camera.transform.localPosition).magnitude;
            _startZoomOut = _targetCameraPosition;
            Debug.Log($"{Camera.transform.position} - {transform.position} = {Camera.transform.position - transform.position}, magni {(Camera.transform.position - transform.position).magnitude}");
        }
        
        private void Update()
        {
            transform.eulerAngles = _realRotation;
            
            Scroll();
            
            if (!_enabled)
            {
                _realRotation += new Vector3(0, Time.deltaTime * 10, 0);
                return;
            }
            
            if (Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                MouseLook();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
            
            Pan();
        }

        private void MouseLook()
        {
            _realRotation += new Vector3(Input.GetAxis("Mouse Y") * Sensitivity, -Input.GetAxis("Mouse X") * Sensitivity, 0);
        }

        private void Scroll()
        {
            if (_enabled)
            {
                _targetCameraPosition -= Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;
                _targetCameraPosition = Mathf.Clamp(_targetCameraPosition, 50, 500);
            }

            Camera.transform.localPosition = Vector3.MoveTowards(Camera.transform.localPosition, _targetCameraPosition * Vector3.up, 
                Time.deltaTime * ZoomSpeedMultiplier * ((Camera.transform.localPosition - _targetCameraPosition * Vector3.up).magnitude + AddedCameraLerpSpeed));
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

        public void Enable()
        {
            _targetCameraPosition = DefaultZoom;
            _enabled = true;
        }

        public void Disable()
        {
            _targetCameraPosition = _startZoomOut;
            _enabled = false;
        }
    }
}