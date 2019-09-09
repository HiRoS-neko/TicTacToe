using System;
using UnityEngine;
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
        public void CalculateMove()
        {
            CalculateMove(_currentBoard);
        }

        public void CalculateMove(BoardObject boardObject)
        {
            if (boardObject == null || _currentBoard == null)
            {
                Debug.LogError("No valid board found", gameObject);
                return;
            }

            if (boardObject == null) boardObject = _currentBoard;

            var pos = MiniMax(boardObject.board, (int) _difficulty, true);

            if (pos != null)
            {
                boardObject.board.Pieces[pos.Item1, pos.Item2] = _role;
            }
        }

        /// <summary>
        /// A function to determine what move to play of tic tac toe based on the minimax algorithm
        /// </summary>
        /// <param name="board">The state of the board to be used to find a move</param>
        /// <param name="depth">The remaining depth left of the algorithm</param>
        /// <returns></returns>
        public Tuple<int, int> MiniMax(Board board, int depth, bool isMax)
        {
            Tuple<int, int> move = null;


            //move = new Tuple<int, int>(Random.Range(0, board.Size), Random.Range(0, board.Size));

            return move;
        }

        [ContextMenu("Evaluate Board")]
        private void EvaluateBoard()
        {
            Debug.Log("Board Value is " + EvaluateBoard(_currentBoard.board, _role));
        }


        private Piece HasWin()
        {
            return Piece.None;
        }


        private bool MovesLeft(Board board)
        {
            bool available = false;

            //check for win conditions already
            if (HasWin() != Piece.None) return false;

            //check for available spots left
            foreach (var boardPiece in board.Pieces)
            {
                if (boardPiece == Piece.None)
                {
                    available = true;
                    break;
                }
            }

            return true;
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