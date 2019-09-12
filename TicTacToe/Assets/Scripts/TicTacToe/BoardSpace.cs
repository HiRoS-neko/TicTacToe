using System;
using UnityEngine;

namespace TicTacToe
{
    public class BoardSpace : UnityEngine.MonoBehaviour
    {
        [SerializeField] private GameObject _xPiece;
        [SerializeField] private GameObject _oPiece;


        public Vector2Int Coordinates;

        public void SetPiece(Piece piece)
        {
            switch (piece)
            {
                case Piece.XPiece:
                    _xPiece.SetActive(true);
                    _oPiece.SetActive(false);
                    break;
                case Piece.None:
                    _xPiece.SetActive(false);
                    _oPiece.SetActive(false);
                    break;
                case Piece.OPiece:
                    _xPiece.SetActive(false);
                    _oPiece.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(piece), piece, null);
            }
        }
    }
}