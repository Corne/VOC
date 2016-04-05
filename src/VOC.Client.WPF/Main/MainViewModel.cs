using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace VOC.Client.WPF.Main
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel()
        {

        }

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
