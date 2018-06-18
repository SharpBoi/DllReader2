using DllReader2.AssemblyData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DllReader2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logger log;

        private AssembliesVisualizer visual;
        private AssemblyReader dllReader = new AssemblyReader();
        private List<AssemblyDllData> dlls = new List<AssemblyDllData>();

        public MainWindow()
        {
            InitializeComponent();

            log = new Logger(textLog);

            visual = new AssembliesVisualizer(treeDllsList, treeDllContent);
        }
        
        private void btnLoadAssemblyFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                visual.Clear();

                List<Exception> excs = new List<Exception>();
                dlls.Clear();
                dlls = dllReader.ReadDllFolder(ofd.SelectedPath, excs);

                visual.Clear();
                visual.ShowAssemblies(dlls);

                log.Log(excs);
                if (dlls.Count != 0)
                    log.Log("Files loaded ", dlls.Count + " dlls; ");
            }
        }
        private void btnLoadAssembly_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Filter = "Dll |*.dll";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<Exception> excs = new List<Exception>();
                dlls = dllReader.ReadDlls(ofd.FileNames, excs);
                visual.ShowAssemblies(dlls);

                log.Log(excs);
                if (dlls.Count != 0)
                    log.Log("Files loaded ", dlls.Count + " dlls; ");
            }
        }
        private void btnClearDllList_Click(object sender, RoutedEventArgs e)
        {
            dlls.Clear();
            visual.Clear();
            GC.Collect();
        }

        private void radioShowModeChange(object sender, RoutedEventArgs e)
        {
            if (visual != null)
                if (radioNamesOnly.IsChecked.Value)
                {
                    visual.ContentDescriptionMode = DescriptionMode.Compact;
                }
                else if (radioFullDescr.IsChecked.Value)
                {
                    visual.ContentDescriptionMode = DescriptionMode.Full;
                }
        }

        private void btnClearLog_Click(object sender, RoutedEventArgs e)
        {
            log.Clear();
        }
    }
}
