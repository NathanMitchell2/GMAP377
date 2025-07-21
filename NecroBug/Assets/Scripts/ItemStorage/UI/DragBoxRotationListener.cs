using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragBoxRotationListener : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [SerializeField] private float dragSpeed = 1f;
    private BuilderUI builderUI;
    private Vector2 initialPos = Vector2.zero;

    public void OnBeginDrag(PointerEventData eventData)
    {
        initialPos = Mouse.current.position.value;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (builderUI != null)
        {
            Debug.LogError(eventData.pointerClick.gameObject);
            Vector2 curPos = Mouse.current.position.value;

            float xChange = initialPos.x - curPos.x;
            float yChange = 0;// initialPos.y - curPos.y;

            builderUI.RotateGrid(new Vector3(yChange*dragSpeed, xChange * dragSpeed, 0));

            initialPos = curPos;

        }
    }

    private void Awake()
    {
        builderUI = GetComponentInParent<BuilderUI>();
    }
    /*
    private void OnMouseDown()
    {
        initialPos = Mouse.current.position.value;
    }
    private void OnMouseDrag()
    {
        Debug.LogError("MouseDrag");
        if (builderUI != null)
        {
            Debug.LogError("MouseDragIn");
            Vector2 curPos = Mouse.current.position.value;

            float xChange = initialPos.x - curPos.x;
            float yChange = initialPos.y - curPos.y;

            builderUI.RotateGrid(new Vector3(xChange, yChange, 0));

            initialPos = curPos;

        }
    }
    */
    
}
