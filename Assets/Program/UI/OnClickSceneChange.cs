using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickSceneChange : MonoBehaviour
{
    private UIButton _uiButton;
    [SerializeField] private string _sceneName;

    private void Awake()
    {
        _uiButton = GetComponent<UIButton>();
    }
    private void Start()
    {
        _uiButton.OnClickAddListener(OnChangeScene);
    }

    private void OnChangeScene()
    {
        SceneManager.LoadScene(_sceneName);
    }
}
