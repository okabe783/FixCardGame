using UnityEngine;

//Title画面でセッティングするものを設定
public class TitleSetting : MonoBehaviour
{
    [SerializeField,Header("スタートボタン")] private UIButton _startButton;
    [SerializeField, Header("遊び方ボタン")] private UIButton _descriptionButton;

    [SerializeField, Header("タイトル画面で流すBGM")]private AudioClip _titleBgm;
    
    public void Start()
    {
        _descriptionButton.OnClickAddListener(OnChangeDescriptionScene);
        AudioManager.I.OnPlayBGM(_titleBgm);
    }

    //遊び方説明を表示
    //ToDo:SceneNavigationで移動？
    private static void OnChangeDescriptionScene()
    {
        //SceneManager.LoadScene("");
    }
}
