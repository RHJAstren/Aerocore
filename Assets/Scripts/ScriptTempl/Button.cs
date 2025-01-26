using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Coroutine moveCoroutine;
    private Vector3 originalPosition;
    internal object onClick;

    private void Start() { originalPosition = transform.position; }

    public virtual void HandleButton() { Debug.Log("Button Clicked"); }

    public void OnPointerEnter(PointerEventData eventData) { StartMoveCoroutine(originalPosition + new Vector3(20.0f, 10.0f, 0.0f), 0.2f); }

    public void OnPointerExit(PointerEventData eventData) { StartMoveCoroutine(originalPosition, 0.2f); }

    private void StartMoveCoroutine(Vector3 targetPosition, float duration) {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveOverTime(targetPosition, duration));
    }

    private IEnumerator MoveOverTime(Vector3 targetPosition, float duration) {
        Vector3 startPos = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration) {
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        moveCoroutine = null;
    }
}
