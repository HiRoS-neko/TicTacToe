using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Control
{
    public class InputControl : MonoBehaviour
    {
        public enum Scheme
        {
            Keyboard,
            Controller,
            Touch
        }

        public Action<Vector2> Rotation;

        public Action<Vector2> Pan;

        public Action<float> Zoom;

        public Action<Vector2> Selection;

        private Vector2 _rotation;
        private Vector2 _pan;
        private float _zoom;
        private Scheme _scheme;

        [SerializeField] private float _interpolation;
        private TouchPhase _touchPhase;
        private Vector2 _selection;
        private bool _select;

        private void Awake()
        {
            if (Input.touchSupported)
            {
                _scheme = Scheme.Touch;
            }
            else if (Input.GetJoystickNames().Length > 0)
            {
                _scheme = Scheme.Controller;
            }
            else _scheme = Scheme.Keyboard;

            if (Input.acceleration != Vector3.zero)
            {
                //todo set bool to get from controller/keyboard
            }
        }

        private void Update()
        {
            switch (_scheme)
            {
                case Scheme.Keyboard:
                    _rotation = Vector2.Lerp(_rotation,
                        new Vector2(
                            (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) -
                            (Input.GetKey(KeyCode.LeftControl) ? 1 : 0),
                            (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) -
                            (Input.GetKey(KeyCode.DownArrow) ? 1 : 0)).normalized, _interpolation);

                    _pan = Vector2.Lerp(_pan,
                               new Vector2(
                                   (Input.GetKey(KeyCode.D) ? 1 : 0) -
                                   (Input.GetKey(KeyCode.A) ? 1 : 0),
                                   (Input.GetKey(KeyCode.W) ? 1 : 0) -
                                   (Input.GetKey(KeyCode.S) ? 1 : 0)).normalized, _interpolation) * .25f;

                    _zoom = Mathf.Lerp(_zoom, Input.mouseScrollDelta.y, _interpolation) * .25f;

                    if (Input.GetMouseButtonDown(0))
                    {
                        _selection = Input.mousePosition;
                        _select = true;
                    }

                    break;
                case Scheme.Controller:
                    break;
                case Scheme.Touch:
                    var accel = Input.acceleration.normalized;
                    _rotation = Vector2.Lerp(_rotation, new Vector2(accel.x, accel.y), _interpolation);

                    switch (Input.touchCount)
                    {
                        case 1:
                            //single tap = select
                            var touch = Input.GetTouch(0);
                            if (touch.phase == TouchPhase.Moved)
                            {
                                _pan = Vector2.Lerp(_pan, touch.deltaPosition, _interpolation);
                            }
                            else if (touch.phase == TouchPhase.Ended && _touchPhase == TouchPhase.Stationary)
                            {
                                _selection = touch.position;
                                _select = true;
                            }

                            _touchPhase = touch.phase;
                            break;
                        case 2:
                            var finger1 = Input.GetTouch(0);
                            var finger2 = Input.GetTouch(1);
                            //a zoom, check delta position of the difference 
                            _zoom = Mathf.Lerp(_zoom, (finger1.deltaPosition - finger2.deltaPosition).magnitude, _interpolation);
                            break;
                    }


                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (_rotation != Vector2.zero) Rotation?.Invoke(_rotation);
            if (_pan != Vector2.zero) Pan?.Invoke(_pan);
            if (Math.Abs(_zoom) > 0.0001f) Zoom?.Invoke(_zoom);

            if (_select)
            {
                Selection?.Invoke(_selection);
                _select = false;
            }
        }
    }
}