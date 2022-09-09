using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The abstraction of an object that implements a health parameter
/// </summary>
public interface IHealth {
    /// <summary>
    /// An event to be raised when health is modified
    /// </summary>
    public event Action onHealthChanged;
    /// <summary>
    /// A property to get the maximum health the object can have
    /// </summary>
    public int MaxHealth { get; }
    /// <summary>
    /// A property to modify the objects current health
    /// </summary>
    public int Health { get; set; }

}
