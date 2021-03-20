using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{

    public GameObject inventory;
    public GameObject quitconfrim;
    public GameObject loadGame;

    Move2D move2D;
    Collectables collectables;

    public void QuitMenu()
    {
        move2D = gameObject.GetComponent<Move2D>();
        collectables = gameObject.GetComponent<Collectables>();
        SaveSystem.SavePlayer(move2D, collectables);

        SceneManager.LoadScene(0);
        inventory.SetActive(false);
        quitconfrim.SetActive(false);
    }
    public void NoQuitMenu()
    {
        quitconfrim.SetActive(false);
    }

    void Start()
    {
        inventory.SetActive(false);
        quitconfrim.SetActive(false);
        loadGame.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            inventory.SetActive(false);
        }
        if (Input.GetKeyDown("escape"))
        {
            quitconfrim.SetActive(true);
        }
    }
}
