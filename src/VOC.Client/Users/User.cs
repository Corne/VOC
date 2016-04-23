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
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name), "Name can't be null or white space");
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; private set; } 
    }
}
