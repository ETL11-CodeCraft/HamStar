using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.Interaction.Toolkit.AR;

public class dragObject : MonoBehaviour
{
    private ARGestureInteractor _gestureInteractor;
    private bool _isDragging = false;
    [SerializeField] private Transform _draggedObject;

    private void Awake()
    {
        // ARGestureInteractor를 찾음
        _gestureInteractor = FindObjectOfType<ARGestureInteractor>();
    }

    private void OnEnable()
    {
        if (_gestureInteractor != null)
        {
            // Drag Gesture 시작 이벤트에 대한 리스너 추가
            _gestureInteractor.dragGestureRecognizer.onGestureStarted += OnDragStarted;
        }
    }

    private void OnDisable()
    {
        if (_gestureInteractor != null)
        {
            // 이벤트 구독 해제
            _gestureInteractor.dragGestureRecognizer.onGestureStarted -= OnDragStarted;
        }
    }

    private void OnDragStarted(DragGesture gesture)
    {
        // 드래그가 시작될 때 실행되는 함수
        Ray ray = Camera.main.ScreenPointToRay(gesture.startPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("드래그 시작");
            // 터치한 오브젝트가 드래그 가능한 경우에만 처리
            if (hit.transform == this.transform)
            {
                _isDragging = true;
                _draggedObject = hit.transform;
                Debug.LogWarning($"[Object Start Postion] {_draggedObject.position}");

                // 드래그 중 업데이트와 종료에 대한 리스너 추가
                gesture.onUpdated += OnDragUpdated;
                gesture.onFinished += OnDragFinished;
            }
        }
    }

    private void OnDragUpdated(DragGesture gesture)
    {
        //if (_isDragging && _draggedObject != null)
        //{
        //    Debug.Log("드래그 중");
        //    // 드래그 중일 때 오브젝트의 위치를 업데이트
        //    Ray ray = Camera.main.ScreenPointToRay(gesture.position);
        //    var hits = new List<ARRaycastHit>();
        //    var raycastManager = FindObjectOfType<ARRaycastManager>();

        //    if (raycastManager != null && raycastManager.Raycast(ray.origin, hits, TrackableType.Planes))
        //    {
        //        Pose hitPose = hits[0].pose;
        //        _draggedObject.position = hitPose.position; // 새로운 위치로 오브젝트 이동
        //    }
        //}

        if(_isDragging && _draggedObject != null)
        {
            Debug.LogWarning($"[Gesture Postion] {gesture.position}");
            _draggedObject.position = gesture.position;
        }
    }

    private void OnDragFinished(DragGesture gesture)
    {
        // 드래그 종료 시 실행되는 함수
        _isDragging = false;
        _draggedObject = null;
    }
}
