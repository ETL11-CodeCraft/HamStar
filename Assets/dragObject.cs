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
        // ARGestureInteractor�� ã��
        _gestureInteractor = FindObjectOfType<ARGestureInteractor>();
    }

    private void OnEnable()
    {
        if (_gestureInteractor != null)
        {
            // Drag Gesture ���� �̺�Ʈ�� ���� ������ �߰�
            _gestureInteractor.dragGestureRecognizer.onGestureStarted += OnDragStarted;
        }
    }

    private void OnDisable()
    {
        if (_gestureInteractor != null)
        {
            // �̺�Ʈ ���� ����
            _gestureInteractor.dragGestureRecognizer.onGestureStarted -= OnDragStarted;
        }
    }

    private void OnDragStarted(DragGesture gesture)
    {
        // �巡�װ� ���۵� �� ����Ǵ� �Լ�
        Ray ray = Camera.main.ScreenPointToRay(gesture.startPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("�巡�� ����");
            // ��ġ�� ������Ʈ�� �巡�� ������ ��쿡�� ó��
            if (hit.transform == this.transform)
            {
                _isDragging = true;
                _draggedObject = hit.transform;
                Debug.LogWarning($"[Object Start Postion] {_draggedObject.position}");

                // �巡�� �� ������Ʈ�� ���ῡ ���� ������ �߰�
                gesture.onUpdated += OnDragUpdated;
                gesture.onFinished += OnDragFinished;
            }
        }
    }

    private void OnDragUpdated(DragGesture gesture)
    {
        //if (_isDragging && _draggedObject != null)
        //{
        //    Debug.Log("�巡�� ��");
        //    // �巡�� ���� �� ������Ʈ�� ��ġ�� ������Ʈ
        //    Ray ray = Camera.main.ScreenPointToRay(gesture.position);
        //    var hits = new List<ARRaycastHit>();
        //    var raycastManager = FindObjectOfType<ARRaycastManager>();

        //    if (raycastManager != null && raycastManager.Raycast(ray.origin, hits, TrackableType.Planes))
        //    {
        //        Pose hitPose = hits[0].pose;
        //        _draggedObject.position = hitPose.position; // ���ο� ��ġ�� ������Ʈ �̵�
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
        // �巡�� ���� �� ����Ǵ� �Լ�
        _isDragging = false;
        _draggedObject = null;
    }
}
