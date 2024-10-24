using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
	public LayerMask IgnoredLayers;

	public float MaxRange = 3f;

	public float MinAllignmentOfRay = 0.8f;

	private Camera _camera;
	private Transform _cameraTransform;

	private IInteractable _selectedObject;

	// Start is called before the first frame update
	void Start()
	{
		_camera = Camera.main;
		_cameraTransform = _camera.transform;
	}

	// Update is called once per frame
	void Update()
	{
		GetNearInteractableObjects();

		if (Input.GetKeyDown(KeyCode.E) && _selectedObject != null)
		{
			_selectedObject.InteractWithObject();
		}
	}

	private void GetNearInteractableObjects()
	{
		//Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
		Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
		Debug.DrawRay(ray.origin, ray.direction, Color.red);

		Collider[] nearObjects = Physics.OverlapSphere(transform.position, MaxRange, ~IgnoredLayers, QueryTriggerInteraction.Ignore);

		if (nearObjects.Length <= 0)
		{
			DeselectSelectedObject();

			return;
		}

		// List<Collider> objectsNearRay = new List<Collider>();


		Collider closetsObject = null;
		float Angle = MinAllignmentOfRay;
		float distanceOfClosestObject = MaxRange;

		for (int i = 0; i < nearObjects.Length; i++)
		{
			print(Vector3.Dot(ray.direction.normalized, (nearObjects[i].transform.position - _cameraTransform.position).normalized) + $" > {Angle} | " + Vector3.Distance(_cameraTransform.position, nearObjects[i].transform.position) + $" < {distanceOfClosestObject}");


			if (Vector3.Dot(ray.direction.normalized, (nearObjects[i].transform.position - _cameraTransform.position).normalized) > Angle && Vector3.Distance(_cameraTransform.position, nearObjects[i].transform.position) < distanceOfClosestObject && closetsObject == null)
			{
				closetsObject = nearObjects[i];
				distanceOfClosestObject = Vector3.Distance(_cameraTransform.position, nearObjects[i].transform.position);

				Angle = Vector3.Dot(ray.direction.normalized, (nearObjects[i].transform.position - _cameraTransform.position).normalized);

				continue;
			}
			else if (Vector3.Dot(ray.direction.normalized, (nearObjects[i].transform.position - _cameraTransform.position).normalized) > Angle && Vector3.Distance(_cameraTransform.position, nearObjects[i].transform.position) < distanceOfClosestObject)
			{


				closetsObject = nearObjects[i];
				distanceOfClosestObject = Vector3.Distance(_cameraTransform.position, nearObjects[i].transform.position);

				Angle = Vector3.Dot(ray.direction.normalized, (nearObjects[i].transform.position - _cameraTransform.position).normalized);

				continue;


				//objectsNearRay.Add(nearObjects[i]);
			}
		}

		// if (objectsNearRay.Count <= 0)
		// {
		// 	DeselectSelectedObject();

		// 	return;
		// }


		// if (objectsNearRay.Count == 1)
		// {
		// 	SelectObject(objectsNearRay[0].transform);

		// 	return;
		// }



		// for (int i = 0; i < objectsNearRay.Count; i++)
		// {

		// 	if (closetsObject == null)
		// 	{
		// 		closetsObject = objectsNearRay[i];
		// 		distanceOfClosestObject = Vector3.Distance(transform.position, objectsNearRay[i].transform.position);
		// 		continue;
		// 	}

		// 	if (Vector3.Distance(transform.position, closetsObject.transform.position) < distanceOfClosestObject)
		// 	{
		// 		closetsObject = objectsNearRay[i];
		// 		distanceOfClosestObject = Vector3.Distance(transform.position, objectsNearRay[i].transform.position);
		// 		continue;
		// 	}
		// }

		if (closetsObject == null)
		{

			DeselectSelectedObject();

			return;
		}

		SelectObject(closetsObject.transform);
	}

	private void SelectObject(Transform objectToSelect)
	{
		if (_selectedObject != null)
		{
			DeselectSelectedObject();
		}

		if (objectToSelect.GetComponent<IInteractable>() == null) return;

		_selectedObject = objectToSelect.GetComponent<IInteractable>();

		_selectedObject.OnSelected();
	}

	private void DeselectSelectedObject()
	{
		if (_selectedObject == null) return;

		_selectedObject.OnDeselected();

		_selectedObject = null;
	}
}
