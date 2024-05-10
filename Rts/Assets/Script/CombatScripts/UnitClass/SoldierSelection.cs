using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSelection : MonoBehaviour
{
    private Camera mainCamera;
    public LayerMask targetLayer;
    public Soldier selectedSoldier;
    private GameObject selectedMarker;
    public GameObject groundMarker;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit, 500, targetLayer))
            {
                Soldier newSelectedSoldier = hit.transform.gameObject.GetComponent<Soldier>();
                if (newSelectedSoldier != null && newSelectedSoldier != selectedSoldier) // Verificar si el objeto golpeado es un soldado y es diferente al seleccionado actualmente
                {
                    DeselectSoldier();
                    selectedSoldier = newSelectedSoldier;
                    selectedSoldier.transform.GetChild(2).gameObject.SetActive(true);
                    Debug.Log(selectedSoldier.gameObject.name + " seleccionado.");
                }
            }
            else // Si el clic no golpea un soldado, deseleccionar todo
            {
                DeselectSoldier();
                Debug.Log("Se deseleccionó.");
            }
        }
        if (Input.GetMouseButtonUp(1) && selectedSoldier != null)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    selectedSoldier.target = hit.collider.gameObject;
                    selectedSoldier.canMove = true;
                    Debug.Log("Se va a mover.");
                }
                else if (hit.collider.gameObject.layer == 7)
                {
                    groundMarker.transform.position = hit.point;
                    selectedSoldier.target = groundMarker;
                    selectedSoldier.groundTarget = true;
                    selectedSoldier.canMove = true;
                    //StartCoroutine(DeactivateMarker(selectedSoldier));
                }
                else
                {
                    DeselectSoldier();
                    Debug.Log("Se deseleccionó.");
                }
            }
        }
    }

    void DeselectSoldier()
    {
        if (selectedSoldier != null)
        {
            selectedSoldier.transform.GetChild(2).gameObject.SetActive(false);
            selectedSoldier = null;
        }
    }
    IEnumerator DeactivateMarker(Soldier soldado)
    {
        yield return new WaitForSeconds(2f);
        groundMarker.gameObject.SetActive(false);
    }
}
