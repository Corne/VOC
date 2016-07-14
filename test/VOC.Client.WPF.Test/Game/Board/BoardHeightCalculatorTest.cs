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
    public class BoardHeightCalculatorTest
    {
        public static IEnumerable<object[]> ConvertInputs
        {
            get
            {
                yield return new object[] { null, null, DependencyProperty.UnsetValue };
                yield return new object[] { 1, null, DependencyProperty.UnsetValue };
                yield return new object[] { null, 5, DependencyProperty.UnsetValue };
                yield return new object[] { 0, 0, 0.0 };
                yield return new object[] { 1,1, 1.5 };
                yield return new object[] { 5, 4, 22.0 };
            }
        }

        [Theory, MemberData(nameof(ConvertInputs))]
        public void TestConvert(object value, object param, object expected)
        {
            var converter = new BoardHeightCalculator();
            object result = converter.Convert(value, null, param, null);
            Assert.Equal(expected, result);
        }
    }
}
