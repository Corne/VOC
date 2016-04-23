using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Users;

namespace VOC.Client.WPF.Main.Users
{
    public class UserViewModel
    {
        private readonly IUser user;

        public UserViewModel(IUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            this.user = user;
        }

        public string Name { get { return user.Name; } }
    }
}
