using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Hands.Samples.Gestures;
using UnityEngine.XR.Hands.Samples.GestureSample;
using System.Numerics;
 using UnityEngine.SceneManagement;
public class Spawn_Object : MonoBehaviour
{
	[SerializeField] private XRRayInteractor xrRayInteractor;
	[SerializeField] private CarController car;
	[SerializeField] private StaticHandGesture _thumpup;
	[SerializeField] private StaticHandGesture _thumpdown;
	[SerializeField] private StaticHandGesture _hand_to_stop_car;
	

	// Ссылка на Input Action 

 public void Quit(){
	Application.Quit();
 }
	// Метод, вызываемый при нажатии на кнопку в руке
	public void SpawnObject(GameObject _spawn)
	{
	if (xrRayInteractor.enabled)
	{
		Debug.Log("CSpawnObject");
		
		// Создаем объект в позиции руки (с небольшим смещением для видимости)
		var spawnPosition = xrRayInteractor.transform.position + xrRayInteractor.transform.forward * 0.5f; 
		var spawnedObject = Instantiate(_spawn, spawnPosition, xrRayInteractor.transform.rotation);
		spawnedObject.AddComponent<ARAnchor>(); 
	}
	}
	
	public void Show_Panel(GameObject _panel)
	{
		Debug.Log("Show_Panel");
		
		var spawnPosition = xrRayInteractor.transform.position + xrRayInteractor.transform.forward * 0.5f; 
		_panel.transform.position = spawnPosition;
		_panel.SetActive(true);
	}
	public void Car_Spawner(GameObject _car)
	{
		Debug.Log("Car_Spawner");
		var spawnPosition = xrRayInteractor.transform.position + xrRayInteractor.transform.forward * 0.1f;
		
		var spawnedObject = Instantiate(_car, spawnPosition, xrRayInteractor.transform.rotation);
		spawnedObject.transform.Rotate(0, 0, 0);
		car = FindObjectOfType<CarController>();
		car._raycast = FindObjectOfType<RayCast_For_Car>();
		_thumpdown.gesturePerformed.AddListener(car.GoReverse);
		_thumpup.gesturePerformed.AddListener(car.GoForward);
		_hand_to_stop_car.gesturePerformed.AddListener(car.ThrottleOff);
		

	}	
}
