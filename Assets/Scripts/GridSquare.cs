using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

namespace Wrj.ConnectFour
{
    public class GridSquare : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private int column = -1;
        [SerializeField]
        [ReadOnly]
        private int row = -1;
        [SerializeField]
        private Transform player1;
        [SerializeField]
        private Transform player2;
        public int Column { get { return column; } }
        public int Row { get { return row; } }
        public enum SquareState
        {
            Empty,
            Player1,
            Player2
        }
        private SquareState _state = SquareState.Empty;
        public SquareState State { get { return _state; } }
        public void SetPosition(int column, int row)
        {
            this.column = column;
            this.row = row;
        }
        private static List<GridSquare> _allSquares = new List<GridSquare>();
        public static List<GridSquare> AllSquares { get { return _allSquares; } }
        public static GridSquare GetSquare(int column, int row)
        {
            return _allSquares.FirstOrDefault(s => s.Column == column && s.Row == row);
        }
        public void SetState(SquareState state)
        {
            _state = state;
            if (_state == SquareState.Player1)
            {
                player1.gameObject.SetActive(true);
                player2.gameObject.SetActive(false);
            }
            else if (_state == SquareState.Player2)
            {
                player1.gameObject.SetActive(false);
                player2.gameObject.SetActive(true);
            }
            else
            {
                player1.gameObject.SetActive(false);
                player2.gameObject.SetActive(false);
            }
        }
        private void Awake()
        {
            _allSquares.Add(this);
        }
        private void OnDestroy()
        {
            _allSquares.Remove(this);
        }
    }
}
