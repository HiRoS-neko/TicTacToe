using UnityEngine;

namespace TicTacToe
{
    public abstract class Player : MonoBehaviour
    {
        [SerializeField] protected Piece _role;
        [SerializeField] protected bool _completed;

        public abstract void StartTurn(BoardObject boardObject);
        public bool CompletedTurn
        {
            get => _completed;
            set => _completed = value;
        }

        public static Piece HasWin(Board board)
        {
            int[] sumX = new int[board.Size];
            int posDiag = 0, negDiag = 0;
            for (int i = 0; i < board.Size; i++)
            {
                posDiag += (int) board.Pieces[i, i];
                negDiag += (int) board.Pieces[i, (board.Size - 1) - i];
                int sumY = 0;
                for (int j = 0; j < board.Size; j++)
                {
                    sumY += (int) board.Pieces[i, j];
                    sumX[j] += (int) board.Pieces[i, j];
                }


                if (Mathf.Abs(sumY) == board.Size)
                {
                    return (Piece) Mathf.Sign(sumY);
                }
            }

            if (Mathf.Abs(posDiag) == board.Size)
            {
                return (Piece) Mathf.Sign(posDiag);
            }

            if (Mathf.Abs(negDiag) == board.Size)
            {
                return (Piece) Mathf.Sign(negDiag);
            }

            for (int i = 0; i < board.Size; i++)
            {
                if (Mathf.Abs(sumX[i]) == board.Size)
                {
                    return (Piece) Mathf.Sign(sumX[i]);
                }
            }


            return Piece.None;
        }


        public static bool MovesLeft(Board board)
        {
            bool available = false;

            //check for win conditions already
            //if (HasWin(board) != Piece.None) return false;

            //check for available spots left
            foreach (var boardPiece in board.Pieces)
            {
                if (boardPiece == Piece.None)
                {
                    available = true;
                    break;
                }
            }

            return available;
        }
    }
}