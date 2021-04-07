using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TreeViewWPF
{
    class Node
    {
        private string name;
        public ObservableCollection<Node> Nodes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
