using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.WPF.Main
{
    public interface IContentViewModel
    {

        /// <summary>
        /// Will be called when navigated to this ContentViewModel
        /// You can load necessary data during this period
        /// If its a long running operation, it's good to set an active LoadMessage
        /// </summary>
        /// <returns>Task that will complete when navigation is completed</returns>
        Task OnNavigate();

        /// <summary>
        /// Will be called when this ContentViewModel will be closed.
        /// You can clean up some objects / set tasks inactive on this call,
        /// because the viewmodel will be kept alive on the navigationstack
        /// </summary>
        /// <returns></returns>
        Task OnClose();
    }
}
