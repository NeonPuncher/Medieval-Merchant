using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Management : MonoBehaviour
{
    public GameObject MAINSHOP;

    public int money;
    private float moneyPrev;
    public TextMeshProUGUI moneyText;
    public GameObject inventoryWeapon;
    public float time;
    public UnityEvent interactEvent;

    //Variables for creating Task in InfoBook
    private string itemTask;
    private string nameTask;
    public GameObject prefabTask;
    public Transform infoParent;

    //Map
    public Map_Generator map;
    public GameObject cartObj;
    public TextMeshProUGUI timeText;

    void Start()
    {
        PlayerPrefs.SetFloat("money", money);
        moneyText.text = money.ToString();
    }

    private void Update()
    {
        if (map.sendWorker == true)
        {
            if (map.workTimer > 0)
            {
                map.workTimer -= Time.deltaTime;
                timeText.text = "Time left: " + Mathf.Round(map.workTimer).ToString() + " sec";
            }
            else
            {
                cartObj.SetActive(true);
                map.sendWorker = false;
            }
        }
        else
        {
            timeText.text = "";
        }

        //JEZUS FUCKING CHRIST REDO THIS CODE????
        //Redraw Money text if amount changes
        if(PlayerPrefs.GetFloat("money") != moneyPrev)
        {
            moneyText.text = PlayerPrefs.GetFloat("money").ToString();
            moneyPrev = PlayerPrefs.GetFloat("money");
        }
    }

    public void ChangeRecipeScene()
    {
        interactEvent.Invoke();
        MAINSHOP.SetActive(false);
        SceneManager.LoadScene("CraftScene", LoadSceneMode.Additive);
    }

    public void ChangeHaggleScene()
    {
        interactEvent.Invoke();
        PlayerPrefs.SetFloat("money", money);
        MAINSHOP.SetActive(false);
        SceneManager.LoadScene("HaggleScene", LoadSceneMode.Additive);
    }
}
