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
using System.Data.Entity;

namespace ZcrlTender
{
    public partial class TenderPlanRecordSelectForm : Form
    {
        public TenderPlanRecord SelectedRecord { get; set; }

        private class TPEntry
        {
            public KekvCode Kekv { get; set; }
            public DkCode Dk { get; set; }
            public decimal PlannedMoney { get; set; }
            public decimal MoneyRemain { get; set; }
            public string Commentary { get; set; }
            public TenderPlanRecord Record { get; set; }
        }

        private class FilterArgument
        {
            public Estimate Estimate { get; set; }
            public KekvCode Kekv { get; set; }
            public bool IsNewSystem { get; set; }
        }

        private BindingList<TPEntry> recordsList;

        public TenderPlanRecordSelectForm()
        {
            InitializeComponent();
            using(TenderContext tc = new TenderContext())
            {
                tpEstimateCBList.DataSource = tc.Estimates.ToList();
                tpEstimateCBList.DisplayMember = "Name";
                tpEstimateCBList.ValueMember = "Id";

                List<KekvCode> kekvs = tc.KekvCodes.OrderBy(p => p.Code).ToList();
                kekvs.Insert(0, new KekvCode { Code = "- ВСІ -", Id = -1 });
                tpKekvsCBList.DataSource = kekvs;
                tpKekvsCBList.DisplayMember = "Code";
                tpKekvsCBList.ValueMember = "Id";

                tenderPlanTable.AutoGenerateColumns = false;

                LoadRecords();

                tpEstimateCBList.SelectedIndexChanged += (sender, e) => LoadRecords();
                tpKekvsCBList.SelectedIndexChanged += (sender, e) => LoadRecords();
                tpNewSystemRButton.CheckedChanged += (sender, e) => LoadRecords();
                tpAltSystemRButton.CheckedChanged += (sender, e) => LoadRecords();

                button1.Click += (sender, e) => SelectRecord();
                tenderPlanTable.CellDoubleClick += (sender, e) => SelectRecord();
            }
        }

        private void LoadRecords()
        {
            if(!recordsLoader.IsBusy)
            {
                FilterArgument arg = new FilterArgument();
                arg.IsNewSystem = tpNewSystemRButton.Checked;
                arg.Kekv = tpKekvsCBList.SelectedItem as KekvCode;
                arg.Estimate = tpEstimateCBList.SelectedItem as Estimate;
                recordsLoader.RunWorkerAsync(arg);
                ToggleLoadingAnimation();
            }
        }

        private void ToggleLoadingAnimation()
        {
            bool currentLoadingAnimation = yearPlanLoadingPicture.Visible;

            yearPlanLoadingPicture.Visible = yearPlanLoadingLabel.Visible = !currentLoadingAnimation;
            groupBox3.Enabled = currentLoadingAnimation;
        }

        private void recordsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            using(TenderContext tc = new TenderContext())
            {
                FilterArgument selectedEstimate = e.Argument as FilterArgument;

                var result = (from rec in tc.TenderPlanRecords.ToList()
                              where ((rec.EstimateId == selectedEstimate.Estimate.Id) && !rec.IsTenderComplete)
                              orderby rec.Dk.Code ascending
                              select new TPEntry
                              {
                                  Kekv = (selectedEstimate.IsNewSystem) ? rec.PrimaryKekv : rec.SecondaryKekv,
                                  Dk = rec.Dk,
                                  PlannedMoney = rec.PlannedSum,
                                  MoneyRemain = rec.AvailableForContractsMoney,
                                  Commentary = rec.CodeRepeatReason,
                                  Record = rec
                              } into s1 
                              where s1.MoneyRemain > 0
                              select s1);
                
                if(selectedEstimate.Kekv.Id < 0)
                {
                    recordsList = new BindingList<TPEntry>(result.ToList());
                }
                else
                {
                    recordsList = new BindingList<TPEntry>(result.Where(p => p.Kekv.Id == selectedEstimate.Kekv.Id).ToList());
                }
            }
        }

        private void recordsLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tenderPlanTable.DataSource = recordsList;
            ToggleLoadingAnimation();
            tenderPlanTable.Refresh();
        }

        private void SelectRecord()
        {
            SelectedRecord = (tenderPlanTable.SelectedRows[0].DataBoundItem as TPEntry).Record;
            
            using(TenderContext tc = new TenderContext())
            {
                tc.TenderPlanRecords.Attach(SelectedRecord);
                tc.Entry<TenderPlanRecord>(SelectedRecord).Collection(p => p.RegisteredContracts).Load();
                tc.Entry<TenderPlanRecord>(SelectedRecord).Collection(p => p.RegisteredContracts).Query().Include(p => p.Invoices).Load();

                tc.Entry<TenderPlanRecord>(SelectedRecord).Reference(p => p.Estimate).Load();
                tc.Entry<TenderPlanRecord>(SelectedRecord).Reference(p => p.Estimate).Query().Include(p => p.Changes).Load();
            }
            Close();
        }

        private void tenderPlanTable_SelectionChanged(object sender, EventArgs e)
        {
            button1.Enabled = tenderPlanTable.SelectedRows.Count > 0;
        }

    }
}
