using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodObject : MonoBehaviour {
    [SerializeField]
    private MeshRenderer _meshRenderer;

    [SerializeField]
    private Transform _line;
    
    private Color normalColor;
    public States curState;
    public Nod nod;
    public void Init(Nod myNod) {
        nod = myNod;
        SetState(nod.type);
        normalColor = _meshRenderer.material.color;
    }

    public void RevertToNormal() {
        _meshRenderer.enabled = true;
        _meshRenderer.material.color = normalColor;
        _line.gameObject.SetActive(false);
    }

    public void SetState(States state) {
        _meshRenderer.enabled = state != States.Invisible;
        curState = state;
        switch (state) {
            case States.Empty:
                _meshRenderer.material.color = Color.white;
                break;

            case States.Wall:
                _meshRenderer.material.color = Color.black;
                break;

            case States.Stairs:
                _meshRenderer.material.color = Color.grey;
                break;

            case States.Selected:
                _meshRenderer.material.color = Color.yellow;
                break;

            case States.Path:
               
                _meshRenderer.material.color = Color.red;
                break;
        }
    }

    public void SetLine(Nod next) {
        _line.gameObject.SetActive(true);
        _line.transform.rotation = Quaternion.LookRotation(next.coordinates - transform.position);
    }
    
    public void SetState(int stateIndex) {
        States state = (States) stateIndex;
        SetState(state);
    }
}

public enum States {
    Invisible = -1,
    Empty,
    Wall,
    Stairs,
    Selected,
    Path
}