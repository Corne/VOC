using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VOC.Client.WPF.Game.Board;
using Xunit;

namespace VOC.Client.WPF.Test.Game.Board
{
    public class YPositionerTest
    {
        public static IEnumerable<object> ConvertInputs
        {
            get
            {
                yield return new object[] { null, null, null, DependencyProperty.UnsetValue };
                yield return new object[] { 5, null, null, DependencyProperty.UnsetValue };
                yield return new object[] { null, 4, null, DependencyProperty.UnsetValue };
                yield return new object[] { null, null, 10, DependencyProperty.UnsetValue };
                yield return new object[] { 0, 0, 10, 0.0 };
                yield return new object[] { 0, 1, 10, 10.0 };
                yield return new object[] { 0, 2, 10, 20.0 };

                yield return new object[] { 1, 0, 10, 5.0 };
                yield return new object[] { 1, 1, 10, 15.0 };
                yield return new object[] { 1, 2, 10, 25.0 };

                yield return new object[] { "2", 0, 10, 0.0 };
                yield return new object[] { 2, "1", 10, 10.0 };
                yield return new object[] { 2, 2, "10", 20.0 };
            }
        }

        [Theory, MemberData(nameof(ConvertInputs))]
        public void TestConvert(object x, object y, object param, object expected)
        {
            var multiply = new YPositioner();
            object result = multiply.Convert(new[] { x, y }, null, param, null);
            Assert.Equal(expected, result);
        }

    }
}
