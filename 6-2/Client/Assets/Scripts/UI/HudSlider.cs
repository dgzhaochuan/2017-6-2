using UnityEngine;
using UnityEngine.UI;

public class HudSlider : MonoBehaviour
{

    const float speed = 5;
    const float minValue = .01f;
    Image transition;
    Image hpimage;
    bool play;
    bool add;
    public  void Init()
    {
        transition = transform.Find("transition").GetComponent<Image>();
        hpimage = transform.Find("value").GetComponent<Image>();
    }

    private void OnEnable()
    {
        Manage.Instance.AddUpdate(update);
    }
    private void OnDisable()
    {
        Manage.Instance.RemoveUpdate(update);
    }

    void update()
    {
        if (!play) return;
        if (!add)
        {
            transition.fillAmount = Mathf.Lerp(transition.fillAmount, hpimage.fillAmount, speed * Time.deltaTime);
            if (transition.fillAmount - hpimage.fillAmount <= minValue)
            {
                play = false;
                transition.fillAmount = hpimage.fillAmount;
            }
        }
        else
        {
            hpimage.fillAmount = Mathf.Lerp(hpimage.fillAmount, transition.fillAmount, speed * Time.deltaTime);
            if (transition.fillAmount - hpimage.fillAmount <= minValue)
            {
                play = false;
                hpimage.fillAmount = transition.fillAmount;
            }
        }
    }
    public void SetValue(float value)
    {
        value = Mathf.Clamp01(value);
        play = true;
        add = hpimage.fillAmount < value;
        if (!add)
        {
            hpimage.fillAmount = value;
        }
        else
        {
            transition.fillAmount = value;
        }
    }
    public void ToValue(float value)
    {
        transition.fillAmount = hpimage.fillAmount = value;
    }
}
