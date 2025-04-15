public enum ParamType
{
    Slider,
    Toggle,
    Dropdown,
    Rebind
}

[System.Serializable]
public class ParamDefinition
{
    public string key;
    public string label;
    public string propertyPath;
    public ParamType type;
    public float minValue;
    public float maxValue;
    public bool showDecimal;
}

