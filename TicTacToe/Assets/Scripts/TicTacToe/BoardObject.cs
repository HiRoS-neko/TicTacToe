﻿using System;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] private Camera _camera;

        public Action<BoardObject, Vector2Int> moveRegistered;

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

        private Vector2 _boardBounds;

        private Control.InputControl _controls;

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
            int numChildren = gameObject.transform.childCount;
            for (int i = 0; i < numChildren; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }

            _spaceGameObjects = new BoardSpace[_board.Size, _board.Size];

            for (int i = 0; i < _board.Size; i++)
            {
                for (int j = 0; j < _board.Size; j++)
                {
                    float x = j - (float) _board.Size / 2;
                    float y = (float) _board.Size / 2 - i;

                    //create a board space at x, y

                    _spaceGameObjects[i, j] = Instantiate(boardSpacePrefab,
                        transform.localToWorldMatrix * (transform.localPosition + new Vector3(x, y, 0)),
                        transform.rotation * Quaternion.Euler(0, 180, 0), transform);

                    _spaceGameObjects[i, j].Coordinates = new Vector2Int(i, j);
                }
            }

            _boardBounds = Vector2.one * ((float) _board.Size / 2);
        }

        public void OnEnable()
        {
            _controls = FindObjectOfType<Control.InputControl>();
            _controls.Pan += Pan;
            _controls.Zoom += Zoom;
            _controls.Ray += Ray;
        }

        private void Ray(Ray ray)
        {
            //ray trace ray and check to see if intersects with a board piece
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform.CompareTag("Space"))
                {
                    //hits a space
                    //for now we will just make it a X
                    //todo make a player object and pass to that here instead
                    var space = hit.transform.GetComponent<BoardSpace>();
                    moveRegistered?.Invoke(this, space.Coordinates);
                }
            }
        }

        private void OnDisable()
        {
            _controls.Pan -= Pan;
            _controls.Zoom -= Zoom;
            _controls.Ray -= Ray;
        }

        private void Zoom(float zoom)
        {
            transform.parent.localScale += zoom * Vector3.one;
        }

        private void Pan(Vector2 pan)
        {
            transform.localPosition += (Vector3) pan;
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

        public void SetPieceSilent(int x, int y, Piece piece)
        {
            _pieces[x, y] = piece;
        }

        public void SetPiece(Vector2Int coordinate, Piece piece)
        {
            SetPiece(coordinate.x, coordinate.y, piece);
        }
    }
}