using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class navigation : MonoBehaviour
{
    public void LoadSceneBernouli()
    {
        SceneManager.LoadScene("bernouli"); // Replace with your scene name
    }
    public void LoadSceneBernouliAR()
    {
        SceneManager.LoadScene("bernouliAr"); // Replace with your scene name
    }
    public void LoadSceneToricelli()
    {
        SceneManager.LoadScene("SampleScene"); // Replace with your scene name
    }
    public void LoadSceneToricelliAR()
    {
        SceneManager.LoadScene("ToricelliAr"); // Replace with your scene name
    }
    public void LoadSceneMain()
    {
        SceneManager.LoadScene("Mainmenu"); // Replace with your scene name
    }

}
