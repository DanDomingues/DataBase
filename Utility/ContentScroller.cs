using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentScroller : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    GameObject contentPrefab;
    [SerializeField]
    RectTransform contentHost;
    [SerializeField]
    Image prevCryptsButton;
    [SerializeField]
    Image nextCryptsButton;

    [Header("Adjustments")]
    [SerializeField, Range(0, 300)]
    float maxOffset;
    [SerializeField, Range(0f, 5.0f)]
    float swipeMultiplier = 1.0f;
    [SerializeField, Range(0.0f, 500.0f), Tooltip("Speed of position adjustment, after drag is released. Measured in canvas units/second")]
    float positionAdjustSpeed = 10.0f;

    [Header("Movement Sequence")]
    [SerializeField]
    AnimationCurve moveCurve;
    [SerializeField, Range(0.0f, 3.0f)]
    float movementDuration = 1.0f;
    [SerializeField,Range(0, 10)]
    int movementAmount;

    [Header("Display")]
    [SerializeField]
    int currentIndex;
    [SerializeField]
    int shownElementsCap;
    [SerializeField]
    int totalElements;

    private float clampMin;
    private float clampMax;

    private Vector2 lastCursorPosition;

    private float elementWidth;

    private Coroutine moveCorout;
    private Coroutine adjustCorout;

    private float ClampMax { get { return clampMax + maxOffset; } }
    private Vector2 Cursor { get
        {
            Vector2 cursor = Vector2.zero;

            if(Input.GetMouseButton(0))
            {
                cursor = Input.mousePosition;
            }
            if(Input.touches.Length > 0)
            {
                cursor = Input.touches[0].position;
            }

            return cursor;

        } }

    private bool HostInBounds
    {
        get
        {
            float x = contentHost.anchoredPosition.x;
            return x > -clampMax && x < clampMin;

        }
    }

    [SerializeField]
    ScoreCompare[] shownCrypts;

#if UNITY_EDITOR
    [Header("Editor")]
    [SerializeField]
    int randomMin = 5;
    [SerializeField]
    int randomMax = 15;
    [SerializeField]
    bool generateRandom;
#endif


    void UpdateIndex()
    {
        float normalizedPos = (contentHost.anchoredPosition.x - clampMin) * -1;
        currentIndex =  (int) (normalizedPos / elementWidth);
        currentIndex = Mathf.Clamp(currentIndex, 0, totalElements - shownElementsCap);

        UpdateButtons();
    }

    void UpdateButtons()
    {
        prevCryptsButton.enabled =  currentIndex > 0;
        nextCryptsButton.enabled = (currentIndex + shownElementsCap) < totalElements;

    }

    Vector2 GetCorrectedPosition(int index, bool getClosest)
    {
        float targetX = -clampMin + (elementWidth * index);
        float overlapX = targetX - Mathf.Abs(contentHost.anchoredPosition.x);
        float iconWidth = contentPrefab.GetComponent<RectTransform>().sizeDelta.x;

        if(getClosest)
        {
            if (Mathf.Abs(overlapX) > iconWidth / 2)
            {
                //Debug.Log(string.Format("Base {0} with offset {1}", targetX, targetX + elementWidth));
                targetX += elementWidth;
                currentIndex++;

            }

        }

        targetX = Mathf.Clamp(targetX * -1, -clampMax, clampMin);

        Vector2 correctedPosition = new Vector2(targetX, contentHost.anchoredPosition.y);
        return correctedPosition;

    }

    Vector2 GetCorrectedPosition()
    {
        return GetCorrectedPosition(currentIndex, true);
    }

    void AdjustPosition()
    {
        if (adjustCorout != null) StopCoroutine(adjustCorout);

        adjustCorout = StartCoroutine(AdjustPosition_Routine(GetCorrectedPosition()));

    }

    IEnumerator AdjustPosition_Routine(Vector2 targetPosition)
    {
        Vector2 startPosition = contentHost.anchoredPosition;
        int way = targetPosition.x > contentHost.anchoredPosition.x ? 1 : -1;

        while(Utility.Compare(targetPosition.x, contentHost.anchoredPosition.x, way))
        {
            Vector2 projPosition = contentHost.anchoredPosition + (Vector2.right * positionAdjustSpeed * way * Time.fixedDeltaTime);

            if(Vector2.Distance(projPosition, targetPosition) < Vector2.Distance(contentHost.anchoredPosition, targetPosition))
            {
                contentHost.anchoredPosition = projPosition;
            }
            else
            {
                contentHost.anchoredPosition = targetPosition;
                break;
            }

            yield return new WaitForEndOfFrame();

        }

        adjustCorout = null;
        UpdateButtons();

    }

    public void MoveHost(int way)
    {
        if (way == 0) return;
        if (moveCorout != null) return;
        if (totalElements < shownElementsCap) return;

        int rawTargetIndex = currentIndex + (movementAmount * way);

        if (rawTargetIndex == currentIndex) return;

        Debug.Log(string.Format("Current {0} Target {1} Clamped {2}",
            currentIndex, currentIndex + (shownElementsCap * way), rawTargetIndex));

        moveCorout = StartCoroutine(MoveHost_Routine(rawTargetIndex));

    }

    IEnumerator MoveHost_Routine(int rawTargetIndex)
    {
        int targetIndex = Mathf.Clamp(rawTargetIndex, 0, totalElements - shownElementsCap);

        float indexDiff = Mathf.Abs(rawTargetIndex - targetIndex);
        float durationFactor = ((shownElementsCap - indexDiff) / shownElementsCap);

        currentIndex = targetIndex;

        float start = contentHost.anchoredPosition.x;
        float target = GetCorrectedPosition(targetIndex, false).x;
        float distance = target - start;

        float t = 0f;
        float curveFactor = 0.0f;

        while (t < 1.0f)
        {
            curveFactor = moveCurve.Evaluate(t);
            contentHost.anchoredPosition = new Vector2(start + (distance * curveFactor), contentHost.anchoredPosition.y);

            t += Time.fixedDeltaTime / (movementDuration * durationFactor);

            yield return new WaitForEndOfFrame();
        }

        contentHost.anchoredPosition = new Vector2(target, contentHost.anchoredPosition.y);
        moveCorout = null;
        UpdateButtons();

    }

    public void OnPointerBeginDrag()
    {
        lastCursorPosition = Cursor;
    }

    public void OnPointerDrag()
    {
        if (moveCorout != null) return;
        if (totalElements < shownElementsCap) return;
        if (adjustCorout != null) StopCoroutine(adjustCorout);

        Vector2 cursor = Cursor;

        float factor = Screen.width / 1080f;
        float moveAmount = (cursor.x - lastCursorPosition.x) / factor * swipeMultiplier;
        float xPos = contentHost.anchoredPosition.x + moveAmount;

        if (xPos < -clampMax || xPos > clampMin)
        {
            float extraAmount = 0;

            if (xPos < -ClampMax) extraAmount = xPos + ClampMax;
            if (xPos > clampMin) extraAmount = xPos - clampMin;
            extraAmount = Mathf.Abs(extraAmount);

            float extraFactor = 1 - Mathf.Clamp01(extraAmount / maxOffset);
            //Debug.Log(string.Format("Factor {0} Extra {1} Max {2}",
            //            extraFactor.ToString("0.00"), extraAmount, maxOffset));

            if (extraFactor > 0.1f)
                moveAmount *= extraFactor;

            else
                moveAmount = 0;

        }

        xPos = contentHost.anchoredPosition.x + moveAmount;

        //xPos = Mathf.Clamp(xPos, -clampMax, clampMin);
        contentHost.anchoredPosition = new Vector2(xPos, contentHost.anchoredPosition.y);

        lastCursorPosition = cursor;
        UpdateIndex();

    }

    public void OnPointerUp()
    {
        if (totalElements < shownElementsCap) return;
        adjustCorout = InvokeAlternatives.InvokeRealtime(this, HostInBounds ? 0.5f : 0.0f, AdjustPosition);
    }

    #region Setup Methods
    public virtual void Setup(ScoreCompare[] scores, CryptAssemblyData cryptAssembly)
    {
        shownCrypts = scores;

        totalElements = scores.Length;
        int placing;
        GameObject crypt;
        for (int i = 0; i < scores.Length; i++)
        {
            if(contentHost.childCount > i)
            {
                crypt = contentHost.GetChild(i).gameObject;
            }
            else
            {
                crypt = Instantiate(contentPrefab, contentHost);
            }

            crypt.GetComponentInChildren<Text>().text = scores[i].value.ToString("000");

            var usedSettings = scores[i].playerImage != null ? cryptAssembly.fbSettings : cryptAssembly.settings;
            var frame = crypt.transform.Find("Frame").GetComponent<Image>();
            placing = Mathf.Clamp(scores[i].placing, 0, 3);

            frame.sprite = usedSettings[placing].wholeSprite;

            if(scores[i].playerImage != null)
            {
                crypt.transform.Find("Mask").Find("Avatar").GetComponent<Image>().sprite = scores[i].playerImage;
            }

        }

        for (int i = totalElements; i < contentHost.childCount; i++)
        {
            contentHost.GetChild(i).gameObject.SetActive(false);
        }

        SetupBounds();

    }

    void SetupBounds()
    {
        elementWidth = contentPrefab.GetComponent<RectTransform>().sizeDelta.x;
        elementWidth += contentHost.GetComponent<HorizontalLayoutGroup>().spacing;

        clampMin = contentHost.anchoredPosition.x;
        clampMax = (elementWidth * (totalElements - shownElementsCap)) - clampMin;

        UpdateButtons();

    }

    #endregion

}
