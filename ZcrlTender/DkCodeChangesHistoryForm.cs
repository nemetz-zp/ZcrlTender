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
    public partial class DkCodeChangesHistoryForm : Form
    {
        public DkCodeChangesHistoryForm(TenderPlanRecord record, bool isNewSystemSelected)
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;

            using(TenderContext tc = new TenderContext())
            {
                tc.TenderPlanRecords.Attach(record);

                estimateNameLabel.Text = record.Estimate.Name;
                dkCodeLabel.Text = record.Dk.FullName;

                List<TenderPlanItemChangeEntry> changesList = new List<TenderPlanItemChangeEntry>();

                if(isNewSystemSelected)
                {
                    kekvLabel.Text = record.PrimaryKekv.Code;
                    changesList = (from rec in tc.TenderPlanRecordChanges.ToList()
                                   where (rec.TenderPlanRecordId == record.Id)
                                   orderby rec.DateOfChange descending
                                   select new TenderPlanItemChangeEntry
                                   {
                                       DateOfChange = rec.DateOfChange,
                                       Description = rec.Description,
                                       DkCode = rec.Record.Dk.FullName,
                                       Kekv = rec.Record.PrimaryKekv.Code,
                                       SumChange = rec.ChangeOfSum
                                   }).ToList();
                }
                else
                {
                    kekvLabel.Text = record.SecondaryKekv.Code + "(за старою системою)";
                    changesList = (from rec in tc.TenderPlanRecordChanges.ToList()
                                   where (rec.TenderPlanRecordId == record.Id)
                                   orderby rec.DateOfChange descending
                                   select new TenderPlanItemChangeEntry
                                   {
                                       DateOfChange = rec.DateOfChange,
                                       Description = rec.Description,
                                       DkCode = rec.Record.Dk.FullName,
                                       Kekv = rec.Record.SecondaryKekv.Code,
                                       SumChange = rec.ChangeOfSum
                                   }).ToList();
                }

                dataGridView1.DataSource = changesList;
            }
        }
    }
}
