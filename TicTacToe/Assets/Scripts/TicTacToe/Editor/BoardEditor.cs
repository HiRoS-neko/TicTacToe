using UnityEditor;
using UnityEngine;

namespace TicTacToe.Editor
{
    [CustomEditor(typeof(BoardObject))]
    public class BoardEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var boardObject = target as BoardObject;

            var board = boardObject.board;

            board.Size = EditorGUILayout.IntSlider("Size", board.Size, 3, 20);

            if (GUILayout.Button("Initialize Board"))
            {
                board.Pieces = new Piece[board.Size, board.Size];
            }

            if (board.Pieces != null)
            {
                for (int i = 0; i < board.Pieces.GetLength(0); i++)
                {
                    GUILayout.BeginHorizontal();
                    for (int j = 0; j < board.Pieces.GetLength(1); j++)
                    {
                        board.Pieces[i, j] = (Piece) EditorGUILayout.EnumPopup(board.Pieces[i, j]);
                    }

                    GUILayout.EndHorizontal();
                }
            }

            boardObject.board = board;
        }
    }
}