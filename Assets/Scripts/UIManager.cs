using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField]
    GameObject swipeUI;
    [SerializeField]
    GameObject gameOverUI;
    [SerializeField]
    GameObject nextLevelUI;
    [SerializeField]
    GameObject scoreUI;
    private void Awake()
    {
        gameManager = GameManager.Instance;
    }
    private void OnEnable()
    {
        gameManager.GameOverEvent += GameOverUI;
        gameManager.GameReadyEvent += GameReadyUI;
        gameManager.GameResetEvent += GameResetUI;
        gameManager.GameNextLevelEvent += GameNextLevelUI;
        gameManager.CollectableHit += CollectableUpdate;
    }
    private void OnDisable()
    {
        gameManager.GameOverEvent -= GameOverUI;
        gameManager.GameReadyEvent -= GameReadyUI;
        gameManager.GameResetEvent -= GameResetUI;
        gameManager.GameNextLevelEvent -= GameNextLevelUI;
        gameManager.CollectableHit -= CollectableUpdate;
    }

    void CollectableUpdate(int score)
    {
        scoreUI.GetComponent<Text>().text = "Score: " + score;
    }
    void GameOverUI()
    {
        gameOverUI.SetActive(true);
    }
    void GameReadyUI()
    {
        swipeUI.SetActive(false);
    }
    void GameResetUI()
    {
        gameOverUI.SetActive(false);
    }
    void GameNextLevelUI()
    {
        nextLevelUI.SetActive(true);
    }
    public void GameNextLevelContinueUI()
    {
        nextLevelUI.SetActive(false);
    }
}
