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
    public partial class ContractsOnDkCodeForm : Form
    {
        public ContractsOnDkCodeForm(TenderPlanRecord record, bool isNewSystemSelected)
        {
            InitializeComponent();
            contractsTable.AutoGenerateColumns = false;
            contractsTable.RowsAdded += (sender, e) => DataGridViewHelper.CalculateNewRowNumber(contractsTable, 0, e.RowIndex, e.RowCount);

            using (TenderContext tc = new TenderContext())
            {
                tc.TenderPlanRecords.Attach(record);
                estimateNameLabel.Text = record.Estimate.Name;
                dkCodeLabel.Text = record.Dk.FullName;

                List<ContractsTableEntry> tableEntries = new List<ContractsTableEntry>();

                if(isNewSystemSelected)
                {
                    kekvLabel.Text = record.PrimaryKekv.Code;
                    tableEntries = (from rec in tc.Contracts.ToList()
                                    where (rec.EstimateId == record.EstimateId) &&
                                          (rec.DkCodeId == record.DkCodeId) &&
                                          (rec.PrimaryKekvId == record.PrimaryKekvId)
                                    orderby rec.SignDate descending
                                    select new ContractsTableEntry 
                                    {
                                        ContractDate = rec.SignDate,
                                        ContractNum = rec.Number,
                                        Contractor = rec.Contractor,
                                        Description = rec.Description,
                                        FullSum = rec.Sum,
                                        RelatedContract = rec,
                                        UsedSum = rec.UsedMoney
                                    }).ToList();
                }
                else
                {
                    kekvLabel.Text = record.SecondaryKekv.Code + "(по старой системе)";
                    tableEntries = (from rec in tc.Contracts
                                    where (rec.EstimateId == record.EstimateId) &&
                                          (rec.DkCodeId == record.DkCodeId) &&
                                          (rec.SecondaryKekvId == record.SecondaryKekvId)
                                    orderby rec.SignDate descending
                                    select new ContractsTableEntry
                                    {
                                        ContractDate = rec.SignDate,
                                        ContractNum = rec.Number,
                                        Contractor = rec.Contractor,
                                        Description = rec.Description,
                                        FullSum = rec.Sum,
                                        RelatedContract = rec,
                                        UsedSum = rec.UsedMoney
                                    }).ToList();

                }

                contractsTable.DataSource = tableEntries;
            }
        }
    }
}
