using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using VOC.Client.WPF.Main.Users;

namespace VOC.Client.WPF.Main
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel(UserViewModel userViewModel)
        {
            if (userViewModel == null)
                throw new ArgumentNullException(nameof(userViewModel));

            User = userViewModel;
        }

        public UserViewModel User { get; }

        private IContentViewModel _content;
        public IContentViewModel Content
        {
            get { return _content; }
        }

        public async Task Update(IContentViewModel value)
        {
            var current = _content;
            if (Set(ref _content, value, nameof(Content)))
            {
                if (current != null)
                {
                    await current.OnClose();
                }
                await _content.OnNavigate();
            }
        }

    }
}
