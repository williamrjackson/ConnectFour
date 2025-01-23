using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System;
using System.Linq;

namespace Wrj.ConnectFour
{
    [RequireComponent(typeof(GridLayoutGroup))]    
    public class GridManager : MonoBehaviour
    {
        [SerializeField]
        private GridLayoutGroup _gridLayoutGroup;
        public GridLayoutGroup gridLayoutGroup
        {
            get
            {
                if (_gridLayoutGroup == null)
                {
                    _gridLayoutGroup = GetComponent<GridLayoutGroup>();
                }
                return _gridLayoutGroup;
            }
        }
        [SerializeField]
        private GridSquare gridSquarePrefab;
        [Button("SetGrid")]
        public void SetGrid()
        {
            int numberOfSquaresAcross = 7;
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 size = rectTransform.rect.size;
            float w = Mathf.Min(size.x, size.y);
            float h = Mathf.Max(size.x, size.y);
            rectTransform.pivot = rectTransform.anchorMin = rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = new Vector2(0, h / -4);
            float cellSize = w / numberOfSquaresAcross;
            gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
            gridLayoutGroup.spacing = new Vector2(0, 0);
        }
        [Button("CreateGrid")]
        public void CreateGridButton()
        {
            var existingSquares = FindObjectsByType<GridSquare>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Debug.Log($"Found {existingSquares.Length} existing squares");
            foreach (GridSquare square in existingSquares)
            {
                if (square == gridSquarePrefab) continue;
                DestroyImmediate(square.gameObject);
            }
            CreateGrid();
        }
        public void CreateGrid()
        {
            gridSquarePrefab.gameObject.SetActive(false);
            int numberOfSquaresAcross = 7;
            int numberOfSquaresDown = 6;

            if (GridSquare.AllSquares.Count != numberOfSquaresAcross * numberOfSquaresDown)
            {
                foreach (GridSquare square in GridSquare.AllSquares)
                {
                    DestroyImmediate(square.gameObject);
                }
            }
            for (int i = 0; i < numberOfSquaresAcross * numberOfSquaresDown; i++)
            {
                GridSquare square = Instantiate(gridSquarePrefab, transform);
                square.gameObject.SetActive(true);
                square.SetPosition(i % numberOfSquaresAcross, i / numberOfSquaresAcross);
                square.name = $"Square {square.Column}, {square.Row}";
            }
        }
        public void ClearGrid()
        {
            foreach (GridSquare square in GridSquare.AllSquares)
            {
                square.SetState(GridSquare.SquareState.Empty);
            }
        }
        public float GetColumnWidth()
        {
            return gridLayoutGroup.cellSize.x;
        }
        private int _activeColumn = -1;
        public int ActiveColumn
        {
            get { return _activeColumn; }
            set
            {
                if (_activeColumn != value)
                {
                    if (GridSquare.AllSquares.Where(s => s.Column == value && s.State == GridSquare.SquareState.Empty).Count() == 0)
                    {
                        return;
                    }
                    _activeColumn = value;
                    DropColumnIndicator.ShowIndicatorForColumn(_activeColumn);
                }
            }
        }
        public void ClearActiveColumn()
        {
            _activeColumn = -1;
            DropColumnIndicator.HideAllIndicators();
        }
        private static GridManager _instance;
        public static GridManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GridManager>();
                }
                return _instance;
            }
        }
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            SetGrid();
            ScreenSizeNotifier.Instance.OnScreenChange += SetGrid;
        }

        private void SetGrid(Vector2 screenDimensions)
        {
            SetGrid();
        }

        public void DropPiece()
        {
            if (ActiveColumn == -1)
            {
                return;
            }
            int row = 5;
            while (row >= 0)
            {
                GridSquare nextSquare = GridSquare.GetSquare(ActiveColumn, row);
                if (nextSquare.State == GridSquare.SquareState.Empty)
                {
                    nextSquare.SetState(ConnectFourSystem.Instance.IsPlayer1Turn ? GridSquare.SquareState.Player1 : GridSquare.SquareState.Player2);
                    ActiveColumn = -1;
                    DropColumnIndicator.HideAllIndicators();
                    ConnectFourSystem.Instance.HideTapToConfirmLabel();
                    ConnectFourSystem.Instance.SwitchPlayerTurn();
                    return;
                }
                row--;
            }
        }
    }
}
