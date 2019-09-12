using UnityEngine;

namespace TicTacToe
{
    public class HumanPlayer : Player
    {
        public override void StartTurn(BoardObject boardObject)
        {
            _completed = false;
            boardObject.moveRegistered += MoveRegistered;
        }

        private void MoveRegistered(BoardObject board, Vector2Int coordinates)
        {
            board.moveRegistered -= MoveRegistered;
            board.board.SetPiece(coordinates, _role);
            _completed = true;
        }
    }
}