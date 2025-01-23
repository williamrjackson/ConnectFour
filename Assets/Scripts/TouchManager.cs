using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Wrj.ConnectFour
{
    public class TouchManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        [SerializeField]
        private float tapDuration = 0.2f;
        [SerializeField]
        private CanvasScaler canvasScaler;
        private bool isPointerDown = false;
        private float touchTime = 0;
        private Coroutine activeColumnCoroutine;
        private float RemapScaledX(float x)
        {
            return x.Remap(0, Screen.width, 0, canvasScaler.referenceResolution.x);
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            isPointerDown = true;
            touchTime = Time.time;
            var xTouch = eventData.position.x;
            activeColumnCoroutine = StartCoroutine(DelayedActiveColumn(xTouch));
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (Time.time - touchTime < tapDuration && GridManager.Instance.ActiveColumn != -1)
            {
                GridManager.Instance.DropPiece();
            }
            if (activeColumnCoroutine != null)
            {
                StopCoroutine(activeColumnCoroutine);
            }
            isPointerDown = false;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (isPointerDown)
            {
                var xTouch = eventData.position.x;
                SetActiveColumnByTouch(xTouch);
            }
        }

        private void SetActiveColumnByTouch(float xTouch)
        {
            // Get column from touch position
            Debug.Log("xTouch: " + xTouch);
            xTouch = RemapScaledX(xTouch);
            Debug.Log($"RemapScaledX: {xTouch}");
            for (int i = 0; i < 7; i++)
            {
                if (xTouch - GridManager.Instance.GetColumnWidth() < i * GridManager.Instance.GetColumnWidth())
                {
                    GridManager.Instance.ActiveColumn = i;
                    break;
                }
            }
        }
        private IEnumerator DelayedActiveColumn(float xTouch)
        {
            yield return new WaitForSeconds(tapDuration);
            SetActiveColumnByTouch(xTouch);
        }
    }
}