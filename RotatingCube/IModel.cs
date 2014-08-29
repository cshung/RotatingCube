namespace RotatingCube
{
    using System.Collections.Generic;

    public interface IModel
    {
        IEnumerable<Triangle> Triangles { get; }
    }
}
