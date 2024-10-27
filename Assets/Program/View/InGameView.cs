using Cysharp.Threading.Tasks;
using UnityEngine;

public class InGameView : MonoBehaviour
{
    // ゲームのメインセットアップ
    public async UniTask GameMainSetUp()
    {
        await DrawCard();
        await ShowActivePanel("StartPhase");
    }
    
    public async UniTask DrawCard()
    {
        InGameLogic.I.PlayerHand.ResetCard();
        await InGameLogic.I.AddCardToHand();
    }
    
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
    
    public async UniTask ShowEffect(string effectName,Vector2 position)
    {
        EffectSettings effectPrefab = Resources.Load<EffectSettings>("Motion/" + effectName);
        if (effectPrefab == null)
        {
            Debug.LogError("Effectが見つかりません");
            return;
        }
        EffectSettings effectInstance = Instantiate(effectPrefab,position,Quaternion.identity);
        await effectInstance.SetParticle(effectInstance);
    }

    public void ChangeHPBar(int hp,int playerIndex)
    {
        var hpSettings = FindObjectOfType<HPSettings>();

        switch (playerIndex)
        {
            case 0:
                hpSettings.UpdateEnemyHPText(hp);
                break;
            case 1:
                hpSettings.UpdatePlayerHPText(hp);
                break;
            default:
                Debug.LogError("無効です");
                break;
        }
    }
}