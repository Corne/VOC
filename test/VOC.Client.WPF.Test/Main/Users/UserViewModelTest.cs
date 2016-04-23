using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.WPF.Main.Users;
using Xunit;

namespace VOC.Client.WPF.Test.Main.Users
{
    public class UserViewModelTest
    {
        [Fact]
        public void CantBeConstructedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UserViewModel(null));
        }
    }
}
