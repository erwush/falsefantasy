using UnityEngine;
using UnityEngine.UI;
public class BarController : MonoBehaviour
{
    [SerializeField] private string barName;
    [SerializeField]private Camera cam;
    [SerializeField]private float value;
    [SerializeField]private Slider slider;
    [SerializeField]private Vector3 offset;
    [SerializeField] private Image fillImage;
    [SerializeField]private RectTransform fillArea;
    [SerializeField] private GameObject trackingTarget;
    [SerializeField] private Transform target;

    public void UpdateBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
        if (slider.value > 0.5f)
        {
            fillImage.color = Color.green;
        }
        else if (slider.value <= 0.5f && slider.value > 0.15f)
        {
            fillImage.color = Color.yellow;
        }
        else
        {
            fillImage.color = Color.red;
        }
    }

    void Update()
    {
        // slider.transform.localScale = new Vector3 (0.02, 0.02, ;
    }





    // Start is called once before the first execution of Update after the MonoBehaviour is create
}
