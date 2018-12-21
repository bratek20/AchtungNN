using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DotConfig", menuName = "ScriptableObjects/DotConfig")]
public class DotConfig : ScriptableObject {
    public Color color;
    public string leftKey;
    public string rightKey;
}
