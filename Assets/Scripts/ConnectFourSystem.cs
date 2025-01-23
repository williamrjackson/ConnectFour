using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrj.ConnectFour
{
    public class ConnectFourSystem : MonoBehaviour
    {
        [SerializeField]
        private GameObject tapToConfirmLabel;
        private bool confirmLabelVisible = false;
        private bool isPlayer1Turn = true;
        public bool IsPlayer1Turn { get { return isPlayer1Turn; } }
        private bool isPaused = false;
        public bool IsPaused { get { return isPaused; } }
        public static ConnectFourSystem Instance { get; private set; }
        public string playerString => (isPlayer1Turn) ? "Player 1" : "Player 2";
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        public void SwitchPlayerTurn()
        {
            if (CheckForWin())
            {
                Debug.Log($"{playerString} Wins!");
                isPaused = true;
                Message.Show($"{playerString} Wins!", Message.MessageType.Info, 3f, () => 
                {
                    Utils.MapToCurve.Linear.Delay(4f, () => ResetGame());
                    Utils.MapToCurve.Linear.Delay(5f, () => isPaused = false);
                });
                return;
            }
            isPlayer1Turn = !isPlayer1Turn;
            GridManager.Instance.ClearActiveColumn();
            // if (isPlayer1Turn)
            // {
            //     Debug.Log("PLayer 1");
            // }
            // else
            // {
            //     Debug.Log("PLayer 2");
            // }
        }
        public void ShowTapToConfirmLabel()
        {
            if (confirmLabelVisible)
            {
                return;
            }
            tapToConfirmLabel.SetActive(true);
            confirmLabelVisible = true;
        }
        public void HideTapToConfirmLabel()
        {
            if (!confirmLabelVisible)
            {
                return;
            }
            tapToConfirmLabel.SetActive(false);
            confirmLabelVisible = false;
        }
        public void ResetGame()
        {
            isPlayer1Turn = true;
            GridManager.Instance.ClearGrid();
        }

        public bool CheckForWin()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    GridSquare.SquareState state = GridSquare.GetSquare(i, j).State;
                    if (state == GridSquare.SquareState.Empty)
                    {
                        continue;
                    }
                    if (i + 3 < 7 && GridSquare.GetSquare(i + 1, j).State == state && GridSquare.GetSquare(i + 2, j).State == state && GridSquare.GetSquare(i + 3, j).State == state)
                    {
                        return true;
                    }
                    if (j + 3 < 6)
                    {
                        if (GridSquare.GetSquare(i, j + 1).State == state && GridSquare.GetSquare(i, j + 2).State == state && GridSquare.GetSquare(i, j + 3).State == state)
                        {
                            return true;
                        }
                        if (i + 3 < 7 && GridSquare.GetSquare(i + 1, j + 1).State == state && GridSquare.GetSquare(i + 2, j + 2).State == state && GridSquare.GetSquare(i + 3, j + 3).State == state)
                        {
                            return true;
                        }
                        if (i - 3 >= 0 && GridSquare.GetSquare(i - 1, j + 1).State == state && GridSquare.GetSquare(i - 2, j + 2).State == state && GridSquare.GetSquare(i - 3, j + 3).State == state)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}