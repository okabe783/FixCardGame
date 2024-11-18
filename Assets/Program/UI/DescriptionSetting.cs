using UnityEngine;

// 遊び方説明を表示するクラス
public class DescriptionSetting　: MonoBehaviour
{
    [SerializeField] private GameObject[] _descriptionUI;
    [SerializeField] private TitleSetting _titleSetting;
    [SerializeField] private GameObject _background;

    private void Start()
    {
        _descriptionUI[0].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackTitle();
        }
    }

    private void BackTitle()
    {
        _background.SetActive(true);
        gameObject.SetActive(false);
        _titleSetting.IsClicked(false);
    }

    // Panelを切り替える処理
    public void ShowNextPanel(int panelIndex)
    {
        if (_descriptionUI[panelIndex] == null)
            Debug.LogError("有効なPanelが存在しません");

        _descriptionUI[panelIndex].SetActive(true);

        if (panelIndex != 0)
        {
            _descriptionUI[panelIndex - 1].SetActive(false);
        }
    }
}