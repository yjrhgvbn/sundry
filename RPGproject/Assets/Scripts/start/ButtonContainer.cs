using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonContainer : MonoBehaviour {
    
    //保存用PlayerPrefs

    //开始新游戏
    public void OnNewGame()
    {
        PlayerPrefs.SetInt("DataFromSave", 0);
        //加载选择角色的场景
        SceneManager.LoadScene(1);
    }

    //价值已经保存的游戏
    public void OnLoadGame()
    {
        PlayerPrefs.SetInt("DataFromSave", 1);//表示游戏是否保存过
        //加载玩家保存的场景

    }
}
