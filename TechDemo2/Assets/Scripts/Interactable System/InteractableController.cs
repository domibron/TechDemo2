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

    private IInteractable _selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
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
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        Collider[] nearObjects = Physics.OverlapSphere(transform.position, MaxRange, ~IgnoredLayers, QueryTriggerInteraction.Ignore);

        if (nearObjects.Length <= 0)
        {
            DeselectSelectedObject();

            return;
        }

        List<Collider> objectsNearRay = new List<Collider>();

        for (int i = 0; i < nearObjects.Length; i++)
        {
            if (Vector3.Dot(ray.direction, (nearObjects[i].transform.position - transform.position)) > MinAllignmentOfRay)
            {
                objectsNearRay.Add(nearObjects[i]);
            }
        }

        if (objectsNearRay.Count <= 0)
        {
            DeselectSelectedObject();

            return;
        }


        if (objectsNearRay.Count == 1)
        {
            SelectObject(objectsNearRay[0].transform);

            return;
        }

        Collider closetsObject = null;
        float distanceOfClosestObject = MaxRange;

        for (int i = 0; i < objectsNearRay.Count; i++)
        {

            if (closetsObject == null)
            {
                closetsObject = objectsNearRay[i];
                distanceOfClosestObject = Vector3.Distance(transform.position, objectsNearRay[i].transform.position);
                continue;
            }

            if (Vector3.Distance(transform.position, closetsObject.transform.position) < distanceOfClosestObject)
            {
                closetsObject = objectsNearRay[i];
                distanceOfClosestObject = Vector3.Distance(transform.position, objectsNearRay[i].transform.position);
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
