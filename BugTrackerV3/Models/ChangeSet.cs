using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerV3.Models
{
    using FrameLog.Models;

    public class ChangeSet : IChangeSet<ApplicationUser>
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public ApplicationUser Author { get; set; }
        public virtual List<ObjectChange> ObjectChanges { get; set; }

        IEnumerable<IObjectChange<ApplicationUser>> IChangeSet<ApplicationUser>.ObjectChanges
        {
            get { return ObjectChanges; }
        }

        void IChangeSet<ApplicationUser>.Add(IObjectChange<ApplicationUser> objectChange)
        {
            ObjectChanges.Add((ObjectChange)objectChange);
        }

        public override string ToString()
        {
            return string.Format("By {0} on {1}, with {2} ObjectChanges",
                Author, Timestamp, ObjectChanges.Count);
        }
    }

    public class ObjectChange : IObjectChange<ApplicationUser>
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string ObjectReference { get; set; }
        public virtual ChangeSet ChangeSet { get; set; }
        public virtual List<PropertyChange> PropertyChanges { get; set; }

        IEnumerable<IPropertyChange<ApplicationUser>> IObjectChange<ApplicationUser>.PropertyChanges
        {
            get { return PropertyChanges; }
        }
        void IObjectChange<ApplicationUser>.Add(IPropertyChange<ApplicationUser> propertyChange)
        {
            PropertyChanges.Add((PropertyChange)propertyChange);
        }
        IChangeSet<ApplicationUser> IObjectChange<ApplicationUser>.ChangeSet
        {
            get { return ChangeSet; }
            set { ChangeSet = (ChangeSet)value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", TypeName, ObjectReference);
        }
    }

    public class PropertyChange : IPropertyChange<ApplicationUser>
    {
        public int Id { get; set; }
        public virtual ObjectChange ObjectChange { get; set; }
        public string PropertyName { get; set; }
        public string Value { get; set; }
        public int? ValueAsInt { get; set; }

        IObjectChange<ApplicationUser> IPropertyChange<ApplicationUser>.ObjectChange
        {
            get { return ObjectChange; }
            set { ObjectChange = (ObjectChange)value; }
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", PropertyName, Value);
        }
    }
}