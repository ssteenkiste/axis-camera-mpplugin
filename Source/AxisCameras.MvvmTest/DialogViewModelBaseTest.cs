using AxisCameras.Mvvm;
using NUnit.Framework;

namespace AxisCameras.MvvmTest
{
    [TestFixture]
    public class DialogViewModelBaseTest
    {
        [Test]
        public void DialogResultCommandIsNull()
        {
            // ARRANGE
            var dialogViewModel = new DialogViewModel(true, true)
            {
                DialogResultCommand = null
            };

            // ACT
            dialogViewModel.OkCommand.Execute(null);

            // ASSERT
            Assert.That(dialogViewModel.ValidateCalled, Is.True);
            Assert.That(dialogViewModel.OnOkCalled, Is.True);
        }

        [TestCase(true, true, true, true, true)]
        [TestCase(true, false, false, true, true)]
        [TestCase(false, true, false, true, false)]
        [TestCase(false, false, false, true, false)]
        public void OkCommand(
            bool isValid,
            bool isOk,
            bool expectedDialogResultCalled,
            bool expectedValidateCalled,
            bool expectedOnOkCalled)
        {
            // ARRANGE
            bool dialogResultCommandCalled = false;

            var dialogViewModel = new DialogViewModel(isOk, isValid)
            {
                DialogResultCommand = new RelayCommand(_ => dialogResultCommandCalled = true)
            };

            // ACT
            dialogViewModel.OkCommand.Execute(null);

            // ASSERT
            Assert.That(dialogResultCommandCalled, Is.EqualTo(expectedDialogResultCalled));
            Assert.That(dialogViewModel.ValidateCalled, Is.EqualTo(expectedValidateCalled));
            Assert.That(dialogViewModel.OnOkCalled, Is.EqualTo(expectedOnOkCalled));
        }

        #region Helper classes

        private class DialogViewModel : DialogViewModelBase
        {
            private readonly bool isOk;
            private readonly bool isValid;

            public DialogViewModel(bool isOk, bool isValid)
            {
                this.isOk = isOk;
                this.isValid = isValid;
            }

            public bool OnOkCalled { get; private set; }

            public bool ValidateCalled { get; private set; }

            protected override bool OnOk()
            {
                OnOkCalled = true;
                return isOk;
            }

            public override bool Validate()
            {
                ValidateCalled = true;
                return isValid;
            }
        }

        #endregion
    }
}