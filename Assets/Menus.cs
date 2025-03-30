using UnityEngine;
using DG.Tweening;
using TMPro;

public class Menus : MonoBehaviour
{

    [SerializeField] CanvasGroup menu;
    [SerializeField] CanvasGroup corner;
    [SerializeField] CanvasGroup instructions;
    [SerializeField] TextMeshProUGUI text;

    [SerializeField]
    OutputAssesser assesser;

    public static bool hasGameStarted = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasGameStarted = false;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        text.text = assesser.NumberOfSlices.ToString();
        if (hasGameStarted)
            return;

        if (Input.anyKeyDown)
        {
            hasGameStarted = true;
            menu.DOFade(0f, .5f);
            corner.DOFade(1f, .5f);
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(5f);
            sequence.Append(instructions.DOFade(0f, 1.5f));
        }
    }
}
