using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCtrl : MonoBehaviour
{
    public GameObject gameOverMessage;
    Animator animator;
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator.GetInteger("HP") < 0)
        {
            gameOverMessage.SetActive(true);
            StartCoroutine(RestartGame(3f));
        }
    }
    IEnumerator RestartGame(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(sceneName);
    }
}
