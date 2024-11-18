using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SouvenirManager : MonoBehaviour
{
    [SerializeField] private List<SouvenirSpec> _souvenirList = new List<SouvenirSpec>(8);
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
    private DataLoader _dataLoader;
    private SouvenirData _souvenirData;

    private void Awake()
    {
        _dataLoader = new DataLoader();
        _souvenirData = _dataLoader.Load<SouvenirData>();
    }

    private void Start()
    {
        _screenWidth = Screen.width;
        _panelOrigin = transform.position.x;

        for(int i=0;i<_souvenirList.Count;i++)
        {
            var obj = Instantiate(_souvenirPrefab);
            var curItem = _souvenirList[i];

            //기본 Info 설정
            obj.SetSouvenir(curItem.SouvenirName, curItem.SouvenirSprite, curItem.SouvenirDescription, curItem.SouvenirID);

            //위치 설정
            obj.transform.SetParent(transform);
            obj.transform.localPosition = _origin + new Vector2(((i % COL == 0) ? 0 : DELTA_X) + _screenWidth * (i / (ROW*COL)), DELTA_Y * ((i / COL) % ROW));

            //OnClickAction 설정
            obj.onClickAction += _souvenirInfo.ActiveInfo;

            //추후 획득한 기념품을 쉽게 찾을 수 있도록 Dictionary 사용
            if(!_souvenirItems.ContainsKey(curItem.SouvenirID))
            {
                _souvenirItems.Add(curItem.SouvenirID, obj);
                if(!_souvenirData.collectedSouvenir.Contains(curItem.SouvenirID))
                {
                    _uncollectedItems.Add(curItem.SouvenirID);
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

        _souvenirData.collectedSouvenir.Add(ItemId);
        _dataLoader.Save(_souvenirData);

        if (_uncollectedItems.Count <= 0)
        {
            //축하합니다. 모든 기념품을 획득하였습니다.
        }
    }
}
