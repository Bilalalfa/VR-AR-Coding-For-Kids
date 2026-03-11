using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadClassroom()
    {
        SceneManager.LoadScene("Classroom");
    }

    public void LoadFirstMenu()
    {
        SceneManager.LoadScene("FirstMenu");
    }
}