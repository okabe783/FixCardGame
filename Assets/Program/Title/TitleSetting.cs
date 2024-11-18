using UnityEngine;
using UnityEngine.SceneManagement;

// Title画面でセッティングするものを設定
public class TitleSetting : MonoBehaviour
{
    [SerializeField, Header("タイトル画面で流すBGM")] private AudioClip _titleBgm;
    [SerializeField, Header("遊び方解説UI")] private DescriptionSetting _descriptionSetting;
    [SerializeField] private GameObject _titleUI;
    private bool _isClicked;

    public void IsClicked(bool value)
    {
        _isClicked = value;
    }

    public void Start()
    {
        AudioManager.I.OnPlayBGM(_titleBgm);
        _isClicked = false;
    }

    public void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (_isClicked)
            return;
        
        // マウスを右クリックorF1をおしたとき
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.F1))
        {
            _descriptionSetting.gameObject.SetActive(true);
            _titleUI.gameObject.SetActive(false);
            _isClicked = true;
            return;
        }

        // 任意のボタンをクリックしたとき
        if (Input.anyKeyDown)
        {
            ChangeScene("InGame");
        }
    }

    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        _isClicked = true;
    }
}