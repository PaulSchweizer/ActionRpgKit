﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ActionRpgKit.Item;
using System.IO;

public class UItemDatabase : MonoBehaviour
{
    public static UItemDatabase Instance;
    public UItem[] Items;

    /// <summary>
    /// Singleton pattern and initializing the ActionRpgKit.ItemDatabase.</summary>
    protected void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }

        InitDatabase();
    }

    /// <summary>
    /// Get all Items and set them to the ActionRpgKit ItemDatabase.</summary>
    private void InitDatabase()
    {
        List<IItem> items = new List<IItem>();
        foreach (UItem uitem in Items)
        {
            if (uitem is UUsableItem)
            {
                var uUsableItem = uitem as UUsableItem;
                var iItem = uUsableItem.Item as IItem;
                items.Add(iItem);
            }
            else if (uitem is UWeaponItem)
            {
                var uWeaponItem = uitem as UWeaponItem;
                var iItem = uWeaponItem.Item as IItem;
                items.Add(iItem);
            }
        }
        ItemDatabase.Items = items.ToArray();
    }
}