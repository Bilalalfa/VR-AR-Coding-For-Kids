using UnityEngine;

public class Unit1MenuController : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject kelasPanel;
    public GameObject buttonKelas;
    public GameObject buttonGame;

    private void Start()
    {
        if (kelasPanel != null)
            kelasPanel.SetActive(false);
    }

    public void OpenKelasMenu()
    {
        if (kelasPanel != null)
            kelasPanel.SetActive(true);

        if (buttonKelas != null)
            buttonKelas.SetActive(false);

        if (buttonGame != null)
            buttonGame.SetActive(false);
    }

    public void BackToMainMenu()
    {
        if (kelasPanel != null)
            kelasPanel.SetActive(false);

        if (buttonKelas != null)
            buttonKelas.SetActive(true);

        if (buttonGame != null)
            buttonGame.SetActive(true);
    }

    public void OpenGame()
    {
        Debug.Log("Game Unit 1 dibuka");
    }

    public void OpenVideo()
    {
        Debug.Log("Video Unit 1 dibuka");
    }

    public void OpenPowerPoint()
    {
        Debug.Log("PowerPoint Unit 1 dibuka");
    }

    public void OpenInfographics()
    {
        Debug.Log("Infographics Unit 1 dibuka");
    }

    public void OpenQuiz()
    {
        Debug.Log("Quiz Unit 1 dibuka");
    }
}