using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIManager : MonoBehaviour
{
    public RectTransform Menu;
    

    public void OnStoreBtn()
    {
        Menu.DOAnchorPos(new Vector2(1090, 0), 0.25f);
    }

    public void OnEquipBtn()
    {
        Menu.DOAnchorPos(new Vector2(-1090, 0), 0.25f);
    }

    public void OnSmithyBtn()
    {
        Menu.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }
}
