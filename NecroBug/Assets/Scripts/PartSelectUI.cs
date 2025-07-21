using UnityEngine;
using UnityEngine.UI;

public class PartSelectUI : MonoBehaviour
{
    [SerializeField] Sprite unselected;
    [SerializeField] Sprite selected;
    private Image img;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        img.sprite = selected;
    }

    public void DeSelect()
    {
        img.sprite = unselected;
    }
}
