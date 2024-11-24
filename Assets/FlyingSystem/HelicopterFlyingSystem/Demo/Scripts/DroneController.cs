using UnityEngine;

using FlyingSystem;

public class DroneController : MonoBehaviour
{
	[SerializeField] UnityEngine.XR.Content.Interaction.XRJoystick _joystic_right;
	[SerializeField] UnityEngine.XR.Content.Interaction.XRJoystick _joystic_left;
	[SerializeField] UnityEngine.XR.Content.Interaction.XRPushButton  _button;
	public Transform springArmTransform;
	public Camera characterCamera;

	public Rigidbody rootRigidbody;

	public Transform rotorTransform1, rotorTransform2, rotorTransform3, rotorTransform4;

	private HelicopterFlyingSystem helicopterFlyingSystem;

	public bool activated = false;

	public float cameraSpeed = 300.0f;

	[Header("General Attributes")]
	public bool takeOff = true;
	public bool boosting;

	private bool draggingMouse = false;

	private float accumulatedDeltaMousePositionX, accumulatedDeltaMousePositionY;

	[Header("Mobile")]
	public bool mobileInputControl = false;
	public float mobileCameraSpeed = 300.0f;
	private float screenCenterX;

	void Start()
	{
		if (activated)
			Activate();

		helicopterFlyingSystem = this.GetComponent<HelicopterFlyingSystem>();

		screenCenterX = screenCenterX = Screen.width / 2.0f;
	}

	void Update()
	{
		// Debug.Log(_joystic_left.recenterOnRelease);
		
		if (activated)
		{
			if (!mobileInputControl)
			{
				if (!draggingMouse)
					PCCameraControlLogic();

				PCInputControlLogic();
			}
			else
			{
				MobileCameraControlLogic();
			}

			rotorTransform1.Rotate(Vector3.forward * 1080.0f * Time.deltaTime);
			rotorTransform2.Rotate(Vector3.forward * 1080.0f * Time.deltaTime);
			rotorTransform3.Rotate(Vector3.forward * 1080.0f * Time.deltaTime);
			rotorTransform4.Rotate(Vector3.forward * 1080.0f * Time.deltaTime);
		}
	}

	public void Activate()
	{
		activated = true;
		// characterCamera.enabled = true;
		// characterCamera.GetComponent<AudioListener>().enabled = true;
	}

	public void Deactivate()
	{
		activated = false;
		characterCamera.enabled = false;
		characterCamera.GetComponent<AudioListener>().enabled = false;
	}

	void PCCameraControlLogic()
	{ if(springArmTransform != null)
	{
		springArmTransform.rotation = Quaternion.Euler(springArmTransform.rotation.eulerAngles.x - Input.GetAxis("Mouse Y") * cameraSpeed * Time.deltaTime, springArmTransform.rotation.eulerAngles.y + Input.GetAxis("Mouse X") * cameraSpeed * Time.deltaTime, 0.0f);
	}
	}
		

	void MobileCameraControlLogic()
	{
		// Temporarily use mouse to simulate the touch
		if (Input.GetMouseButton(0) && Input.mousePosition.x > screenCenterX)
		{
			springArmTransform.Rotate(Vector3.up * mobileCameraSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
			springArmTransform.Rotate(-Vector3.right * mobileCameraSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime);
		}

		// Only detects on mobile devices
		if (Input.touchCount > 0)
		{
			for (var i = 0; i < Input.touchCount; i++)
			{
				if (Input.GetTouch(i).position.x > screenCenterX && Input.GetTouch(i).phase == TouchPhase.Moved)
				{
					springArmTransform.Rotate(Vector3.up * mobileCameraSpeed * Input.GetTouch(i).deltaPosition.x * Time.deltaTime);
					springArmTransform.Rotate(-Vector3.right * mobileCameraSpeed * Input.GetTouch(i).deltaPosition.y * Time.deltaTime);
				}
			}
		}
	}

	void PCInputControlLogic()
	{
		if (_button.m_Pressed)
			Activate();
			TakeOffOrLand();

		// Hold down to turn left / right
		if (_joystic_left.value.x < 0) //кнопка А
		{
			TakeOff();
			helicopterFlyingSystem.AddYawInput(-1.0f);
		}
		else if (_joystic_left.value.x > 0) //кнопка D
		{
			TakeOff();
			helicopterFlyingSystem.AddYawInput(1.0f);
		}

		// Hold down to ascend / descend
		if (_joystic_left.value.y > 0) // кнопка W
		{
			TakeOff();
			Ascend();
		}
		else if (_joystic_left.value.y < 0) //кнопка S
		{
			TakeOff();
			Descend();
		}

		if (_joystic_left.value.x == 0 && _joystic_left.value.y == 0)
			helicopterFlyingSystem.VerticalSlowDown();

		// Hold down mouse left button and drag to move
		if (_joystic_right.move_x != 0 && _joystic_right.value.y != 0) // если нажата кнопка
			draggingMouse = true;

		if (draggingMouse)
		{
			// accumulatedDeltaMousePositionX += Mathf.Clamp(Input.GetAxis("Mouse X"), -1.0f, 1.0f);
			// accumulatedDeltaMousePositionY += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1.0f, 1.0f);

			helicopterFlyingSystem.AddHorizontalInput(new Vector2(_joystic_right.value.x, _joystic_right.value.y));//значения от положения мыши

			TakeOff();
		}

		if (_joystic_right.move_x == 0 && _joystic_right.value.y == 0) //если кнопка отпущена
		{
			draggingMouse = false;
			accumulatedDeltaMousePositionX = 0.0f;
			accumulatedDeltaMousePositionY = 0.0f;
			helicopterFlyingSystem.StopYawInput();
		}

		// Boost on / off
		// if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
		// 	Boost();
	}

	

	public void TakeOffOrLand()
	{
		if (helicopterFlyingSystem.inAir)
		{
			helicopterFlyingSystem.Land();
			takeOff = helicopterFlyingSystem.inAir;

			rootRigidbody.useGravity = true;
			rootRigidbody.constraints = RigidbodyConstraints.None;
		}
		else
		{
			TakeOff();
		}
	}

	void TakeOff()
	{
		if (!helicopterFlyingSystem.inAir)
		{
			helicopterFlyingSystem.TakeOff();
			takeOff = helicopterFlyingSystem.inAir;

			rootRigidbody.useGravity = false;
			rootRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
		}
	}

	public void Boost()
	{
		helicopterFlyingSystem.boosting = !helicopterFlyingSystem.boosting;
		boosting = helicopterFlyingSystem.boosting;
	}

	public void MobileTurnLeft()
	{
		TakeOff();
		helicopterFlyingSystem.AddYawInput(-1.0f);
	}

	public void MobileTurnRight()
	{
		TakeOff();
		helicopterFlyingSystem.AddYawInput(1.0f);
	}

	public void Ascend()
	{
		TakeOff();
		helicopterFlyingSystem.AddVerticalInput(1.0f);
	}

	public void Descend()
	{
		TakeOff();
		helicopterFlyingSystem.AddVerticalInput(-1.0f);
	}

	public void MobileStopAscendOrDescend()
	{
		helicopterFlyingSystem.VerticalSlowDown();
	}
	public float GetFlyingSpeed()
	{
		return helicopterFlyingSystem.horizontalFlyingSpeed;
	}

	public float GetPowerPercentage()
	{
		return helicopterFlyingSystem.powerPercentage;
	}

	public float GetWeightPercentage()
	{
		return helicopterFlyingSystem.weightPercentage;
	}
}