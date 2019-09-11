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

        [SerializeField] private Vector3 _focusPoint;

        [SerializeField] private InputControl _input;

        private void OnEnable()
        {
            _cameraBounds = (Display.main.renderingWidth * Vector2.right + Display.main.renderingHeight * Vector2.up) /
                            Mathf.Min(Display.main.renderingWidth, Display.main.renderingHeight);
            _camera = GetComponent<Camera>();
            _input.Rotation += Rotation;
        }

        private void Rotation(Vector2 rot)
        {
            //rot is recieved as a normalized vector2
            //to get camera position, it needs to be multiplied by the _cameraBounds
            _camera.transform.localPosition = new Vector2(rot.x * _cameraBounds.x, rot.y * _cameraBounds.y);


            _camera.transform.LookAt(_focusPoint);
        }

        private void OnDisable()
        {
            _input.Rotation -= Rotation;
        }
    }
}