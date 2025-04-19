using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [Header("Login UI")]
    [SerializeField] private GameObject nameInputDisplay;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Text errorTxt;

    [Header("Leaderboard UI")]
    [SerializeField] private GameObject leaderboardDisplay;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject rowPrefab;
    

    [Header("Loading UI")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TMP_Text loadingText;

    private string baseText = "Loading";
    private float dotTimer = 0f;

    void Start()
    {
        nameInputDisplay.SetActive(false);
        leaderboardDisplay.SetActive(false);
        loadingPanel.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {
        if (loadingPanel.activeSelf)
        {
            dotTimer += Time.deltaTime;
            int dotCount = Mathf.FloorToInt(dotTimer * 5) % 4;
            loadingText.text = baseText + new string('.', dotCount);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (leaderboardDisplay != null)
            {
                leaderboardDisplay.SetActive(false);
            }
            if (nameInputDisplay != null)
            {
                nameInputDisplay.SetActive(false);
            }
        }
    }

    public void ShowInputDisplay()
    {
        nameInputDisplay.SetActive(true);
    }

    public void SaveName()
    {
        string playerName = nameInputField.text;
        Debug.Log("Player name: " + playerName);
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("Tên người chơi không được để trống!");
            errorTxt.text = "Name must not be empty";
            return;
        }
        UpdateDisplayName(playerName);
        nameInputDisplay.SetActive(false);
        HideAllDisplay();
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        loadingPanel.SetActive(true);
        loadingSlider.value = 0;

        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameScene");
        asyncLoad.allowSceneActivation = false;

        float timer = 0f;

        while (!asyncLoad.isDone)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            loadingSlider.value = Mathf.Clamp01(timer / 1.2f);
            if (asyncLoad.progress >= 0.9f && timer >= 1.2f)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void UpdateDisplayName(string playerName)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = playerName
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request,
            result =>
            {
                Debug.Log($"[PlayFab] Cập nhật tên người chơi thành công: {result.DisplayName}");
            },
            error =>
            {
                Debug.LogError("[PlayFab] Cập nhật tên người chơi thất bại: " + error.GenerateErrorReport());
            });
    }

    public void ShowLeaderBoard()
    {
        leaderboardDisplay.SetActive(true);

        foreach (Transform child in content)
            Destroy(child.gameObject);

        var req = new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(req,
            result =>
            {
                foreach (var entry in result.Leaderboard)
                {
                    var go = Instantiate(rowPrefab, content);
                    var texts = go.GetComponentsInChildren<TMP_Text>();
                    texts[0].text = (entry.Position + 1).ToString();
                    texts[1].text = entry.DisplayName ?? entry.PlayFabId;
                    texts[2].text = entry.StatValue.ToString();
                }
            },
            error =>
            {
                Debug.LogError("[PlayFab] Lỗi lấy leaderboard: " + error.GenerateErrorReport());
            }
        );
    }

    public void OnClickOutside()
    {
        if (leaderboardDisplay != null)
        {
            leaderboardDisplay.SetActive(false);
        }

        if (nameInputDisplay != null)
        {
            nameInputDisplay.SetActive(false);
        }

    }

    private void HideAllDisplay()
    {
        if (leaderboardDisplay != null)
        {
            leaderboardDisplay.SetActive(false);
        }
        if (nameInputDisplay != null)
        {
            nameInputDisplay.SetActive(false);
        }
        GameObject.Find("PlayButton").SetActive(false);
        GameObject.Find("ShowLeaderboard").SetActive(false);

    }

    private void OnEnable()
    {
        var loginRequest = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(loginRequest,
            result => Debug.Log("[PlayFab] Anonymous login thành công"),
            error => Debug.LogError("[PlayFab] Login ẩn danh lỗi: " + error.GenerateErrorReport())
        );
    }
}

