using System;
using UnityEngine;

namespace TicTacToe
{
    public enum Piece
    {
        XPiece = -1,
        None = 0,
        OPiece = 1
    }

    public class BoardObject : MonoBehaviour
    {
        public Board board;
    }

    [Serializable]
    public struct Board
    {
        public int Size
        {
            get => _size;
            set => _size = value;
        }

        private int _size;

        [SerializeField] private Piece[,] _pieces;

        public Piece[,] Pieces
        {
            get => _pieces;
            set => _pieces = value;
        }

        public static Board Clone(Board board)
        {
            var newBoard = new Board();

            newBoard.Size = board.Size;
            newBoard.Pieces = new Piece[newBoard.Size, newBoard.Size];

            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    newBoard.Pieces[i, j] = board.Pieces[i, j];
                }
            }

            return newBoard;
        }
    }
}