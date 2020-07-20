using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Symbol : MonoBehaviour
{
    [SerializeField]
    private ColorTypeSymbol[] colorTypeSymbol;
    [SerializeField]
    private Image backGroundImage;

    public SymbolType type = 0;
    public int x;
    public int y;
    public SymbolColor color = SymbolColor.None;

    private Vector2 originPos = Vector2.zero;
    private Vector2 updatePos = Vector2.zero;

    public int[] posArr;

    //public GridItem left;
    //public GridItem right;
    //public GridItem top;
    //public GridItem bottom;

    private void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();

        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
        trigger.triggers.Add(beginDragEntry);

        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.Drag;
        dragEntry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(dragEntry);

        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        trigger.triggers.Add(endDragEntry);
    }

    private void InitSymbol()
    {
        originPos = this.transform.localPosition;
    }

    public void SetSymbol(int _x, int _y, SymbolColor _color, SymbolType _type)
    {
        x = _x;
        y = _y;
        color = _color;
        type = _type;

        HideSymbol();

        if (type != SymbolType.Blank)
        {
            SetImage();
        }
    }

    private void SetImage()
    {
        int index = (int)type - 1;

        colorTypeSymbol[index].ShowSymbol(color);
        backGroundImage.color = GameManager.Instance.symbolManager.GetSymbolColor(color);
    }

    public void HideSymbol()
    {
        for(int i = 0; i < colorTypeSymbol.Length; i++)
        {
            colorTypeSymbol[i].HideSymbol();
        }
    }

    public void PlayGradeAnimation()
    {

    }

    public void PlayCombineAnimation()
    {

    }

    private void OnBeginDrag(PointerEventData eventData)
    {
        originPos = this.transform.position;
    }

    private void OnDrag(PointerEventData eventData)
    {
        //Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        //Vector3 rayPoint = ray.GetPoint(Vector3.Distance(transform.position, Camera.main.transform.position));
        transform.position = eventData.position;
    }

    private void OnEndDrag(PointerEventData eventData)
    {
        if (CheckCellIn(transform.position) == false)
        {
            transform.position = originPos;
        }
        else
        {
            getNearestGrid(transform.position);
        }
    }

    private bool CheckCellIn(Vector2 pos)
    {
        if(pos.y < 200 || pos.y > 750)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private Vector2 getNearestGrid(Vector2 pos)
    {
        int cell_x = (int)(pos.x / 110);
        int cell_y = (int)(pos.y / 110);

        return pos;
    }
}
