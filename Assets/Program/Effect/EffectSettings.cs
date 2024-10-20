using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;


public class EffectSettings : MonoBehaviour
{
    [Serializable]
    public class SESettings
    {
        [SerializeField] private string _seName;
        [SerializeField] private float _delay;
        public string SEName => _seName;
        public float Delay => _delay;
    }

    #region 変数

    [SerializeField,Header("始点から終点までの時間")] private float _duration;
    [FormerlySerializedAs("_frontCurve")] [SerializeField, Header("Front")] private AnimationCurve _upCurve;
    [SerializeField, Header("Right")] private AnimationCurve _rightCurve;
    [SerializeField, Header("曲がり方の倍率")] private float _magnification;
    [SerializeField, Header("効果音")] private List<SESettings> _seSettings;
    [SerializeField, Header("終点で出すEffect")] private string _onHitEffect;

    #endregion
    
    //プロパティ
    public string OnHitEffect => _onHitEffect;
    public AnimationCurve UpCurve => _upCurve;
    public AnimationCurve RightCurve => _rightCurve;
    public float Magnification => _magnification;
    public float Duration => _duration;
    public List<SESettings> SESettingsList => _seSettings;

    
    public async UniTask MoveEffectToTarget(EffectSettings effect,Vector2 targetPosition)
    {
        Vector3 startPosition = effect.transform.position;
        Vector2 startPosition2D = new Vector2(startPosition.x, startPosition.y);
        
        // 経過時間
        float elapsedTime = 0; 
        while (elapsedTime < effect.Duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / effect.Duration);
            
            Vector2 currentPosition2D = Vector2.Lerp(startPosition2D, targetPosition, progress);
            Vector2 abVec = targetPosition - startPosition2D;
            Vector2 perpendicularVec2D = Vector2.Perpendicular(abVec).normalized;
            currentPosition2D += -perpendicularVec2D
                                 * effect.RightCurve.Evaluate(progress) * effect.Magnification;
            float currentPositionY = currentPosition2D.y += effect.UpCurve.Evaluate(progress) * effect.Magnification;
            effect.transform.position = new Vector3(currentPosition2D.x, currentPositionY,startPosition.z);
            Debug.Log("移動中");
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        Destroy(gameObject);
    }
}
