using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Linq;

namespace ListFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    class File2
    {

        private string path;

        public string filePath
        {
            get
            {
                return this.path;
            }
        }

        public File2(string filePath)
        {
            this.path = filePath;
        }
    }

    public partial class MainWindow : Window
    {
        //public MainWindow()
        //{
        //    InitializeComponent();
        //}

        List<string> listAllFilesInDir(string dir)
        {
            List<string> listToReturn = new List<string>();
            foreach (string file in Directory.GetFiles(dir))
            {
                listToReturn.Add(file);
            }
            foreach (string folder in Directory.GetDirectories(dir))
            {
                listToReturn.AddRange(listAllFilesInDir(folder));
            }
            return listToReturn;
        }

        List<string> listDuplicatingFiles(string dir)
        {
            List<string> listToReturn = new List<string>();
            listToReturn = listAllFilesInDir(dir);
            List<File2> allFiles = new List<File2>();
            foreach (string singleFile in listToReturn)
            {
                File2 newFile = new File2(singleFile);
                allFiles.Add(newFile);
            }

            for (int i = 0; i < allFiles.Count; i++)
            {
                for (int j = i + 1; j < allFiles.Count; j++)
                {
                    byte[] firstFile = System.IO.File.ReadAllBytes(allFiles[i].filePath);
                    byte[] secondFile = System.IO.File.ReadAllBytes(allFiles[j].filePath);

                    if (firstFile.SequenceEqual(secondFile))
                    {
                        allFiles.RemoveAt(j);
                    }
                }
            }

            listToReturn.Clear();

            foreach (File2 singleFile in allFiles)
            {
                listToReturn.Add(singleFile.filePath);
            }

            return listToReturn;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<string> individualFiles = listDuplicatingFiles(folderDialog.SelectedPath);
                StringBuilder csvContent = new StringBuilder();
                string csvPath = folderDialog.SelectedPath + "\\Arquivos Listados.csv";

                foreach (string singleFilePath in individualFiles)
                {
                    string onlyFileName;
                    onlyFileName = singleFilePath.Substring(singleFilePath.LastIndexOf(@"\") + 1);

                    csvContent.AppendLine(onlyFileName);

                    filesListBox.Items.Add(onlyFileName);
                }

                File.AppendAllText(csvPath, csvContent.ToString());
                this.Close();
            }
        }
    }
}
