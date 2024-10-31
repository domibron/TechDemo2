using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InteractableController : MonoBehaviour
{
	public LayerMask IgnoredLayers;

	public float MaxRange = 3f;

	public float MinAllignmentOfRay = 0.8f;

	public bool Locked = false;

	public TMP_Text ToolTip;

	private Camera _camera;
	private Transform _cameraTransform;

	private IInteractable _selectedObject;

	// Start is called before the first frame update
	void Start()
	{
		_camera = Camera.main;
		_cameraTransform = _camera.transform;

		IgnoredLayers = ~IgnoredLayers;
	}

	// Update is called once per frame
	void Update()
	{
		if (Locked)
		{
			DeselectSelectedObject();
			return;
		}

		GetNearInteractableObjects();

		if (Input.GetKeyDown(KeyCode.E) && _selectedObject != null)
		{
			_selectedObject.InteractWithObject();
		}

		if (_selectedObject != null && !Locked)
		{
			if (!string.IsNullOrEmpty(_selectedObject.InteractionToolTipDescriptive))
				ToolTip.text = "Press E to " + _selectedObject.InteractionToolTipDescriptive;
		}
		else
		{
			ToolTip.text = "";
		}
	}

	private void GetNearInteractableObjects()
	{
		//Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
		Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
		Debug.DrawRay(ray.origin, ray.direction, Color.red);

		Collider[] nearObjects = Physics.OverlapSphere(transform.position, MaxRange, IgnoredLayers, QueryTriggerInteraction.Ignore);

		if (nearObjects.Length <= 0)
		{
			DeselectSelectedObject();

			return;
		}


		Collider closetsObject = null;
		float Angle = MinAllignmentOfRay;
		float distanceOfClosestObject = MaxRange;

		for (int i = 0; i < nearObjects.Length; i++)
		{

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
			}
		}

		if (closetsObject == null)
		{

			DeselectSelectedObject();

			return;
		}

		SelectObject(closetsObject.transform);
	}

	private static Ray GetRay(Transform point)
	{
		return new Ray(point.position, point.forward);
	}

	private void SelectObject(Transform objectToSelect)
	{
		if (_selectedObject != null)
		{
			DeselectSelectedObject();
		}

		if (objectToSelect.GetComponent<IInteractable>() == null) return;

		_selectedObject = objectToSelect.GetComponent<IInteractable>();

		if (Physics.Raycast(GetRay(_cameraTransform), out RaycastHit raycastHit, MaxRange, IgnoredLayers))
		{
			if (raycastHit.transform.CompareTag("Interactable"))
			{
				_selectedObject.OnSelected();

				return;
			}
		}

		DeselectSelectedObject();

	}

	private void DeselectSelectedObject()
	{
		if (_selectedObject == null) return;

		_selectedObject.OnDeselected();

		_selectedObject = null;
	}
}
