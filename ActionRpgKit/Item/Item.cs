using System;

namespace ActionRpgKit.Item
{
    public interface IItem
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description{ get; set; }
    }

    public abstract class BaseItem : IItem
    {
        private int _id;
        private string _name;
        private string _description;

        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }
    }
}
