using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuTextEffect  : MonoBehaviour
{
    public TMP_Text text;
    public float minSize = 5f;
    public float maxSize = 10f;
    public float duration = 1f;

    private void Start()
    {
        StartCoroutine(AnimateTextSize());
    }

    private System.Collections.IEnumerator AnimateTextSize()
    {
        while (true)
        {
            yield return StartCoroutine(ScaleTextSize(minSize, maxSize, duration));
            yield return StartCoroutine(ScaleTextSize(maxSize, minSize, duration));
        }
    }

    private System.Collections.IEnumerator ScaleTextSize(float startSize, float targetSize, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            float newSize = Mathf.Lerp(startSize, targetSize, elapsedTime / time);
            text.fontSize = Mathf.RoundToInt(newSize);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.fontSize = Mathf.RoundToInt(targetSize);
    }
}
