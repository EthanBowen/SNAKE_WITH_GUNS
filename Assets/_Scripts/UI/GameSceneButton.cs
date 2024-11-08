using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneButton : MonoBehaviour
{
    public float FlickerTime = 0.5f;
    public float FlickerTimer = 0.0f;

    TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            GoToGameScene();
        }

        FlickerTimer += Time.deltaTime;

        if(FlickerTimer > FlickerTime)
        {
            if(text)
            {
                //text.enabled = !text.enabled;
            }

            FlickerTimer = 0;
        }
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
