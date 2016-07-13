using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VOC.Client.WPF.Converters;
using Xunit;

namespace VOC.Client.WPF.Test.Converters
{
    public class MultiplyTest
    {
        public static IEnumerable<object> ConvertInputs
        {
            get
            {
                yield return new object[] { null, null, DependencyProperty.UnsetValue };
                yield return new object[] { 5, null, DependencyProperty.UnsetValue };
                yield return new object[] { null, 10, DependencyProperty.UnsetValue };
                yield return new object[] { 5, 10, 50.0 };
                yield return new object[] { "11.1", 7, 77.7 };
                yield return new object[] { 8, "5", 40 };
            }
        }

        [Theory, MemberData(nameof(ConvertInputs))]
        public void TestConvert(object value, object param, object expected)
        {
            var multiply = new Multiply();
            object result = multiply.Convert(value, null, param, null);
            Assert.Equal(expected, result);
        }
    }
}
