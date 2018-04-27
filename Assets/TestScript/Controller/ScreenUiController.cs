using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class RaisingUI
{
    public Text[] screenTexts;
    public float liftTime = 1;
    public float raiseSpeed = 10;
    [System.NonSerialized]
    public int index = 0;
}

public class ScreenUiController : MonoBehaviour {

    public RectTransform myR;
    public Transform healthBarParent;
    public Image interactImg;

    public PlayerUiController playerUiController;

    public GameObject inputHelper;
    public RaisingUI raiseUI;

    private void Start()
    {
        inputHelper.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            ShowStringOnScreen(" test ");
    }

    public void ShowStringOnScreen(string content)
    {
        Text temp = raiseUI.screenTexts[raiseUI.index];    
        temp.text = content;
        StartCoroutine(TextMoveOn(temp));
        raiseUI.index++;
        if (raiseUI.index > raiseUI.screenTexts.Length - 1)
            raiseUI.index = 0;
    }

    IEnumerator TextMoveOn(Text text)
    {
        Vector3 originPos = text.transform.parent.localPosition;

        text.transform.parent.gameObject.SetActive(true);

        float timer = 0;
        while(timer < raiseUI.liftTime)
        {
            timer += Time.deltaTime;
            text.transform.parent.localPosition += new Vector3(0, 1, 0) * timer * raiseUI.raiseSpeed;
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Clamp01(raiseUI.liftTime - timer / raiseUI.liftTime + 0.3f));
            yield return null;
        }

        text.transform.parent.gameObject.SetActive(false);

        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        text.transform.parent.localPosition = originPos;
    }

    public void DisplayInteractUI(Transform transform)
    {
        interactImg.gameObject.SetActive(true);
        interactImg.transform.localPosition = WorldToUI(myR, transform.position + (new Vector3(1, 1.5f, 0)));
    }

    public void DisableInteractUI()
    {
        interactImg.gameObject.SetActive(false);
    }

    public void UpdatingNpcUiPos(Transform updaingUI,Transform target)
    {
        updaingUI.localPosition = WorldToUI(myR, target.position + (new Vector3(0, 2.5f, 0)));

        if (updaingUI.localPosition.x < -Screen.width / 2)
            updaingUI.localPosition = new Vector3(-Screen.width / 2, updaingUI.localPosition.y, updaingUI.localPosition.z);
        if (updaingUI.localPosition.x > Screen.width / 2)
            updaingUI.localPosition = new Vector3(Screen.width / 2, updaingUI.localPosition.y, updaingUI.localPosition.z);
        if (Camera.main.WorldToScreenPoint(target.position).z < 0)
            updaingUI.localPosition = new Vector3(updaingUI.localPosition.x, -Screen.height / 2, updaingUI.localPosition.z);
    }

    static public Vector2 WorldToUI(RectTransform r, Vector3 pos)
    {
        Vector2 screenPos = Camera.main.WorldToViewportPoint(pos); //世界物件在螢幕上的座標，螢幕左下角為(0,0)，右上角為(1,1)
        Vector2 viewPos = (screenPos - r.pivot) * 2; //世界物件在螢幕上轉換為UI的座標，UI的Pivot point預設是(0.5, 0.5)，這邊把座標原點置中，並讓一個單位從0.5改為1
        float width = r.rect.width / 2; //UI一半的寬，因為原點在中心
        float height = r.rect.height / 2; //UI一半的高
        return new Vector2(viewPos.x * width, viewPos.y * height); //回傳UI座標
    }

    static public Vector3 UIToWorld(RectTransform r, Vector3 uiPos)
    {
        float width = r.rect.width / 2; //UI一半的寬
        float height = r.rect.height / 2; //UI一半的高
        Vector3 screenPos = new Vector3(((uiPos.x / width) + 1f) / 2, ((uiPos.y / height) + 1f) / 2, uiPos.z); //須小心Z座標的位置
        return Camera.main.ViewportToWorldPoint(screenPos);
    }
}
