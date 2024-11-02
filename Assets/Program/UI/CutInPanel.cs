using UnityEngine;
using DG.Tweening;

public class CutInPanel : MonoBehaviour
{
    [SerializeField] private GameObject _speedLineObject;
    [SerializeField] private float _slideInDuration;
    [SerializeField] private float _slideOutDuration;
    [SerializeField] private float _waitTime;

    private RectTransform _speedLineRectTransform; 
    private Vector2 _initialSpeedLinePosition;

    private void Start()
    {
        _speedLineRectTransform = _speedLineObject.GetComponent<RectTransform>();
        _initialSpeedLinePosition = new Vector2(-110, 0);
        _speedLineRectTransform.anchoredPosition = _initialSpeedLinePosition;
        SlideIn();
    }

    private void SlideIn()
    {
        _speedLineObject.SetActive(true);
        _speedLineRectTransform.DOAnchorPosX(0, _slideInDuration).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            DOVirtual.DelayedCall(_waitTime, SlideOut);
        });
    }

    private void SlideOut()
    {
        _speedLineRectTransform.DOAnchorPosX(Screen.width, _slideOutDuration).SetEase(Ease.InExpo).OnComplete(ResetPosition);
    }

    private void ResetPosition()
    {
        _speedLineRectTransform.anchoredPosition = _initialSpeedLinePosition;
        _speedLineObject.SetActive(false);
    }
}