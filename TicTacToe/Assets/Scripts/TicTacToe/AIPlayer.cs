using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace TicTacToe
{
    internal class MiniMax : IEnumerator
    {
        private int _depth;
        private bool _isMax;

        private int _x, _y;

        private Board _board;

        private Piece _role;

        private Tuple<int, Tuple<int, int>> _best;
        private int _score;

        private Piece Role;

        public MiniMax(Board board, int depth, bool isMax, Piece role)
        {
            _depth = depth;
            _isMax = isMax;
            _board = board;

            Role = role;
            _role = (isMax ? role : (Piece) ((int) role * -1));

            _score = (EvaluateBoard(board, (isMax ? _role : (Piece) ((int) _role * -1))) -
                      EvaluateBoard(board, (isMax ? (Piece) ((int) _role * -1) : _role)));

            if (isMax) _best = new Tuple<int, Tuple<int, int>>(int.MinValue, null);
            else _best = new Tuple<int, Tuple<int, int>>(int.MaxValue, null);
        }

        public bool MoveNext()
        {
            if (_board.Pieces[_x, _y] == Piece.None)
            {
                //do move to board
                _board.SetPieceSilent(_x, _y, _role);

                //try move in minimax
                var miniMax = new MiniMax(_board, _depth - 1, !_isMax, Role);

                while (miniMax.MoveNext()) ;

                var temp = miniMax.GetBest();

                //undo move on board
                _board.SetPieceSilent(_x, _y, Piece.None);


                if (_isMax)
                {
                    if (temp.Item1 > _best.Item1)
                    {
                        _best = new Tuple<int, Tuple<int, int>>(temp.Item1, new Tuple<int, int>(_x, _y));
                    }
                    else if (temp.Item1 == _best.Item1)
                    {
                        //choose randomly between them in order to make a game non-deterministic
                        var r = Random.Range(0, 1f);
                        if (r > 0.5f)
                        {
                            _best = new Tuple<int, Tuple<int, int>>(temp.Item1, new Tuple<int, int>(_x, _y));
                        }
                    }
                }
                else
                {
                    if (temp.Item1 < _best.Item1)
                    {
                        _best = new Tuple<int, Tuple<int, int>>(temp.Item1, new Tuple<int, int>(_x, _y));
                    }
                    else if (temp.Item1 == _best.Item1)
                    {
                        //choose randomly between them in order to make a game non-deterministic
                        var r = Random.Range(0, 1f);
                        if (r > 0.5f)
                        {
                            _best = new Tuple<int, Tuple<int, int>>(temp.Item1, new Tuple<int, int>(_x, _y));
                        }
                    }
                }
            }


            _x += 1;
            if (_x >= _board.Size)
            {
                _x = 0;
                _y += 1;
                if (_y >= _board.Size)
                {
                    _x = 0;
                    _y = 0;
                    return true;
                }
            }

            return false;
        }

        private int EvaluateBoard(Board board, Piece pieceToEvaluateFor)
        {
            //number of possible row, column and diagonals that can be achieved from the current board for the piece passed
            //subtract the number of rows, columns and diagonals that the other piece can achieve
            //with a bonus score for an achieved win state (a full row, column or diagonal)

            int score = 0;

            //check each column for possible win rows
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (board.Pieces[i, j] != pieceToEvaluateFor && board.Pieces[i, j] != Piece.None)
                    {
                        //there is another piece blocking this row
                        break;
                    }

                    if (j == board.Size - 1)
                    {
                        //at the last index of the row and there is no blocking pieces
                        score += 1;
                    }
                }
            }


            //check each row for possible win columns
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (board.Pieces[j, i] != pieceToEvaluateFor && board.Pieces[j, i] != Piece.None)
                    {
                        //there is another piece blocking this column
                        break;
                    }

                    if (j == board.Size - 1)
                    {
                        //at the last index of the column and there is no blocking pieces
                        score += 1;
                    }
                }
            }


            //check both diagonals

            //positive diagonal
            for (int i = 0; i < board.Size; i++)
            {
                if (board.Pieces[i, i] != pieceToEvaluateFor && board.Pieces[i, i] != Piece.None)
                {
                    //there is another piece blocking this column
                    break;
                }

                if (i == board.Size - 1)
                {
                    //at the last index of the column and there is no blocking pieces
                    score += 1;
                }
            }

            //negative diagonal
            for (int i = 0; i < board.Size; i++)
            {
                int j = (board.Size - 1) - i;
                if (board.Pieces[i, j] != pieceToEvaluateFor && board.Pieces[i, j] != Piece.None)
                {
                    //there is another piece blocking this column
                    break;
                }

                if (i == board.Size - 1)
                {
                    //at the last index of the column and there is no blocking pieces
                    score += 1;
                }
            }


            //todo full row, column or diagonal bonus score
            //row
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (board.Pieces[i, j] != pieceToEvaluateFor) break;
                    else if (j == board.Size - 1) score += 10;
                }
            }

            //column
            for (int i = 0; i < board.Size; i++)
            {
                for (int j = 0; j < board.Size; j++)
                {
                    if (board.Pieces[j, i] != pieceToEvaluateFor) break;
                    else if (j == board.Size - 1) score += 10;
                }
            }

            //diagonal positive
            for (int i = 0; i < board.Size; i++)
            {
                if (board.Pieces[i, i] != pieceToEvaluateFor) break;
                else if (i == board.Size - 1) score += 10;
            }

            //diagonal negative
            for (int i = 0; i < board.Size; i++)
            {
                int j = (board.Size - 1) - i;
                if (board.Pieces[i, j] != pieceToEvaluateFor) break;
                else if (i == board.Size - 1) score += 10;
            }

            return score;
        }


        public Tuple<int, Tuple<int, int>> GetBest()
        {
            return _best;
        }

        public bool IsCompleted { get; set; }

        public void Reset()
        {
            _x = 0;
            _y = 0;
            _depth = 0;
            _isMax = false;
        }

        public object Current { get; }
    }


    public class AIPlayer : Player
    {
        public enum Difficulty
        {
            //difiiculty is based on depth the minimax will go
            Easy = 1, // essentially random
            Medium = 2, // difficult on small baords
            Hard = 4, // difficult on medium boards
            VeryHard = 6, // difficult on large boards
            Impossible = 8 // should be effectively impossible to win
        }

        [SerializeField] private Difficulty _difficulty;


        public IEnumerator CalculateMove(BoardObject boardObject)
        {
            Piece win = Piece.None;


            var miniMax = new MiniMax(boardObject.board, (int) _difficulty, true, _role);

            while (miniMax.MoveNext())
            {
                yield return null;
            }

            var pos = miniMax.GetBest();

            if (pos.Item2 != null)
            {
                boardObject.board.SetPiece(pos.Item2.Item1, pos.Item2.Item2, _role);
            }

            _completed = true;
        }

        public override void StartTurn(BoardObject boardObject)
        {
            _completed = false;
            StartCoroutine(CalculateMove(boardObject));
        }

    }
}