using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AStarManager {
    public int CheckCount = 0;
    public double MillisecondsPast = 0;

    private Dictionary<Vector3Int, Nod> _nods;

    public AStarManager(Dictionary<Vector3Int, Nod> nodsMap) {
        _nods = nodsMap;
    }

    public Stack<Nod> FindPath(Algorithm algorithm, Nod start, Nod finish) {
        CheckCount = 0;
        MillisecondsPast = 0;
        DateTime before = DateTime.Now;
        Stack<Nod> path = null;
        switch (algorithm) {
            case Algorithm.DeepSearch:
                path = FindPathByDeepSearch(start, finish);
                break;

            case Algorithm.WidthSearch:
                path = FindPathByWidthSearch(start, finish);
                break;

            case Algorithm.AStar:
                path = FindPathByAStar(start, finish);
                break;
        }

        MillisecondsPast = (DateTime.Now - before).TotalMilliseconds;
        return path;
    }

    private Stack<Nod> FindPathByAStar(Nod start, Nod finish) {
        List<Nod> visited = new();
        Queue<Nod> frontier = new();
        start.previous = null;
        frontier.Enqueue(start);

        while (frontier.Count > 0) {
            CheckCount++;
            Nod current = frontier.Dequeue();
            visited.Add(current);

            if (current == finish) {
                return GetPathBack(current, new Stack<Nod>());
            }

            List<Nod> neighbours = new();
            foreach (Vector3Int neighbour in current.Neighbours) {
                if (!visited.Contains(_nods[neighbour])) {
                    neighbours.Add(_nods[neighbour]);
                    _nods[neighbour].additionalValue =
                        Vector3Int.Distance(_nods[neighbour].coordinates, finish.coordinates);
                }
            }

            neighbours = neighbours.OrderBy(a => a.additionalValue).ToList();
            foreach (Nod neighbour in neighbours) {
                frontier.Enqueue(neighbour);
                neighbour.previous = current;
            }
        }

        return default;
    }

    private Stack<Nod> FindPathByWidthSearch(Nod start, Nod finish) {
        List<Nod> visited = new();
        Queue<Nod> frontier = new();
        start.previous = null;
        frontier.Enqueue(start);

        while (frontier.Count > 0) {
            CheckCount++;
            Nod current = frontier.Dequeue();
            visited.Add(current);

            if (current == finish) {
                return GetPathBack(current, new Stack<Nod>());
            }

            List<Vector3Int> neighbours = current.Neighbours;
            neighbours = neighbours.OrderBy(a => Random.Range(0, 1f)).ToList();
            foreach (Vector3Int neighbour in neighbours)
                if (!visited.Contains(_nods[neighbour])) {
                    frontier.Enqueue(_nods[neighbour]);
                    _nods[neighbour].previous = current;
                }
        }

        return default;
    }

    private Stack<Nod> FindPathByDeepSearch(Nod start,
        Nod finish) {
        List<Nod> visited = new();
        Stack<Nod> frontier = new();
        frontier.Push(start);
        start.previous = null;

        while (frontier.Count > 0) {
            CheckCount++;
            Nod current = frontier.Pop();
            visited.Add(current);

            if (current == finish) {
                return GetPathBack(current, new Stack<Nod>());
            }

            List<Vector3Int> neighbours = current.Neighbours;
            neighbours = neighbours.OrderBy(a => Random.Range(0, 1f)).ToList();
            foreach (Vector3Int neighbour in neighbours)
                if (!visited.Contains(_nods[neighbour])) {
                    frontier.Push(_nods[neighbour]);
                    _nods[neighbour].previous = current;
                }
        }

        return default;
    }

    private static Stack<Nod> GetPathBack(Nod finish, Stack<Nod> curPath) {
        while (true) {
            curPath.Push(finish);
            if (finish.previous == null) {
                return curPath;
            }

            finish = finish.previous;
        }
    }
}

public enum Algorithm {
    DeepSearch,
    WidthSearch,
    AStar
}