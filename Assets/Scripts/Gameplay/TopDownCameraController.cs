// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.Gameplay.Player
{   
    public class TopDownCameraController : MonoBehaviour
    {
        [Header("Following Variables")]
        [SerializeField] private Transform followTarget;
        [SerializeField] private Vector2 bounds;
        [SerializeField] private Vector3 followOffset;
        [SerializeField] private float smoothingRate;

        [Header("Camera Shake Variables")]
        [SerializeField] private float shakeDuration;
        [SerializeField] private float shakeMagnitude;
        private bool shaking;
        private Coroutine shake;

        private Vector3 targetPosition;
        private Vector3 velocity;

        private void FixedUpdate()
        {
            if (shaking)
                return;

            bool _xApprox = Mathf.Approximately(transform.position.x, followTarget.position.x + followOffset.x);
            bool _zApprox = Mathf.Approximately(transform.position.z, followTarget.position.z + followOffset.z);

            if (_xApprox && _zApprox)
                return;
                        
            targetPosition = Vector3.SmoothDamp(transform.position, followTarget.position + followOffset, ref velocity, smoothingRate);
            
            if(targetPosition.x > 0)
                targetPosition.x = Mathf.Min(targetPosition.x, bounds.x);
            else
                targetPosition.x = Mathf.Max(targetPosition.x, -bounds.x);

            if (targetPosition.z > 0)
                targetPosition.z = Mathf.Min(targetPosition.z, bounds.y);
            else
                targetPosition.z = Mathf.Max(targetPosition.z, -bounds.y);

            targetPosition.y = followOffset.y;

            transform.position = targetPosition;
        }

        public void ShakeCamera(Component invoker, object data)
        {
            if (shake != null)
                StopCoroutine(shake);
            
            shake = StartCoroutine(Shake());
        }

        public void StopShake(Component invoker, object data)
        {
            if (shake != null)
                StopCoroutine(shake);
        }

        private IEnumerator Shake()
        {
            Vector3 _originalPosition = transform.localPosition;
            float currentTime = 0.0f;
            shaking = true;

            while(currentTime <= shakeDuration)
            {
                float _x = Random.insideUnitCircle.x * shakeMagnitude;
                float _z = Random.insideUnitCircle.y * shakeMagnitude;

                transform.localPosition += new Vector3(_x, 0.0f, _z);

                currentTime += Time.deltaTime;

                yield return null;
            }

            shaking = false;
        }
    }
}