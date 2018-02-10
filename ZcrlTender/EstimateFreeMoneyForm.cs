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
    public partial class EstimateFreeMoneyForm : Form
    {

        public EstimateFreeMoneyForm(int estimateId)
        {
            InitializeComponent();
            kekvRemainsTable.AutoGenerateColumns = false;

            using (TenderContext tc = new TenderContext())
            {
                Estimate est = tc.Estimates.Where(p => p.Id == estimateId).FirstOrDefault();

                estimateNameLabel.Text = est.Name;
                this.Text += " на " + est.Year.Year + " рік";

                var groupedTenderRecordsByKekv =    (from rec in tc.TenderPlanRecords.ToList()
                                                     where rec.EstimateId == est.Id
                                                     group rec by rec.PrimaryKekv into g1
                                                     select new KekvMoneyRecord
                                                     {
                                                         Kekv = g1.Key, Sum = g1.Sum(p => p.UsedByRecordSum)
                                                     });
                var groupedEstimatedMoneysByKekv = (from rec in tc.BalanceChanges
                                                    where (((rec.PrimaryKekvSum > 0) || (rec.PlannedSpendingId != null)) 
                                                                && (rec.EstimateId == est.Id))
                                                    group rec by rec.PrimaryKekv into g1
                                                    select new KekvMoneyRecord
                                                    {
                                                        Kekv = g1.Key, Sum = g1.Sum(p => p.PrimaryKekvSum)
                                                    });

                kekvRemainsTable.DataSource = (from rec1 in groupedEstimatedMoneysByKekv.ToList()
                                               join rec2 in groupedTenderRecordsByKekv.ToList() on rec1.Kekv equals rec2.Kekv into j1
                                               from def in j1.DefaultIfEmpty(new KekvMoneyRecord { Sum = 0 })
                                               //where rec1.Sum > def.Sum
                                               select new KekvMoneyRecord
                                               {
                                                   Kekv = rec1.Kekv, Sum = rec1.Sum - def.Sum
                                               }).ToList();
            }
        }
    }
}
