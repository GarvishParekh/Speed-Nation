using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class StoryUiController : MonoBehaviour
{
    BgMusicManager bgMusicManager;
    [SerializeField] private List<GameObject> messages = new List<GameObject>();
    [SerializeField] private GameObject phoneHolder;
    [SerializeField] private GameObject sendButton;

    bool firstMessageSent = false;
    bool secondMessageSent = false;

    private void Start()
    {
        bgMusicManager = BgMusicManager.instance;
        // starter finishup
        foreach (var message in messages)
        {
            message.transform.localScale = Vector3.zero;
        }
        sendButton.SetActive(false);

        phoneHolder.transform.localScale = Vector3.zero;
        LeanTween.scale(phoneHolder, Vector3.one, 0.25f).setEaseInOutSine().setOnComplete(() =>
        {
            StartCoroutine(nameof(StartConversation));
        });
    }



    private IEnumerator StartConversation()
    {
        
        LeanTween.scale(messages[0], Vector3.one, 0.5f).setEaseInOutElastic().setDelay(1).setOnComplete(()=>
        {
            sendButton.SetActive(true);
        });

        while(!firstMessageSent)
        {
            yield return null;
        }

        sendButton.SetActive(false);
        LeanTween.scale(messages[1], Vector3.one, 0.5f).setEaseInOutElastic();

        LeanTween.scale(messages[2], Vector3.one, 0.5f).setEaseInOutElastic().setDelay(1).setOnComplete(() =>
        {
            sendButton.SetActive(true);
        });

        while (!secondMessageSent)
        {
            yield return null;
        }
        sendButton.SetActive(false);
        LeanTween.scale(messages[3], Vector3.one, 0.5f).setEaseInOutElastic().setOnComplete(()=>
        {
            bgMusicManager.StartMusic();
        });
        LeanTween.moveY(phoneHolder, -2000f, 0.5f).setEaseInOutSine().setDelay(1.5f).setOnComplete(()=>
        {
            SceneManager.LoadScene(ConstantKeys.SCENE_GAMEPLAY);
        });
    }

    public void _SendButton()
    {
        if (!firstMessageSent)
        {
            firstMessageSent = true;
            return;
        }
        secondMessageSent = true;
    }
}
