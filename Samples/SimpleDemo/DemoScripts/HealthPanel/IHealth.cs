using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth {
    public event Action onHealthChanged;

    public int MaxHealth { get; }

    public int Health { get; set; }

}
