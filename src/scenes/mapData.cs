public class MapData
{
    public Layer[] layers { get; set; }
}

public class Layer
{
    public string name { get; set; }
    public Object[] objects { get; set; }
}

public class Object
{
    public float x { get; set; }
    public float y { get; set; }
    public float width { get; set; }
    public float height { get; set; }
    public string name { get; set; }
}
