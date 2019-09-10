using System;
using Control;
using UnityEngine;

namespace Parallax
{
    [RequireComponent(typeof(Camera))]
    public class ParallaxControl : MonoBehaviour
    {
        private Camera _camera;

        private Vector2 _cameraBounds;

        [SerializeField] private InputControl _input;

        private void OnEnable()
        {
            _cameraBounds = Vector2.one;
            _camera = GetComponent<Camera>();
            _input.Rotation += Rotation;
        }

        private void Rotation(Vector2 rot)
        {
            //rot is recieved as a normalized vector2
            //to get camera position, it needs to be multiplied by the _cameraBounds
            _camera.transform.localPosition = new Vector2(rot.x * _cameraBounds.x, rot.y * _cameraBounds.y);

            //for the lens shift, it needs to be from 0 -> 1 instead of -1 -> 1
            //var lensShift = (rot * 0.5f) + 0.5f * Vector2.one;
            _camera.lensShift = rot * 0.25f;
        }

        private void OnDisable()
        {
            _input.Rotation -= Rotation;
        }
    }
}