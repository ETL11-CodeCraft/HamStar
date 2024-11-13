using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

struct SouvenirStruct
{
    public SouvenirStruct(string souvenirName, string souvenirDescription, int souvenirID)
    {
        name = souvenirName;
        desc = souvenirDescription;
        id = souvenirID;
    }

    public string name;
    public string desc;
    public int id;
}

public class SouvenirManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> _souvenirSprites = new List<Sprite>(8);
    private List<SouvenirStruct> _souvenirList = new List<SouvenirStruct>(8)     //이후 데이터를 읽는 방식으로 변경
    {
        //임시 데이터
        new SouvenirStruct("statue_of_liberty","statue_of_liberty",0),
        new SouvenirStruct("eiffel_tower", "eiffel_tower", 1),
        new SouvenirStruct("pyramid", "pyramid", 2),
        new SouvenirStruct("statue_of_liberty","statue_of_liberty",0),
        new SouvenirStruct("eiffel_tower", "eiffel_tower", 1),
        new SouvenirStruct("pyramid", "pyramid", 2),
        new SouvenirStruct("statue_of_liberty","statue_of_liberty",0),
        new SouvenirStruct("eiffel_tower", "eiffel_tower", 1),
        new SouvenirStruct("pyramid", "pyramid", 2),
    };
    private Vector2 _origin = new Vector2(-140f, 500f);
    private float _screenWidth;
    private const float DELTA_X = 280f;
    private const float DELTA_Y = -230f;
    private const int ROW = 4;
    private const int COL = 2;
    [SerializeField] private Souvenir _souvenirPrefab;
    private Dictionary<int,Souvenir> _souvenirItems = new Dictionary<int,Souvenir>(8);
    private List<int> _uncollectedItems = new List<int>(8);
    [SerializeField] SouvenirInfo _souvenirInfo;
    private int _pageIdx = 0;
    private float _panelOrigin;

    private void Start()
    {
        SaveManager.LoadSouvenirData();

        _screenWidth = Screen.width;
        _panelOrigin = transform.position.x;

        for(int i=0;i<_souvenirList.Count;i++)
        {
            var obj = Instantiate(_souvenirPrefab);
            var curItem = _souvenirList[i];

            //기본 Info 설정
            Sprite objSprite = _souvenirSprites[Mathf.Clamp(curItem.id, 0, _souvenirSprites.Count - 1)];
            obj.SetSouvenir(curItem.name, objSprite, curItem.desc, curItem.id);

            //위치 설정
            obj.transform.SetParent(transform);
            obj.transform.localPosition = _origin + new Vector2(((i % COL == 0) ? 0 : DELTA_X) + _screenWidth * (i / (ROW*COL)), DELTA_Y * ((i / COL) % ROW));

            //OnClickAction 설정
            obj.onClickAction += _souvenirInfo.ActiveInfo;

            //추후 획득한 기념품을 쉽게 찾을 수 있도록 Dictionary 사용
            if(!_souvenirItems.ContainsKey(curItem.id))
            {
                _souvenirItems.Add(curItem.id, obj);
                if(!SaveManager.souvenirData.collectedSouvenir.Contains(curItem.id))
                {
                    _uncollectedItems.Add(curItem.id);
                }
                else
                {
                    obj.IsCollected = true;
                }
            }
        }
    }

    
    public void MoveNextPage()
    {
        if (_pageIdx >= _souvenirList.Count / (COL * ROW)) return;

        _pageIdx++;
        transform.DOMoveX(_panelOrigin - _pageIdx * _screenWidth, 0.5f);
    }
    public void MovePrevPage()
    {
        if (_pageIdx <= 0) return;

        _pageIdx--;
        transform.DOMoveX(_panelOrigin - _pageIdx * _screenWidth, 0.5f);
    }

    public void CollectSouvenir()
    {
        if(_uncollectedItems.Count <= 0)
        {
            //이미 모든 기념품을 획득하였습니다.
            return;
        }
        var ItemId = _uncollectedItems[Random.Range(0,_uncollectedItems.Count)];

        _souvenirItems[ItemId].IsCollected = true;
        _uncollectedItems.Remove(ItemId);

        SaveManager.AddCollectedSouvenir(ItemId);

        if (_uncollectedItems.Count <= 0)
        {
            //축하합니다. 모든 기념품을 획득하였습니다.
        }
    }
}
