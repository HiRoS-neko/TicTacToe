using System;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace TicTacToe
{
    public class AIPlayer : MonoBehaviour
    {
        public enum Difficulty
        {
            //difiiculty is based on depth the minimax will go
            Easy = 1, // essentially random
            Medium = 2, // difficult on small baords
            Hard = 4, // difficult on medium boards
            VeryHard = 8, // difficult on large boards
            Impossible = 16 // should be effectively impossible to win
        }

        [SerializeField] private Difficulty _difficulty;

        [SerializeField] private Piece _role;

        [SerializeField] private BoardObject _currentBoard;

        [ContextMenu("Play a move")]
        public bool CalculateMove()
        {
            return CalculateMove(_currentBoard);
        }

        public bool CalculateMove(BoardObject boardObject)
        {
            if (boardObject == null || _currentBoard == null)
            {
                Debug.LogError("No valid board found", gameObject);
                return false;
            }

            if (boardObject == null) boardObject = _currentBoard;

            Piece win = Piece.None;

            if (!MovesLeft(boardObject.board) || (win = HasWin(boardObject.board)) != Piece.None)
            {
                switch (win)
                {
                    case Piece.XPiece:
                        Debug.Log("X has won");
                        return false;
                        break;
                    case Piece.None:
                        Debug.Log("No available moves left");
                        return false;
                        break;
                    case Piece.OPiece:
                        Debug.Log("O has won");
                        return false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            var pos = MiniMax(Board.Clone(boardObject.board), (int) _difficulty, true);

            if (pos.Item2 != null)
            {
                boardObject.board = DoMove(boardObject.board, pos.Item2, _role);
            }

            return true;
        }

        private Board DoMove(Board board, Tuple<int, int> move, Piece role)
        {
            var newBoard = Board.Clone(board);

            newBoard.Pieces[move.Item1, move.Item2] = role;

            return newBoard;
        }

        /// <summary>
        /// A function to determine what move to play of tic tac toe based on the minimax algorithm
        /// </summary>
        /// <param name="board">The state of the board to be used to find a move</param>
        /// <param name="depth">The remaining depth left of the algorithm</param>
        /// <returns></returns>
        public Tuple<int, Tuple<int, int>> MiniMax(Board board, int depth, bool isMax)
        {
            int score = 0;
            Tuple<int, int> move = null;

            //evaluate the score of the current board
            score = EvaluateBoard(board, (isMax ? _role : (Piece) ((int) _role * -1)));

            //if there is no moves left or someone has one, return the score
            if (!MovesLeft(board) || depth == 0) return new Tuple<int, Tuple<int, int>>(score, move);

            if (isMax)
            {
                Tuple<int, Tuple<int, int>> best = new Tuple<int, Tuple<int, int>>(int.MinValue, move);
                for (int i = 0; i < board.Size; i++)
                {
                    for (int j = 0; j < board.Size; j++)
                    {
                        if (board.Pieces[i, j] == Piece.None)
                        {
                            var temp = MiniMax(
                                DoMove(board, new Tuple<int, int>(i, j), (isMax ? _role : (Piece) ((int) _role * -1))),
                                depth - 1, !isMax);

                            if (temp.Item1 > best.Item1)
                            {
                                best = new Tuple<int, Tuple<int, int>>(temp.Item1, new Tuple<int, int>(i, j));
                            }
                        }
                    }
                }

                return best;
            }

            else
            {
                Tuple<int, Tuple<int, int>> best = new Tuple<int, Tuple<int, int>>(int.MaxValue, move);
                for (int i = 0; i < board.Size; i++)
                {
                    for (int j = 0; j < board.Size; j++)
                    {
                        if (board.Pieces[i, j] == Piece.None)
                        {
                            var temp = MiniMax(
                                DoMove(board, new Tuple<int, int>(i, j), (isMax ? _role : (Piece) ((int) _role * -1))),
                                depth - 1, !isMax);

                            if (temp.Item1 < best.Item1)
                            {
                                best = new Tuple<int, Tuple<int, int>>(temp.Item1, new Tuple<int, int>(i, j));
                            }
                        }
                    }
                }

                return best;
            }

            //move = new Tuple<int, int>(Random.Range(0, board.Size), Random.Range(0, board.Size));

            return new Tuple<int, Tuple<int, int>>(score, move);
        }

        [ContextMenu("Evaluate Board")]
        private void EvaluateBoard()
        {
            Debug.Log("Board Value is " + EvaluateBoard(_currentBoard.board, _role));
        }


        private Piece HasWin(Board board)
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


        private bool MovesLeft(Board board)
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
    }
}