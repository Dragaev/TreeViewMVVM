using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TreeViewWPF
{
    class AppViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Node> Nodes { get; set; }
        bool Expanded = false;
        int initialNodesQuantity;
        int nodesCounter = 1;
        List<int> itemNumbers = new List<int>();
        TreeView treeView = ((MainWindow)System.Windows.Application.Current.MainWindow).treeView1;
        TextBox filterTB = ((MainWindow)System.Windows.Application.Current.MainWindow).FilterBox;

         private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        initialNodesQuantity += 25;
                        MakeTree(initialNodesQuantity);
                    }));
            }
        }

        private RelayCommand findCommand;
        public RelayCommand FindCommand
        {
            get
            {
                return findCommand ??
                    (findCommand = new RelayCommand(obj =>
                    {
                            SearchNode(Nodes, filterTB.Text);
                            ExpandAllNodes();
                        if (filterTB.Text != "")
                        {
                            changeColor();
                        }
                    }));
            }
        }
        public AppViewModel()
        {
            Nodes = new ObservableCollection<Node>();
        }
        void MakeTree(int countOfFirstLevelNodes)
        {
            Random rand = new Random();
            for (int i = nodesCounter; i <= countOfFirstLevelNodes; i++)
            {
                Nodes.Add(
                    new Node
                    {
                        Name = "Category" + i.ToString(),
                        Nodes = new ObservableCollection<Node>(MakeSecondLevelNodesList(rand.Next(1, 10)))
                    }); ;
                nodesCounter++;
                itemNumbers = new List<int>();
            }

        }
        List<Node> MakeSecondLevelNodesList(int nodesQuantity)
        {
            List<Node> nodeList = new List<Node>();
            foreach (var element in GenerateItemNumbers(nodesQuantity))
            {
                nodeList.Add(new Node { Name = "Item" + element.ToString() });
            }
            return nodeList;
        }
         List<int> GenerateItemNumbers(int itemsQuantity)
        {
            int newNumber;
            while(itemNumbers.Count<itemsQuantity)
            {
                Random rnd = new Random();
                newNumber = rnd.Next(1, 30);
                if (!itemNumbers.Contains(newNumber))
                    itemNumbers.Add(newNumber);
            }
            return itemNumbers;
        }
    
        private void SearchNode(ObservableCollection<Node> allNodes, string searchableNode)
        {
            allNodes = Nodes;
            ObservableCollection<Node> changedLevelNodes = new ObservableCollection<Node>();
            if (searchableNode == "") 
            { 
                changedLevelNodes = allNodes; 
            }
            else
            {
                foreach (var category in allNodes)
                {
                    if (category.Name.IndexOf(searchableNode, StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        changedLevelNodes.Add(category);
                    }
                    foreach (var item in category.Nodes)
                    {
                        if (item.Name.IndexOf(searchableNode, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            if(!changedLevelNodes.Contains(category))
                            changedLevelNodes.Add(category);
                        }
                    }
                }
            }
           treeView.ItemsSource = changedLevelNodes;
        }

        void changeColor()
        {
            foreach (object item in treeView.Items)
            {             
                var tvItem = treeView.ItemContainerGenerator.ContainerFromItem(item)
                             as TreeViewItem;
                if (tvItem != null) tvItem.Background = Brushes.Green;
            }
        }
        void ExpandAllNodes()
        {
            if (filterTB.Text!= "")
                Expanded = true;
            else Expanded = false;
            Style Style = new Style
            {
                TargetType = typeof(TreeViewItem)
            };
            Style.Setters.Add(new Setter(TreeViewItem.IsExpandedProperty, Expanded));
            treeView.ItemContainerStyle = Style;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

}
