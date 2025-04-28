using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ButtonFeedback : MonoBehaviour
{
  Image buttonImg;
  public Color clickColor = Color.red;
    void Start()
    {
        buttonImg = GetComponent<Image>();
    }

    // Update is called once per frame
    public void ClickAction()
    {
        StartCoroutine(ClickFeedback());
    }

    IEnumerator ClickFeedback()
    {
        buttonImg.color = clickColor;
        yield return null;
        yield return null;
        buttonImg.color = Color.white;
    }
}
