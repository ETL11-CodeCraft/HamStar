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
        new SouvenirStruct("자유의 여신상","오늘은 정말 멋진 광경을 보았어! 바로바로 자유의 여신상이야! 멀리서부터 커다랗고 우아하게 서 있는 모습을 보고 깜짝 놀랐어. 초록색 드레스에 거대한 책을 들고 있는데, 정말 멋지고 대단했어. 사람들은 여신상 앞에서 사진도 찍고 신나서 웃고 있더라고.\r\n\r\n배를 타고 가까이 다가갔을 때, 바람이 살랑살랑 불어서 기분도 좋았어. 여신상이 웃으며 날 환영해주는 것 같았달까? 언젠가 나도 저렇게 멋진 동물이 될 수 있으면 좋겠다는 생각이 들었어. 햄스터의 자유를 위해!\r\n\r\n오늘의 교훈: 여행은 나를 조금 더 크고 용감한 햄스터로 만들어준다는 것!",0),
        new SouvenirStruct("에펠탑", "오늘은 프랑스 파리에 있는 에펠탑을 보고 왔어! 정말 하늘까지 닿을 듯이 높고 섬세하게 만들어져서 눈을 뗄 수 없었어. 햇살을 받아 반짝이는 철골 구조가 멋지게 빛나고 있었지. 얼마나 높이 올라가고 싶은지 내 작은 발로는 상상도 할 수 없는 높이였어!\r\n\r\n에펠탑 근처에 있는 사람들이 다들 탑을 배경으로 사진을 찍고 있었는데, 나도 한 장 찍고 싶더라! 그래서 탑이 잘 보이는 잔디밭에서 작은 피크닉을 열고, 그림 같은 풍경을 마음껏 즐겼어.\r\n\r\n높은 곳에서 파리를 내려다보면 어떤 기분일까? 언젠가 용기를 내서 꼭대기까지 올라가 보고 싶어졌어. 파리의 탑에서 세상을 내려다보며 ‘이게 바로 햄스터의 삶이지!’라고 외칠 날을 기다리며!\r\n\r\n오늘의 교훈: 가끔은 높은 곳을 바라보는 것만으로도 꿈을 더 크게 가질 수 있다는 것!", 1),
        new SouvenirStruct("피라미드", "오늘은 꿈에 그리던 이집트의 피라미드를 보고 왔어! 사막 한가운데 우뚝 솟은 커다란 돌덩이들이 줄지어 서 있는데, 정말 신비롭고 멋졌어. 수천 년 전부터 저곳에서 조용히 세월을 견뎌왔다는 생각에 마음이 두근거렸지.\r\n\r\n햇살이 뜨겁게 내리쬐는 가운데, 피라미드를 한 바퀴 돌면서 마치 옛날 탐험가가 된 기분이었어. 꼭대기까지 올라가 볼 수는 없었지만, 거대한 돌들을 하나하나 살펴보며 햄스터의 호기심을 가득 채웠지. 작은 내 발로는 상상할 수 없는 모험이었달까?\r\n\r\n그리고 사막의 바람에 모래가 살랑살랑 불어올 때마다, 내 앞에도 새로운 모험의 길이 펼쳐지는 것 같았어. 이 피라미드가 누군가의 묘지라니 조금은 무서웠지만, 고대 이집트의 비밀을 알아내고 싶은 마음이 점점 커졌어.\r\n\r\n오늘의 교훈: 가끔은 알 수 없는 미지의 세계가 우리를 더 멀리, 더 용감하게 이끈다는 것!", 2),
        new SouvenirStruct("스톤헨지", "오늘은 영국의 신비로운 유적지, 스톤헨지에 다녀왔어! 멀리서부터 커다란 돌들이 원을 그리며 서 있는 모습을 보고 깜짝 놀랐어. 대체 어떻게 저렇게 큰 돌을 세웠는지 상상도 할 수 없더라구. 이 돌들이 도대체 왜 여기에 서 있는 걸까? 뭔가 중요한 비밀을 간직한 것 같아 괜히 두근거렸어.\r\n\r\n주변을 돌며 살펴봤는데, 이곳은 마치 옛날 햄스터들이 모여 회의라도 했을 것 같은 장소야. 햄스터 마을의 전설 같은 이야기가 떠올랐지! 아마도 이 돌들이 하늘과 땅, 그리고 별들을 이어주는 신성한 장소일지도 몰라. 밤이 되면 별빛을 타고 우주랑 연결된다는 느낌이랄까?\r\n\r\n스톤헨지의 돌들 사이를 거닐며, 나도 조금은 신비한 햄스터가 된 것 같은 기분이 들었어. 이곳에서 시간을 보내면서, 아주 오래전에도 햄스터들이 이렇게 모험을 즐기지 않았을까 상상해봤어.\r\n\r\n오늘의 교훈: 세상에는 우리가 다 알 수 없는 신비한 것들이 많고, 그걸 알아가려는 호기심이야말로 모험을 멈추지 않는 힘이라는 것!", 3),
        new SouvenirStruct("시드니 오페라하우스", "오늘은 시드니 오페라 하우스를 보러 갔어! 바다를 배경으로 우아하게 서 있는 모습이 정말 감동적이었어. 둥글게 겹쳐진 지붕들이 마치 커다란 조개껍질처럼 보였는데, 햄스터 입장에서는 거대한 해변의 성 같은 느낌이랄까? 그 속에서 어떤 멋진 공연들이 펼쳐질지 궁금해서 혼자 상상해 봤어.\r\n\r\n햇살이 반짝이는 바다와 오페라 하우스가 어우러진 풍경은 정말 환상적이었어. 특히 밤이 되자 불빛이 하나둘 켜지면서 건물이 빛나는 걸 보고 있자니, 여기가 현실인지 꿈속인지 헷갈릴 정도였지. 바다 냄새를 맡으며 산책하는 것도 아주 특별했어.\r\n\r\n나중에 나도 멋진 연주자가 되어 무대에 서고 싶다는 생각이 들었어. 혹시 햄스터 오케스트라가 있다면 오페라 하우스에서 꼭 연주를 해보고 싶어! 그럼 관객들도 나처럼 설레는 마음으로 이 건물과 공연을 즐길 수 있겠지?\r\n\r\n오늘의 교훈: 멋진 풍경은 그곳에 있는 것만으로도 마음에 음악을 울려주고, 나를 더 큰 꿈으로 이끈다는 것!", 4),
        new SouvenirStruct("피사의 사탑", "오늘은 피사의 사탑을 보러 갔어! 유명한 '기울어진 탑'이라는 말을 들었을 때는 그게 얼마나 기울었을까 궁금했는데, 정말 보기만 해도 쓰러질 것 같은 모습이라 깜짝 놀랐어. 어떻게 이렇게 오래동안 무너지지 않고 서 있는 걸까? 햄스터 입장에선 그야말로 균형잡기 고수인 탑이었지!\r\n\r\n사람들이 탑을 배경으로 손을 대는 포즈로 사진을 찍고 있어서 나도 따라 해봤어. 작고 짧은 앞발로 '탑을 잡고 있다'는 포즈를 해보려고 했지만, 조금 어려웠어. 그래도 탑 아래에서 주변을 둘러보니 햇살도 따뜻하고, 탑을 보며 웃는 사람들 덕에 기분이 좋아졌어.\r\n\r\n탑을 둘러보며 생각했어. 나도 저 탑처럼 흔들리지 않고 씩씩하게 여행을 계속할 수 있다면 얼마나 좋을까? 햄스터에게는 조금은 무모한 꿈일지도 모르지만, 여행의 순간순간이 나를 더 튼튼하게 만들어주는 것 같아.\r\n\r\n오늘의 교훈: 가끔은 비뚤어진 모습 그대로, 흔들림 속에서도 당당하게 서 있는 게 더 멋진 법이라는 것!", 5),
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
                if(!_souvenirData.collectedSouvenir.Contains(curItem.id))
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

        _souvenirData.collectedSouvenir.Add(ItemId);
        _dataLoader.Save(_souvenirData);

        if (_uncollectedItems.Count <= 0)
        {
            //축하합니다. 모든 기념품을 획득하였습니다.
        }
    }
}
