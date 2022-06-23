using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FloorsDataSO : ScriptableObject {
    public List<Nod> Nods;
    public List<Vector3Int> NodsVectors;

    public FloorsDataSO() {
        Nods = new List<Nod>();
    }

    public Dictionary<Vector3Int, Nod> GetNodsMap() {
        Dictionary<Vector3Int, Nod> dictionary = new();
        for (int i = 0; i < Nods.Count; i++) {
            dictionary.Add(NodsVectors[i], Nods[i]);
        }

        return dictionary;
    }
}

[System.Serializable]
public class Nod {
    [SerializeField]
    public List<Vector3Int> Neighbours;
    [NonSerialized]
    public Nod previous;
    [NonSerialized]
    public float additionalValue;
    public Vector3Int coordinates;
    public int type;
}