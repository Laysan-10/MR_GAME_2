using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlane))]
[RequireComponent(typeof(MeshRenderer))]
public class ARPlaneColorizer : MonoBehaviour
{
   
    // �������� ��� ���������� ������������� ���������
    public bool isVisualise
    {
        get => _isVisualise;
        set
        {
            _isVisualise = value;
            UpdateColor(); // ��������� ���� ��� ��������� ��������
        }
    }

    private const float DEFAULT_COLOR_ALPHA = 0.25f; // ������������ ����� �� ���������
    private bool _isVisualise; // ���������� ���� ��� �������� isVisualise
    private Color _defaultColor; // ���� �� ���������, ������������ �� ������ �������������

    private ARPlane _arPlane; // ��������� ARPlane
    private MeshRenderer _meshRenderer; // ��������� MeshRenderer
    private LineRenderer _lineRenderer; // ��������� LineRenderer

    private void Awake()
    {
        // ������������� �����������
        _arPlane = GetComponent<ARPlane>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _lineRenderer = GetComponent<LineRenderer>();

        if (_arPlane == null || _meshRenderer == null)
        {
            Debug.LogError("ARPlane ��� MeshRenderer ��������� �����������.");
            return;
        }

        // �������� ���� ��������� �� �������������
        _defaultColor = GetColorByClassification(_arPlane.classification);
        _defaultColor.a = DEFAULT_COLOR_ALPHA; // ������������� ������������ �����

        // ������������� ��������� ����
        UpdateColor();

        // �������� �� ������� ��������� ������ ���������
        _arPlane.boundaryChanged += OnPlaneBoundaryChanged;
    }

    private void OnDestroy()
    {
        // ������������ �� ������� ��� ����������� �������
        if (_arPlane != null)
        {
            _arPlane.boundaryChanged -= OnPlaneBoundaryChanged;
        }
    }

    private void OnPlaneBoundaryChanged(ARPlaneBoundaryChangedEventArgs eventArgs)
    {
        // ��������� ���� ��� ��������� ������ ���������
        UpdateColor();
    }

    // ����� ��� ���������� ����� ��������� ���������
    private void UpdateColor()
    {
        _meshRenderer.materials[0].color = _isVisualise ? Color.clear : _defaultColor;
        _lineRenderer.startColor = _isVisualise ? Color.clear : Color.white;
    }

    // ����� ��� ��������� ����� �� ������ ������������� ���������
    private static Color GetColorByClassification(PlaneClassification classifications) => classifications switch
    {
        PlaneClassification.None => Color.green,
        PlaneClassification.Wall => Color.white,
        PlaneClassification.Floor => Color.red,
        PlaneClassification.Ceiling => Color.yellow,
        PlaneClassification.Table => Color.blue,
        PlaneClassification.Seat => Color.blue,
        PlaneClassification.Door => Color.blue,
        PlaneClassification.Window => new Color(1f, 0.4f, 0f), //orange
        PlaneClassification.Other => Color.magenta,
        _ => Color.gray // ���� �� ���������
    };
}