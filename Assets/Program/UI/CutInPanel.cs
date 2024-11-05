using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class CutInPanel : MonoBehaviour
{
    [SerializeField] private GameObject _speedLineObject;
    [SerializeField] private Image _icon;
    [SerializeField] private float _slideInDuration;
    [SerializeField] private float _slideOutDuration;
    [SerializeField] private float _waitTime;
    [SerializeField] private TextMeshProUGUI _text;

    private RectTransform _speedLineRectTransform;
    private Vector2 _initialSpeedLinePosition;

    private void Start()
    {
        _speedLineRectTransform = _speedLineObject.GetComponent<RectTransform>();
        _initialSpeedLinePosition = new Vector2(-110, 0);
        _speedLineRectTransform.anchoredPosition = _initialSpeedLinePosition;
    }

    public async UniTask SlideIn(Image icon,string text)
    {
        _speedLineObject.SetActive(true);
        _text.text = $"{text}の攻撃";
        _icon.sprite = icon.sprite;

        // スライドインを待機
        await _speedLineRectTransform.DOAnchorPosX(0, _slideInDuration)
            .SetEase(Ease.OutExpo)
            .ToUniTask();

        // 指定の待機時間を待機
        await UniTask.Delay(System.TimeSpan.FromSeconds(_waitTime));

        // スライドアウトを待機
        await SlideOut();
    }

    private async UniTask SlideOut()
    {
        await _speedLineRectTransform.DOAnchorPosX(Screen.width, _slideOutDuration).SetEase(Ease.InExpo)
            .OnComplete(ResetPosition);
    }

    private void ResetPosition()
    {
        _speedLineRectTransform.anchoredPosition = _initialSpeedLinePosition;
        _speedLineObject.SetActive(false);
    }
}