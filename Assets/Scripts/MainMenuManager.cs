using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] string newGameScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NewGameButton(){
        SceneManager.LoadScene(newGameScene);
    }

    public void LoadGameButton(){
    }

    public void ExitButton(){

    }
}
