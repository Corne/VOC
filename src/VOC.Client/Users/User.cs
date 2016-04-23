using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Users
{
    public class User : IUser
    {

        public User(string name)
        {

            Name = name;
        }

        public Guid Id { get; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(Name), "Name can't be null or white space");
                _name = value;
            }
        }

    }
}
