using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private int MaxHeath = 10;
    [SerializeField] private int CurrentHealth = 0;

    [SerializeField] private TMPro.TMP_Text GodmodeText;
    [SerializeField] private Slider HealthSlider;

    public bool Godmode = false;

    void Start()
    {
        CurrentHealth = MaxHeath;
        HealthSlider.maxValue = MaxHeath;
        HealthSlider.value = CurrentHealth;
    }

    public void UpdateHealth(int Hp)
    {
        if(Godmode) return;

        //update health
        CurrentHealth += Hp;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHeath);

        //ui
        HealthSlider.value = CurrentHealth;

        //death
        if(CurrentHealth == 0)
        {
            SceneManager.LoadScene("MainScene");
            Time.timeScale = 0f;
            gameObject.SetActive(false);
        }
    }

    public void ToggleGodmode()
    {
        Godmode = !Godmode;
        GodmodeText.text = "GODMODE: " + Godmode;
    }
}
