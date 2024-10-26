using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Viewの管理
/// エフェクトやボイスなどを管理する
/// </summary>
public class InGameView : MonoBehaviour
{
    // ゲームのメインセットアップ
    public async UniTask GameMainSetUp()
    {
        // ToDo:敵のAnimationを実装
        // 手札を配る
        await InGameLogic.I.AddCardToHand();
        // StartPanelをアクティブにする
        await ShowActivePanel("StartPhase");
    }
    
    //Panelを表示する
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
}
