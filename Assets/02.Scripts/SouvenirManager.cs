using System.Collections.Generic;
using UnityEngine;

struct SouvenirData
{
    public SouvenirData(string souvenirName, string souvenirDescription, int souvenirID)
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
    [SerializeField] private List<Sprite> souvenirSprites = new List<Sprite>(8);
    private List<SouvenirData> souvenirList = new List<SouvenirData>(8)     //이후 데이터를 읽는 방식으로 변경
    {
        //임시 데이터
        new SouvenirData("statue_of_liberty","statue_of_liberty",0),
        new SouvenirData("eiffel_tower", "eiffel_tower", 1),
        new SouvenirData("pyramid", "pyramid", 2),
        new SouvenirData("statue_of_liberty","statue_of_liberty",0),
        new SouvenirData("eiffel_tower", "eiffel_tower", 1),
        new SouvenirData("pyramid", "pyramid", 2),
        new SouvenirData("statue_of_liberty","statue_of_liberty",0),
        new SouvenirData("eiffel_tower", "eiffel_tower", 1),
        new SouvenirData("pyramid", "pyramid", 2),
    };
    private Vector2 origin = new Vector2(-140f, 500f);
    private float screenWidth;
    private const float DELTA_X = 280f;
    private const float DELTA_Y = -230f;
    private const int ROW = 4;
    private const int COL = 2;
    [SerializeField] private Souvenir souvenirPrefab;
    private Dictionary<int,Souvenir> souvenirItems = new Dictionary<int,Souvenir>(8);
    [SerializeField] SouvenirInfo souvenirInfo;

    private void Start()
    {
        screenWidth = Screen.width;

        for(int i=0;i<souvenirList.Count;i++)
        {
            var obj = Instantiate(souvenirPrefab);

            //기본 Info 설정
            Sprite objSprite = souvenirSprites[Mathf.Clamp(souvenirList[i].id, 0, souvenirSprites.Count - 1)];
            obj.SetSouvenir(souvenirList[i].name, objSprite, souvenirList[i].desc, souvenirList[i].id);

            //위치 설정
            obj.transform.SetParent(transform);
            obj.transform.localPosition = origin + new Vector2(((i % COL == 0) ? 0 : DELTA_X) + screenWidth * (i / (ROW*COL)), DELTA_Y * ((i / COL) % ROW));

            //OnClickAction 설정
            obj.onClickAction += souvenirInfo.ActiveInfo;
        }
    }
}
