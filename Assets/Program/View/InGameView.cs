using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class InGameView : MonoBehaviour
{
    [SerializeField] private GameEndPanel _gameEndPanel;
    [SerializeField] private CutInPanel _cutInPanel;
    
    public async UniTask ShowActivePanel(string panelName)
    {
        PhasePanel panelPrefab = Resources.Load<PhasePanel>("Panel/CurrentPhasePanel");

        if (panelPrefab == null)
        {
            Debug.LogError("Resourcesが存在しません");
            return;
        }

        PhasePanel panelInstance = Instantiate(panelPrefab, transform);
        panelInstance.UpdatePanelText(panelName);
        await UniTask.Delay(1000);
        Destroy(panelInstance.gameObject);
    }

    public async UniTask ShowGameEndPanel(string panelText)
    {
        _gameEndPanel.ActiveGameEndPanel(panelText);
        await UniTask.Delay(2000);
    }

    public async UniTask ShowEffect(string effectName,Vector2 startPosition,bool isEnemyAttack)
    {
        await ShowEffect(effectName, startPosition, startPosition,isEnemyAttack);
    }
    
    public async UniTask ShowEffect(string effectName,Vector2 startPosition,Vector2 targetPosition,bool isEnemyAttack)
    {
        // Effectをロードする
        EffectSettings effectPrefab = Resources.Load<EffectSettings>("Motion/" + effectName);
        
        if (effectPrefab == null)
        {
            Debug.LogError("Effectが見つかりません");
            return;
        }
        
        Quaternion rotation = Quaternion.Euler(isEnemyAttack ? 180 : 0, 0, 0); 

        // Effectを生成
        EffectSettings effectInstance = Instantiate(effectPrefab,startPosition,effectPrefab.transform.rotation * rotation);

        // ヒットエフェクトが指定されていれば生成
        if (!string.IsNullOrEmpty(effectPrefab.OnHitEffect))
        {
            await effectInstance.MoveEffectToTarget(effectInstance, targetPosition);
            EffectSettings hitEffectPrefab = Resources.Load<EffectSettings>("Motion/" + effectPrefab.OnHitEffect);
            if (hitEffectPrefab != null)
            {
                EffectSettings hitEffectInstance = Instantiate(hitEffectPrefab, targetPosition, hitEffectPrefab.transform.rotation);
                await hitEffectInstance.SetParticle(hitEffectInstance);
            }
            else
            {
                Debug.LogError($"ヒットエフェクト '{effectPrefab.OnHitEffect}' が見つかりません。パスを確認してください。");
            }
        }
        else
        {
            await effectInstance.SetParticle(effectInstance);
        }
    }
    
    public async UniTask ShowCutInAnimation(Image icon, string characterType)
    {
        if (_cutInPanel == null)
        {
            Debug.LogError("CutInPanelがアタッチされていません");
            return;
        }
        
        await _cutInPanel.SlideIn(icon, characterType);
    }

    public void ChangeHPBar(int hp,int playerIndex)
    {
        var hpSettings = FindObjectOfType<HPSettings>();

        switch (playerIndex)
        {
            case 0:
                hpSettings.UpdatePlayerHPText(hp);
                break;
            case 1:
                hpSettings.UpdateEnemyHPText(hp);
                break;
            default:
                Debug.LogError("無効です");
                break;
        }
    }
}