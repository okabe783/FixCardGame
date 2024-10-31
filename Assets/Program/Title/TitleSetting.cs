using UnityEngine;

//Title画面でセッティングするものを設定
public class TitleSetting : MonoBehaviour
{
    [SerializeField, Header("タイトル画面で流すBGM")]private AudioClip _titleBgm;
    
    public void Start()
    {
        AudioManager.I.OnPlayBGM(_titleBgm);
    }
}