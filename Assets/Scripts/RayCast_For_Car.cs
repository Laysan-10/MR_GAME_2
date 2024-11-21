using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast_For_Car : MonoBehaviour
{
    public Transform rayOrigin; // �����, �� ������� ������� ���
    public float rayLength = 10f; // ����� ����
    public Transform hand;
    public float angle;



    public LayerMask layerMask;



    void Update()
    {
        //����������� ���� ���������
        // ����������� ���� (�����)
        Vector3 direction = transform.position;
        Vector3 direction_2 = transform.forward;


        // ������ ����
        Vector3 origin = rayOrigin.position;

        // ����� ����
        Vector3 end = origin + direction * rayLength;

        // ������ ���
        Debug.DrawLine(origin, end, Color.red);



        float dotProduct = Vector3.Dot(direction, direction_2);
        angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;


        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction_2, out hit, rayLength, layerMask))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            Debug.DrawLine(transform.position, hit.point, Color.green);
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + direction_2 * rayLength, Color.red);
        }

        Vector3 crossProduct = Vector3.Cross(direction, direction_2);
        float sign = Mathf.Sign(crossProduct.y); //��� z, � ����������� �� ���������� ����� ������� ���������

        angle *= sign;

        //Debug.Log("���� ����� ������: " + angle);

        //������ ���� ����� ����� raycast
    }
}
