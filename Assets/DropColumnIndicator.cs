using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wrj.ConnectFour
{
    public class DropColumnIndicator : MonoBehaviour
    {
        [SerializeField]
        private int _column = -1;
        [SerializeField]
        private Transform player1;
        [SerializeField]
        private Transform player2;

        public int Column { get { return _column; } }
        private static List<DropColumnIndicator> _allIndicators = new List<DropColumnIndicator>();
        private void Awake()
        {
            _allIndicators.Add(this);
        }

        private void ShowIndicator()
        {
            if (ConnectFourSystem.Instance.IsPlayer1Turn)
            {
                player1.gameObject.SetActive(true);
                player2.gameObject.SetActive(false);
            }
            else
            {
                player1.gameObject.SetActive(false);
                player2.gameObject.SetActive(true);
            }
        }
        private void HideIndicator()
        {
            player1.gameObject.SetActive(false);
            player2.gameObject.SetActive(false);
        }
        public static void ShowIndicatorForColumn(int column)
        {
            // Debug.Log($"ShowIndicatorForColumn: {column}");
            ConnectFourSystem.Instance.ShowTapToConfirmLabel();
            foreach (DropColumnIndicator indicator in _allIndicators)
            {
                if (indicator.Column == column)
                {
                    indicator.ShowIndicator();
                }
                else
                {
                    indicator.HideIndicator();
                }
            }
        }
        public static void HideAllIndicators()
        {
            ConnectFourSystem.Instance.HideTapToConfirmLabel();
            foreach (DropColumnIndicator indicator in _allIndicators)
            {
                indicator.HideIndicator();
            }
        }
    }
}