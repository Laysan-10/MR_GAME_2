using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Hands.Samples.Gestures;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class Spawn_Object : MonoBehaviour
{
	[SerializeField] private XRRayInteractor xrRayInteractor;
	[SerializeField] private CarController car;
	[SerializeField] private StaticHandGesture _thumpup;
	[SerializeField] private StaticHandGesture _thumpdown;

	// Ссылка на Input Action 


	// Метод, вызываемый при нажатии на кнопку в руке
	public void SpawnObject(GameObject _spawn)
	{
	if (xrRayInteractor.enabled)
	{
		// Создаем объект в позиции руки (с небольшим смещением для видимости)
		var spawnPosition = xrRayInteractor.transform.position + xrRayInteractor.transform.forward * 0.5f; 
		var spawnedObject = Instantiate(_spawn, spawnPosition, xrRayInteractor.transform.rotation);
		spawnedObject.AddComponent<ARAnchor>(); 
	}
	}
	
	public void Show_Panel(GameObject _panel)
	{
		var spawnPosition = xrRayInteractor.transform.position + xrRayInteractor.transform.forward * 0.5f; 
		_panel.transform.position = spawnPosition;
		_panel.SetActive(true);
	}
	public void Car_Spawner(GameObject _car)
	{
		
		var spawnPosition = xrRayInteractor.transform.position + xrRayInteractor.transform.forward * 0.1f;
		var spawnedObject = Instantiate(_car, spawnPosition, xrRayInteractor.transform.rotation);
		car = FindObjectOfType<CarController>();
		car._raycast = FindObjectOfType<RayCast_For_Car>();
		_thumpdown.gesturePerformed.AddListener(car.GoReverse);
		_thumpup.gesturePerformed.AddListener(car.GoForward);

	}
}
