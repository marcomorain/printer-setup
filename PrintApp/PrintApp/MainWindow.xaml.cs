using System;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Drawing.Printing;

namespace PrintApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                Console.WriteLine(string.Format("Printer: {0}", printer));
            }
            _file = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();

            Console.WriteLine(_file);
        }

        private StreamReader _stream;
        private Font _font;
        private readonly string _file;

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               _stream  = new StreamReader(_file);
                try
                {
                    _font = new Font("Arial", 10);
                    PrintDocument pd = new PrintDocument();
                    Console.WriteLine(pd.PrinterSettings.PrinterName);
                    pd.PrintPage += new PrintPageEventHandler(this.PrintPage);
                    pd.Print();
                }
                finally
                {
                    _stream.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            var count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // Calculate the number of lines per page.
           float linesPerPage = ev.MarginBounds.Height / _font.GetHeight(ev.Graphics);

            // Print each line of the file.
            while (count < linesPerPage &&
               ((line = _stream.ReadLine()) != null))
            {
                var yPos = topMargin + (count * _font.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, _font, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            ev.HasMorePages = line != null;
        }
    }
}
