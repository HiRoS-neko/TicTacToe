using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace TicTacToe
{
    public class Match : UnityEngine.MonoBehaviour
    {
        public enum MatchType
        {
            PlayerVsPlayer,
            PlayerVsAi,
            AiVsAi
        }

        [SerializeField] private MatchType _matchType;

        [SerializeField] private Player[] _players = new Player[2];


        [SerializeField] private BoardObject _boardObject;

        private Player _currentPlayer;

        [ContextMenu("Start Game")]
        public void StartMatch()
        {
            StartMatch(_matchType);
        }

        public void StartMatch(MatchType matchType)
        {
            _matchType = matchType;

            StartCoroutine(MatchLoop());
        }

        private IEnumerator MatchLoop()
        {
            /// Game loop
            ///
            /// Start Game
            /// --->First Person starts turn
            /// |       Wait for turn to play
            /// |   Check if end of game --------|
            /// |   Second Person starts turn    |
            /// |       Wait for turn to play    |
            /// |   Check if end of game --------|
            /// --<                              |
            ///                                  |
            /// Display Results of Game <--------|

            Piece win = Piece.None;
            int playerIndex = 0;
            bool gameOver = false;

            do
            {
                // player starts turn
                _players[playerIndex].StartTurn(_boardObject);
                //wait for the end of their turn
                do
                {
                    yield return new WaitForSeconds(0.01f);
                } while (!_players[playerIndex].CompletedTurn);

                //check status of the game
                gameOver = !Player.MovesLeft(_boardObject.board) ||
                           (win = Player.HasWin(_boardObject.board)) != Piece.None;

                playerIndex = (playerIndex + 1) % 2;
            } while (!gameOver);


            //todo hook up ui and events - gamemanager?
            switch (win)
            {
                case Piece.XPiece:
                    Debug.Log("X has won");
                    break;
                case Piece.None:
                    Debug.Log("No available moves left");
                    break;
                case Piece.OPiece:
                    Debug.Log("O has won");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}