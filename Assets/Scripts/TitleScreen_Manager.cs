using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

public class TitleScreen_Manager : MonoBehaviour
{

    public void ChangeGameScene()
    {
        SceneManager.LoadScene("MainShop");
    }
}
