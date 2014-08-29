namespace RotatingCube
{
    using System.Collections.Generic;
    using System.Linq;

    public class TransformModel : IModel
    {
        public IModel Original
        {
            get;
            set;
        }

        public AbstractTransform Transform
        {
            get;
            set;
        }

        public IEnumerable<Triangle> Triangles
        {
            get
            {
                return this.Original.Triangles.Select(t => this.Transform.Transform(t));
            }
        }
    }
}
