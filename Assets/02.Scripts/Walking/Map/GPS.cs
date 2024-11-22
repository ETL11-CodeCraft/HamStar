using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Android;

public class GPS : MonoBehaviour, IGPS
{
    public float latitude => _latitude;

    public float longitude => _longitude;

    public float latitudeMap => _latitudeMap;

    public float longitudeMap => _longitudeMap;

    private float _maxWaitTime = 10.0f;   //GPS 타임아웃 에러 타이머
    private float _resendTime = 1.0f; //GPS 갱신주기
    private float _latitude = 0;
    private float _longitude = 0;
    private float _latitudeMap = 0;
    private float _longitudeMap = 0;
    private float _waitTime = 0;
    private bool _receiveGPS = false;

    public event Action<float, float> onLocationChanged;
    public event Action<float, float, float, float> onMapMoved;

    void Start()
    {
        StartCoroutine(C_RefreshGPSData());
    }

    /// <summary>
    /// GPS 정보 갱신
    /// </summary>
    /// <returns></returns>
    private IEnumerator C_RefreshGPSData()
    {
        //만일,GPS사용 허가를 받지 못했다면, 권한 허가 팝업을 띄움
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        //만일 GPS 장치가 켜져 있지 않으면 위치 정보를 수신할 수 없다고 표시
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }

        //위치 데이터를 요청 -> 수신 대기
        Input.location.Start();

        //GPS 수신 상태가 초기 상태에서 일정 시간 동안 대기함
        while (Input.location.status == LocationServiceStatus.Initializing && _waitTime < _maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            _waitTime++;
        }

        //수신 실패 시 수신이 실패됐다는 것을 출력
        if (Input.location.status == LocationServiceStatus.Failed)
        {
        }

        //응답 대기 시간을 넘어가도록 수신이 없었다면 시간 초과됐음을 출력
        if (_waitTime >= _maxWaitTime)
        {
        }

        //수신된 GPS 데이터를 화면에 출력/

        LocationInfo li = Input.location.lastData;

        //위치 정보 수신 시작 체크
        _receiveGPS = true;

        //위치 데이터 수신 시작 이후 resendTime 경과하고 위도나 경도가 200m 이상 멀어지면 새로운 맵 생성. 아니라면 위치에 따른 맵 이미지 이동
        while (_receiveGPS)
        {
            li = Input.location.lastData;

            _latitude = li.latitude;
            _longitude = li.longitude;

            //맵을 불러온 위치에서 위도나 경도가 200m 이상 멀어지면 맵을 새로 불러온다.
            if (DistanceCalculator.GetDistance(_latitudeMap, _longitudeMap, li.latitude, _longitudeMap) > 200 || DistanceCalculator.GetDistance(_latitudeMap, _longitudeMap, _latitudeMap, li.longitude) > 200)
            {
                _latitudeMap = li.latitude;
                _longitudeMap = li.longitude;
                onLocationChanged?.Invoke(_latitude, _longitude);
            }
            
            onMapMoved?.Invoke(_latitudeMap, _longitudeMap, _latitude, _longitude);
            yield return new WaitForSeconds(_resendTime);
        }
    }
}
