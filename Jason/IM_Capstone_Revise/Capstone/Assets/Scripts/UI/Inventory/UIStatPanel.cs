using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStatPanel : MonoBehaviour
{
    [SerializeField] private StatTabType _tabType = default;

    public StatTabType TabType => _tabType;
}
