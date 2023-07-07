namespace MVC
{
    public abstract class Model
    {
        public static bool operator ==(Model m, Model n)
        {
            if (object.ReferenceEquals(m, n))
            {
                return true;
            }

            if (((object)m == null) || ((object)n == null))
            {
                return false;
            }

            return string.Equals(m.GetID(), n.GetID());
        }

        public static bool operator !=(Model m, Model n)
        {
            return !(m == n);
        }

        public override int GetHashCode()
        {
            string id = this.GetID();
            return string.IsNullOrEmpty(id) ? 0 : id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Model m = obj as Model;
            if ((object)m == null)
            {
                return false;
            }

            return string.Equals(this.GetID(), m.GetID());
        }

        public bool Equals(Model m)
        {
            if ((object)m == null)
            {
                return false;
            }

            return string.Equals(this.GetID(), m.GetID());
        }

        public abstract string GetID();

        public override string ToString()
        {
            return this.GetID();
        }
    }
}