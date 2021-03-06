﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Users;
using Xunit;

namespace VOC.Client.Test.Users
{
    public class UserTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void CantConstructWithoutName(string name)
        {
            Assert.Throws<ArgumentNullException>(() => new User(name));
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void SetNameShouldBeValidInput(string name)
        {
            var user = new User("Henk");

            Assert.Throws<ArgumentNullException>(() => user.Name = name);
            Assert.Equal("Henk", user.Name);
        }
    }
}
