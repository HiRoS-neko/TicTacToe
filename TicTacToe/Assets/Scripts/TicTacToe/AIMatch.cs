using System;
using System.Collections;
using System.Collections.Generic;
using TicTacToe;
using UnityEngine;

namespace DefaultNamespace
{
    public class AIMatch : UnityEngine.MonoBehaviour
    {
        [SerializeField] private List<AIPlayer> _ai;

        [ContextMenu("Play AI Match")]
        private void PlayAIMatch()
        {
            StartCoroutine(AIMatchCoroutine());
        }

        private IEnumerator AIMatchCoroutine()
        {
            bool status = true;
            int index = 0;

            do
            {
                status = _ai[index].CalculateMove();
                index++;
                index %= _ai.Count;
                yield return null;
            } while (status);
        }
    }
}