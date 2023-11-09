using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;
    public World world;

    Dictionary<string, AssetInfo> assets = new Dictionary<string, AssetInfo>();
    bool isLoaded;
    public HashSet<GameObject> deleteQueue = new HashSet<GameObject>();

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            instance.LoadAssets();
        }
        else
            Debug.LogWarning("Cannot have more than 1 Game behaviours in the world");
    }

    public T GetAsset<T>(string name) where T : AssetInfo, new()
    {
        if (assets.ContainsKey(name) && assets[name] is T)
            return (T)assets[name];
        return null;
    }

    public void Update()
    {
        foreach (var item in deleteQueue)
        {
            Destroy(item);
        }
        deleteQueue.Clear();
    }

    public void Delete(GameObject obj)
    {
        deleteQueue.Add(obj);
    }

    void LoadAssets()
    {
        if (isLoaded)
            return;
        AssetInfo[] loaded = Resources.LoadAll<AssetInfo>("");
        foreach (var item in loaded)
        {
            assets.Add(item.Name, item);
            if (item is SmelterRecipeInfo sri)
                SmelterRecipeInfo.recipes.Add(sri);
        }
        isLoaded = true;
    }

    internal List<T> GetAllAssets<T>() where T : AssetInfo, new()
    {
        List<T> list = new List<T>();
        foreach (var item in assets)
        {
            if (item.Value is T t)
                list.Add(t);
        }
        return list;
    }
}
