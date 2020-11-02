using UnityEngine.SceneManagement;
using UnityEngine;


public class SceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadAILevel()
    {
        SceneManager.LoadScene("AIMode");
    }
    public void LoadPlayerLevel()
    {
        SceneManager.LoadScene("PlayerMode");
    }
}
