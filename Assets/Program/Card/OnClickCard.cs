using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickCard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    #region Transform

    [SerializeField, Header("元の位置のTransform")] private CardTransform _homeTransform;
    [SerializeField, Header("移動先のTransform")] private CardTransform _targetTransform;

    #endregion

    #region Drag管理

    [SerializeField, Header("Drag可能かどうか")] private bool _isDraggable;
    [SerializeField, Header("Drag可能かのステータス")] private SelectableStatus _dragStatus;
    [SerializeField, Header("Drag中かどうか")] private bool _isDragging;

    #endregion

    #region Card管理

    [SerializeField, Header("Cardのパラメーター")] private CardSO _cardSo;
    [SerializeField, Header("Cardの情報")] private CardSettingsSO _cardSettings;
    [SerializeField, Header("Cardが移動する速度")] private float _leapSpeed;
    [SerializeField, Header("Cardが回転する速度")] private float _leapAngleSpeed;

    #endregion

    #region View

    [SerializeField, Header("CardView")] private CardView _cardView;

    #endregion
    
    [SerializeField, Header("召喚開始の閾値")] private float _summonThreshold = 0.5f;
    
    [SerializeField, Header("選択状態の表示オブジェクト")] private Transform _selectObj;

    private List<IDisposable> _disposables = new();

    public bool CanSummon => RunTimeData.I.CanSummon();

    [Flags]
    public enum SelectableStatus
    {
        None = 0, // 選択不可な状態
        Summonable = 1 << 0, // 召喚可能
        Selectable = 2 << 1, // 選択可能
    }

    public void Refresh()
    {
        _isDraggable = false;
        // 現在のPhaseを取得
        InGameFlow phase = RunTimeData.I.CurrentPhase;

        if (phase == InGameFlow.Mulligan)
        {
            // Mulliganの処理
        }
        else if (phase == InGameFlow.Wait)
        {
            // Waitの処理
        }
    }

    public void Init(CardSO cardSO, CardSettingsSO cardSettings)
    {
        _cardSo = cardSO;
        // ここでイベントの購読を解除
        //_disposables.Add();
        _cardSettings = cardSettings;
    }

    public void OnDestroy()
    {
        for (int i = 0; i < _disposables.Count; i++)
        {
            _disposables[i].Dispose();
        }
    }

    private void Update()
    {
        // ドラッグ可能かを確認
        if (_isDraggable)
        {
            if (_isDragging)
            {
                //　ターゲットを補完して移動
                transform.position = Vector3.Lerp(transform.position, _targetTransform.WorldPos, 20f * Time.deltaTime);
                // 回転をターゲット回転に補間
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_targetTransform.Rotation),
                    _leapAngleSpeed * Time.deltaTime);
            }
            else
            {
                //　カードをホーム位置まで補完して戻す
                transform.position = Vector3.Lerp(transform.position, _homeTransform.WorldPos, 20f * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_homeTransform.Rotation),
                    _leapAngleSpeed * Time.deltaTime);
            }
        }
    }

    public void SetHomeTransform(CardTransform cardTransform)
    {
        // ホーム位置を設定
        _homeTransform.Set(cardTransform);
        // ターゲット位置もホームに設定
        _targetTransform.Set(cardTransform);
    }

    public void MoveToHomeTransform(float duration = 0.5f, int flipCount = 0)
    {
        // Animの停止
        transform.DOKill();
        //　ホーム位置が設定されていなければなにもしていない
        if (_homeTransform == null) return;

        // ホーム位置に移動
        transform.DOMove(_homeTransform.WorldPos, duration, false);
        // ホーム位置のスケールを設定
        transform.DOScale(_homeTransform.Scale, duration);
    }

    //Drag可能ステータスを設定
    public void ChangeDragStatus(SelectableStatus dragStatus)
    {
        _dragStatus = dragStatus;
    }

    public void Show(CardSO so)
    {
        var phase = RunTimeData.I.CurrentPhase;
        bool canSummon = true;
        if (phase != InGameFlow.Mulligan)
        {
            canSummon = RunTimeData.I.CanSummon();
        }
        //Todo:カードの描画設定
        _cardView.Show();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //　Cardの説明を表示
        //　ダブルクリックの判定
        // Drag状態に設定
        _isDragging = true;
        Drag();
    }

    private void OnDoubleClick()
    {
        
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        //　現在のPhaseを取得
        var phase = RunTimeData.I.CurrentPhase;
        // Drag状態を終了
        _isDragging = false;
        //Todo:Fieldのカードを選択できない状態にする
        
        // Drag可能か確認
        if (_isDraggable)
        {
            // ホーム位置とターゲット位置の差を取得
            float diff = _targetTransform.WorldPos.z - _homeTransform.WorldPos.z;

            if (phase == InGameFlow.Wait)
            {
                bool flickUp = _summonThreshold < diff;

                if (flickUp)
                {
                    if (CanSummon)
                    {
                        //Todo:召喚アクションを発行
                    }
                }
            }
        }
        // ターゲット位置をホームにリセット
        _targetTransform.Set(_homeTransform);
    }

    public void OnDrag(PointerEventData pointerEventData)
    {
        // Drag中の処理を発行
        Drag();
    }

    public void Drag()
    {
        //Drag不可なら終了
        if(!_isDraggable) return;

        // 現在のPhaseを取得
        InGameFlow phase = RunTimeData.I.CurrentPhase;
        // マウス位置からrayを取得
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 地面の平面を作成
        Plane plane = new Plane(Vector3.up,Vector3.zero);

        // rayが平面の交点を取得
        if (plane.Raycast(ray, out float enter))
        {
            //rayの平面の交点を取得
            Vector3 hitPoint = ray.GetPoint(enter);
            // 現在の位置を保存
            Vector3 beforePos = _targetTransform.WorldPos;
            _targetTransform.WorldPos = new Vector3(hitPoint.x, _homeTransform.WorldPos.y);

            // 位置の差分を計算
            Vector3 diff = _targetTransform.WorldPos - beforePos;
        }
    }

    public void SetSelectable(bool selectable)
    {
        _isDraggable = selectable;
        _selectObj.gameObject.SetActive(selectable);
        if (selectable)
        {
            _selectObj.DOScale(1.1f, 0.6f).From(1f).SetLoops(-1, LoopType.Restart);
        }
    }
}