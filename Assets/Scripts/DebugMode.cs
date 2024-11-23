using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARPlaneManager))]
public class DebugMode : MonoBehaviour
{
	
	[SerializeField]
	private InputActionReference toggleSurfaceRenderingAction; // �������� ����� ��� ������������ ������������

	[SerializeField] private bool isVisualiseOnStart; // ��������� ��������� ������������

	private bool _isVisualise; // ������� ��������� ������������
	private ARPlaneManager _planeManager; // ��������� ��� ���������� AR-�����������

	[SerializeField] private Canvas debugPanel;

	private void Awake()
	{


		// ������������� �����������
		_planeManager = GetComponent<ARPlaneManager>();
		_isVisualise = isVisualiseOnStart; // ������������� ��������� ��������� ������������
	}

	public void OnEnable()
	{
		// �������� �� �������� ����� ��� ������������ ������������
		toggleSurfaceRenderingAction.action.started += OnToggleSurfaceRendering;

		// �������� �� ������� ��������� AR-���������� � ������
		_planeManager.planesChanged += OnPlanesChanged;

		// ���������� ��������� ������������
		PlaneUpdateVisualisation();
	}

	public void OnDisable()
	{
		// ������� �� �������� �����
		toggleSurfaceRenderingAction.action.started -= OnToggleSurfaceRendering;

		// ������� �� ������� ��������� AR-���������� � ������
		_planeManager.planesChanged -= OnPlanesChanged;
	}

	// �����-���������� ��� ��������� ��������� AR-����������
	private void OnPlanesChanged(ARPlanesChangedEventArgs arPlanesChangedEventArgs) => PlaneUpdateVisualisation();

	// �����-���������� ��� ������������ ������������ ��� ������������ �������� �����
	private void OnToggleSurfaceRendering(InputAction.CallbackContext obj)
	{
		_isVisualise = !_isVisualise; // ������������ ��������� ������������
		debugPanel.enabled = _isVisualise;
		PlaneUpdateVisualisation();
	}

	// ���������� ������������ AR-����������
	private void PlaneUpdateVisualisation()
	{
		foreach (var arPlane in _planeManager.trackables)
		{
			if (arPlane.TryGetComponent(out ARPlaneColorizer arPlaneColorizer))
			{
				arPlaneColorizer.isVisualise = _isVisualise; // ������������� ��������� ������������
			}
		}
	}
}