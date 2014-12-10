using AxisCameras.Mvvm;
using AxisCameras.Mvvm.Validation;
using NUnit.Framework;

namespace AxisCameras.MvvmTest
{
    [TestFixture]
    public class ViewModelBaseTest
    {
        [Test]
        public void AddMoreTests()
        {
            // ARRANGE

            // ACT

            // ASSERT
            Assert.Fail();
        }

        #region Helper classes and methods

        private class ViewModel : ViewModelBase
        {
            public ViewModel()
            {
                AddValidator(() => Username, new NotEmptyOrNullValidationRule());
            }

            public string Username
            {
                get { return GetValue<string>(); }
                set { SetValue(value); }
            }
        }

        private class NotEmptyOrNullValidationRule : IValidationRule
        {
            public string ErrorMessage
            {
                get { return "Validation failed!"; }
            }

            public bool Validate(object value)
            {
                return !string.IsNullOrEmpty(value as string);
            }
        }

        #endregion
    }
}