using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject PlayerHUD;

    private void Start()
    {
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        MainMenu.SetActive(false);
        PlayerHUD.SetActive(true);
    }
}
