using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RenderManager : MonoBehaviour {
    [SerializeField]
    private FloorsDataSO dataSo;

    [SerializeField]
    private NodObject nodPrefab;

    [SerializeField]
    private Transform nodsContainer;

    private NodObject start, finish;
    private Dictionary<Nod, NodObject> _nodObjects;
    private bool isPathSelected;
    public bool TryGetPoints(out Nod _start, out Nod _finish) {
        if (start == null || finish == null) {
            _start = null;
            _finish = null;
            return false;
        }

        _start = start.nod;
        _finish = finish.nod;
        return true;
    }

    public void SelectPath( Stack<Nod> path) {
        RevertAllNods();
        Nod previousNod = null;
        foreach (Nod nod in path) {
            _nodObjects[nod].SetState(States.Path);
            if (previousNod != null) {
                _nodObjects[nod].SetLine(previousNod);
            }
            previousNod = nod;
        }
        isPathSelected = true;
    }

    private void RevertAllNods() {
        foreach (NodObject nodObj in _nodObjects.Values) {
            nodObj.RevertToNormal();
        } 
        isPathSelected = false;
    }
    
    private void Start() {
        DrawNods();
    }

    private void DrawNods() {
        _nodObjects = new();
        foreach (Nod nod in dataSo.Nods) {
            NodObject nodObj = Instantiate(nodPrefab, nod.coordinates, Quaternion.identity, nodsContainer);
            nodObj.Init(nod);
            _nodObjects.Add(nod,nodObj);
        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            TrySelectStart();
        }

        if (Input.GetMouseButtonDown(1)) {
            TrySelectFinish();
        }
    }

    private void TrySelectStart() {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100)) {
            return;
        }

        NodObject obj = hitInfo.transform.gameObject.GetComponent<NodObject>();
        if (obj.curState == States.Wall) {
            Debug.Log("Cant select wall!");
            return;
        }

        if (obj == finish) {
            Debug.Log("Choose different nods!");
            return;
        }
        if (isPathSelected) {
            RevertAllNods();
        }

        if (start != null) {
            start.RevertToNormal();
        }

        start = obj;
        obj.SetState(States.Selected);
    }

    private void TrySelectFinish() {
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 100)) {
            return;
        }

        NodObject obj = hitInfo.transform.gameObject.GetComponent<NodObject>();
        if (obj.curState == States.Wall) {
            Debug.Log("Cant select wall!");
            return;
        }

        if (obj == start) {
            Debug.Log("Choose different nods!");
            return;
        }

        if (isPathSelected) {
            RevertAllNods();
        }

        if (finish != null) {
            finish.RevertToNormal();
        }

        finish = obj;
        obj.SetState(States.Selected);
    }
}