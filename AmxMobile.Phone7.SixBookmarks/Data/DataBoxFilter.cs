using System;
using System.Collections.Generic;

namespace AmxMobile.Phone7.SixBookmarks
{
    internal class DataBoxFilter
    {
        private DataBox Box { get; set; }
        private List<SqlConstraint> Constraints { get; set; }

        internal DataBoxFilter(DataBox box)
        {
            this.Box = box;
            this.Constraints = new List<SqlConstraint>();
        }

        internal List<T> ExecuteEntityCollection<T>()
            where T : Entity
        {
            // get them all from the box...
            List<T> all = this.Box.GetAll<T>();

            // create a new collection to put the filtered results in...
            List<T> results = this.Box.EntityType.CreateCollectionInstance<T>();

            // walk the master list...
            foreach (T item in all)
            {
                bool ok = true;

                // look for non-matches...
                foreach (SqlConstraint constraint in this.Constraints)
                {
                    if (constraint.Field.Type == DataType.String)
                    {
                        string value = item.GetStringValue(constraint.Field);
                        if (string.Compare(value, (string)constraint.Value, StringComparison.InvariantCultureIgnoreCase) != 0)
                            ok = false;
                    }
                    else if (constraint.Field.Type == DataType.Int32)
                    {
                        int value = item.GetInt32Value(constraint.Field);
                        if (value != (int)constraint.Value)
                            ok = false;
                    }
                    else if (constraint.Field.Type == DataType.Boolean)
                    {
                        Boolean value = item.GetBooleanValue(constraint.Field);
                        if (value != (bool)constraint.Value)
                            ok = false;
                    }
                    else
                        throw new InvalidOperationException(string.Format("Cannot handle {0}.", constraint.Field.Type));

                    // stop early?
                    if(!(ok))
                        break;
                }

                // add if we're OK...
                if (ok)
                    results.Add(item);
            }

            // return...
            return results;
        }

        internal void AddConstraint(string name, object value)
        {
            EntityField field = this.Box.EntityType.GetField(name, true);
            if (field == null)
                throw new InvalidOperationException("'field' is null.");

            // defer...
            AddConstraint(field, value);
        }

        internal void AddConstraint(EntityField field, object value)
        {
            if (field == null)
                throw new ArgumentNullException("field");

            // add...
            this.Constraints.Add(new SqlConstraint(field, value));
        }

        internal T ExecuteEntity<T>()
            where T : Entity
        {
            List<T> items = ExecuteEntityCollection<T>();
            if (items.Count > 0)
                return items[0];
            else
                return null;
        }
    }
}
