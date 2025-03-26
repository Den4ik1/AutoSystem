using AutoSystem.Models;
using System.Collections.ObjectModel;

namespace AutoSystem.Contexts
{
    public class WindowDataContext
    {
        public ObservableCollection<ModeDataModel> Modes { get; set; }
        public ObservableCollection<StepDataModel> Steps { get; set; }
    }
}
