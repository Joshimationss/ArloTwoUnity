using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator ani;

    public float transTime = 1f;

    public IEnumerator LoadLevel(int levelIndex)
    {
        //play anim
        ani.SetTrigger("FadeOut");
        //wait
        yield return new WaitForSeconds(transTime);
        //load
        SceneManager.LoadScene(levelIndex);
    }
}
