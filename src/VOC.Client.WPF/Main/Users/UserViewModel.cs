using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using VOC.Client.Users;

namespace VOC.Client.WPF.Main.Users
{
    public class UserViewModel : ViewModelBase
    {
        private readonly IUser user;

        public UserViewModel(IUser user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            this.user = user;

            EditNameCommand = new RelayCommand(
                () => { InEditMode = true; EditValue = Name; });
            SaveNameCommand = new RelayCommand(
                () => { InEditMode = false; this.user.Name = EditValue; }, 
                () => !string.IsNullOrWhiteSpace(EditValue));
            CancelEditCommand = new RelayCommand(() => InEditMode = false);
        }

        public string Name { get { return user.Name; } }

        private bool _inEditMode;
        public bool InEditMode
        {
            get { return _inEditMode; }
            set { Set(ref _inEditMode, value); }
        }

        private string _editValue;
        public string EditValue
        {
            get { return _editValue; }
            set
            {
                if (Set(ref _editValue, value))
                    SaveNameCommand.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand EditNameCommand { get; }
        public RelayCommand SaveNameCommand { get; }
        public RelayCommand CancelEditCommand { get; }
    }
}
