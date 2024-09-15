using UnityEngine;
using UnityEngine.SceneManagement;

//Title画面でセッティングするものを設定
public class TitleSetting : MonoBehaviour
{
    [SerializeField,Header("スタートボタン")] private UIButton _startButton;
    [SerializeField, Header("遊び方ボタン")] private UIButton _descriptionButton;

    [SerializeField, Header("タイトル画面で流すBGM")]private AudioClip _titleBgm;
    
    public void Start()
    {
        _startButton.OnClickAddListener(OnChangeStartScene);
        _descriptionButton.OnClickAddListener(OnChangeDescriptionScene);
        AudioManager.I.OnPlayBGM(_titleBgm);
    }
    
    //Gameをスタート
    private static void OnChangeStartScene()
    {
        SceneManager.LoadScene("InGame");
    }

    //遊び方説明を表示
    //ToDo:SceneNavigationで移動？
    private static void OnChangeDescriptionScene()
    {
        //SceneManager.LoadScene("");
    }
}
