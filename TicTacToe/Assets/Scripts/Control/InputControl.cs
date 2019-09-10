using System;
using UnityEngine;

namespace Control
{
    public class InputControl : MonoBehaviour
    {
        public Action<Vector2> Rotation;

        private void Update()
        {
            Rotation?.Invoke(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized);
        }
    }
}