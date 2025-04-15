using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AudioSettingsCategory", menuName = "Settings/Category")]
public class SettingsCategory : ScriptableObject
{
    public List<ParamDefinition> parameters;
}
