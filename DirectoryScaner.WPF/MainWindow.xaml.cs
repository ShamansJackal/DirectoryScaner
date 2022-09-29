using DirectoryScanner.Core;
using DirectoryScanner.Core.Struct;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DirectoryScaner.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Scanner scanner = new Scanner(4, @"D:\Учеба\5 сем\GBA");
            treeView1.ItemsSource = scanner.Root.Childs;

            Thread thread = new Thread(x => scanner.StartProcess());
            thread.Start();
        }

        private void InputFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Multiselect = false,
                Filter = "Files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    ((Button)sender).Tag = filename;
                }
            }
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
