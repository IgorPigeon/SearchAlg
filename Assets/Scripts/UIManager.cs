using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private RenderManager _renderManager;

    [SerializeField]
    private FloorsDataSO dataSo;

    [SerializeField]
    private TMP_Dropdown algoritmDropdown;

    private Algorithm _curAlgorithm;
    private AStarManager _aStarManager;

    private void Awake() {
        _curAlgorithm = Algorithm.DeepSearch;
        FillDropdown();
        _aStarManager = new AStarManager(dataSo.GetNodsMap());
    }

    private void FillDropdown() {
        List<string> options = new() {
            Algorithm.DeepSearch.ToString(),
            Algorithm.WidthSearch.ToString(),
            Algorithm.AStar.ToString()
        };

        algoritmDropdown.ClearOptions();
        algoritmDropdown.AddOptions(options);
    }

    public void SelectAlgorithm(int value) {
        _curAlgorithm = (Algorithm) value;
    }

    public void FindPath() {
        if (_renderManager.TryGetPoints(out Nod start, out Nod finish)) {
            Stack<Nod> path = null;
            path = _aStarManager.FindPath(_curAlgorithm, start, finish);

            _renderManager.SelectPath(path);
            Debug.Log(
                $"Path found in {_aStarManager.CheckCount} checks and {_aStarManager.MillisecondsPast} milliseconds!");
        } else {
            Debug.Log("Choose start & finish points!");
        }
    }
}