using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeaponUI : MonoBehaviour {

    public static ShopWeaponUI _instance;

    public int[] weaponIdArrary;
    public UIGrid grid;
    public GameObject weaponItem;
    private TweenPosition tween;
    private bool isShow = false;
    private GameObject numberDialog;
    private UIInput numberInput;
    private int buyId = 0;

    private void Awake()
    {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
        numberDialog = transform.Find("Panel/NumberDialog").gameObject;
        numberInput = transform.Find("Panel/NumberDialog/NumberInput").GetComponent<UIInput>();
        numberDialog.SetActive(false);
    }

    private void Start()
    {       
        InitShopWeapon();
    }

    public void TransformState()
    {
        if(isShow)
        {
            tween.PlayReverse();
            isShow = false;
        }
        else
        {
            tween.PlayForward();
            isShow = true;
        }
    }

    void InitShopWeapon()//初始化武器商店的信息
    {
        foreach(int id in weaponIdArrary)
        {
            GameObject go = NGUITools.AddChild(grid.gameObject, weaponItem);
            grid.AddChild(go.transform);
            go.GetComponent<ShopWeaponItem>().SetId(id);
        }
    }

    public void OnOkButtonClick()
    {
        int count = int.Parse(numberInput.value);
        if (count <= 0)
        {
            numberInput.value = "0";
            buyId = 0;
            numberDialog.SetActive(true);
            return;
        }
        int price = ObjectsInfo._instance.GetObjectByID(buyId).price_buy;
        int total_price = price * count;
        if (inventory._instance.GetCoin(total_price))
        {
            inventory._instance.GetID(buyId, count);
        }

        numberInput.value = "0";
        buyId = 0;
        numberDialog.SetActive(true);

    }

    public void OnBuyClick(int id)
    {
        numberDialog.SetActive(true);
        numberInput.value = "0";
        buyId = id;

    }

    public void OnClick()
    {
        numberDialog.SetActive(false);
    }
}
