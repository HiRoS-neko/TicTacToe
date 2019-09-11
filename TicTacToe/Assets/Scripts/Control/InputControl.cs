using System;
using UnityEngine;

namespace Control
{
    public class InputControl : MonoBehaviour
    {
        public Action<Vector2> Rotation;

        private Vector2 _rotation;

        private void Update()
        {
            _rotation = Vector2.Lerp(_rotation,
                new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized, 0.5f);

            Rotation?.Invoke(_rotation);
        }
    }
}