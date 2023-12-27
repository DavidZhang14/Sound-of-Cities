//	Created by: Sunny Valley Studio 
//	https://svstudio.itch.io

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SVS
{

    public class CameraMovement : MonoBehaviour
    {
        private Camera gameCamera;
        private GameObject listener;
        public float cameraMovementSpeed = 5;
        public static CameraMovement instance;
        private void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }
        private void Start()
        {
            listener = GameObject.Find("Character");
            if (listener == null) Debug.LogError("Can't find character listener.");
            gameCamera = GetComponent<Camera>();
        }
        public void MoveCamera(Vector3 inputVector)
        {
            var movementVector = Quaternion.Euler(0,30,0) * inputVector;
            gameCamera.transform.position += movementVector * Time.deltaTime * cameraMovementSpeed;
            listener.transform.position += movementVector * Time.deltaTime * cameraMovementSpeed;
        }
    }
}