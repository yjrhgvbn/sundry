using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour {

    public GameObject[] characterPrefabs;
    private GameObject[] characterGameObjects;
    private int selectIndex = 0;
    private int length;
    public UIInput nameInput;//用来得到输入的文本

	// Use this for initialization
	void Start () {
        length = characterPrefabs.Length;
        characterGameObjects = new GameObject[length];
		for(int i = 0;i<length;i++)
        {
            characterGameObjects[i] = GameObject.Instantiate(characterPrefabs[i], transform.position, transform.rotation);
        }
	}
	
	// Update is called once per frame
	void Update () {
        UpdateCharacterShow();
	}

    void UpdateCharacterShow()//更新角色的显示
    {
        characterGameObjects[selectIndex].SetActive(true);
        for(int i=0;i<length;i++)
        {
            if(i!=selectIndex)
            {
                characterGameObjects[i].SetActive(false);
            }
        }
    }

    public void OnNextButtonClick()
    {
        selectIndex++;
        selectIndex %= length;
        UpdateCharacterShow();
    }

    public void OnPrevButtonClick()
    {
        selectIndex--;
        if (selectIndex <= -1)
            selectIndex = length - 1;
        UpdateCharacterShow();
    }

    public void OnOkButtonClick()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", selectIndex);//存储选择的角色
        PlayerPrefs.SetString("name", nameInput.value);//存储输入的名字
        SceneManager.LoadScene(2);
    }
}
