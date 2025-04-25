using System;
using System.Collections.Generic;

public class SkipList<TKey, TValue> where TKey : IComparable<TKey>
{
    private class Node<TNodeKey, TNodeValue>
    {
        public TNodeKey Key { get; }
        public TNodeValue Value { get; set; }
        public Node<TNodeKey, TNodeValue>[] Forward { get; }

        public Node(TNodeKey key, TNodeValue value, int level)
        {
            Key = key;
            Value = value;
            Forward = new Node<TNodeKey, TNodeValue>[level + 1];
        }
    }

    private const int MaxLevel = 32;
    private readonly Random _random = new Random();
    private int _currentLevel;
    private Node<TKey, TValue> _header;

    public SkipList()
    {
        _header = new Node<TKey, TValue>(default, default, MaxLevel);
        _currentLevel = 0;
    }

    public void Add(TKey key, TValue value)
    {
        var update = new Node<TKey, TValue>[MaxLevel + 1];
        var current = _header;

        for (int i = _currentLevel; i >= 0; i--)
        {
            while (current.Forward[i] != null && current.Forward[i].Key.CompareTo(key) < 0)
                current = current.Forward[i];
            update[i] = current;
        }

        current = current.Forward[0];
        if (current != null && current.Key.CompareTo(key) == 0)
        {
            current.Value = value;
            return;
        }

        int level = RandomLevel();
        if (level > _currentLevel)
        {
            for (int i = _currentLevel + 1; i <= level; i++)
                update[i] = _header;
            _currentLevel = level;
        }

        var newNode = new Node<TKey, TValue>(key, value, level);
        for (int i = 0; i <= level; i++)
        {
            newNode.Forward[i] = update[i].Forward[i];
            update[i].Forward[i] = newNode;
        }
    }

    public bool Remove(TKey key)
    {
        var update = new Node<TKey, TValue>[MaxLevel + 1];
        var current = _header;

        for (int i = _currentLevel; i >= 0; i--)
        {
            while (current.Forward[i] != null && current.Forward[i].Key.CompareTo(key) < 0)
                current = current.Forward[i];
            update[i] = current;
        }

        current = current.Forward[0];
        if (current == null || current.Key.CompareTo(key) != 0)
            return false;

        for (int i = 0; i <= _currentLevel; i++)
        {
            if (update[i].Forward[i] != current)
                break;
            update[i].Forward[i] = current.Forward[i];
        }

        while (_currentLevel > 0 && _header.Forward[_currentLevel] == null)
            _currentLevel--;

        return true;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        var current = _header;

        for (int i = _currentLevel; i >= 0; i--)
        {
            while (current.Forward[i] != null && current.Forward[i].Key.CompareTo(key) < 0)
                current = current.Forward[i];
        }

        current = current.Forward[0];
        if (current != null && current.Key.CompareTo(key) == 0)
        {
            value = current.Value;
            return true;
        }

        value = default;
        return false;
    }

    private int RandomLevel()
    {
        int level = 0;
        while (_random.NextDouble() < 0.5 && level < MaxLevel)
            level++;
        return level;
    }
}