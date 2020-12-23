using UnityEngine;
using UnityEngine.UI;

public class SceneSelect : MonoBehaviour
{
    public static string levelID="Level1";
    Button homeButton, resetButton;
    GameObject gameMenu, levelMenu, message;


    void Start()
    {
        homeButton = GameObject.Find("HomeButton").GetComponent<Button>();
        resetButton = GameObject.Find("ResetButton").GetComponent<Button>();

        gameMenu = GameObject.Find("Game");
        levelMenu = GameObject.Find("LevelMenu");

        message = GameObject.Find("Message");
        homeButton.onClick.AddListener(ShowHomeMenu);
        resetButton.onClick.AddListener(ResetLevel);

        gameMenu.SetActive(false);
        message.SetActive(false);
    }

    private void ResetLevel()
    {
        message.SetActive(false);
        gameMenu.GetComponent<BoardSetup>().InitialiseBoard();
    }

    private void ShowHomeMenu()
    {
        if(gameMenu.activeInHierarchy)
        {
            message.SetActive(false);
            gameMenu.SetActive(false);
            levelMenu.SetActive(true);
        }
    }


    public void ShowMessage()
    {
        if (!message.activeInHierarchy)
        {
            message.SetActive(true);
        }
    }


    public void OnClicked(Button btn)
    {
        //Debug.LogWarning(btn.gameObject.name);
        levelID = btn.gameObject.name;

        levelMenu.SetActive(false);
        gameMenu.SetActive(true);

        gameMenu.GetComponent<BoardSetup>().InitialiseBoard();
    }
}
