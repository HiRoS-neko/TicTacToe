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
        public Board board
        {
            get { return _board; }
            set
            {
                if (_board.PieceMoved != null)
                {
                    _board.PieceMoved -= PieceMoved;
                }

                _board = value;

                _board.PieceMoved += PieceMoved;
            }
        }

        private void PieceMoved(int x, int y, Piece piece)
        {
            //update the game object in space
            _spaceGameObjects?[x, y]?.SetPiece(piece);
        }

        private BoardSpace[,] _spaceGameObjects;

        [SerializeField] public BoardSpace boardSpacePrefab;
        private Board _board;


        public void CreateGameBoard()
        {
            
        }
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

        public Action<int, int, Piece> PieceMoved;

        public Piece[,] Pieces
        {
            get => _pieces;
            set => _pieces = value;
        }

        public void SetPiece(int x, int y, Piece piece)
        {
            _pieces[x, y] = piece;
            PieceMoved?.Invoke(x, y, piece);
        }

        public Board SetPieceClone(int x, int y, Piece piece)
        {
            var temp = new Board {Size = this.Size, Pieces = (Piece[,]) this._pieces.Clone()};
            temp.SetPiece(x, y, piece);
            return temp;
        }

        public static Board Clone(Board board)
        {
            var temp = new Board {Size = board.Size, Pieces = (Piece[,]) board._pieces.Clone()};
            return temp;
        }
    }
}