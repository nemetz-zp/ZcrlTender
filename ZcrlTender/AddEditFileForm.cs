using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TenderLibrary;
using ZcrlTender.ViewModels;

namespace ZcrlTender
{
    public partial class AddEditFileForm : Form
    {
        private UploadedFile createdFile;

        public UploadedFile CreatedFile 
        { 
            get 
            {
                return createdFile;
            } 
        }

        public AddEditFileForm()
        {
            InitializeComponent();
        }

        public AddEditFileForm(UploadedFile file)
        {
            InitializeComponent();
            this.createdFile = file;
            textBox1.Text = file.PublicName;
            textBox2.Text = file.PhisicalName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Multiselect = false;
            of.ShowDialog();

            if(!string.IsNullOrWhiteSpace(of.FileName))
            {
                textBox2.Text = of.FileName;

                if(createdFile != null)
                {
                    createdFile.UploadDate = DateTime.Now;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox1.Text))
            {
                NotificationHelper.ShowError("Ви не вказали публічне ім'я файлу!");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                NotificationHelper.ShowError("Ви не вибрали файл для завантаження!");
                return;
            }

            if(createdFile == null)
            {
                createdFile = new UploadedFile();
                createdFile.UploadDate = DateTime.Now;
            }

            createdFile.PublicName = textBox1.Text.Trim();
            createdFile.PublicName = FileManager.ClearIllegalFileNameSymbols(createdFile.PublicName);
            createdFile.PhisicalName = textBox2.Text;

            Close();
        }

        private void button1_Click(object sender, MouseEventArgs e)
        {
            button1_Click(this, new EventArgs());
        }
    }
}
