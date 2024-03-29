﻿using Microsoft.Reporting.WebForms;
using MySql.Data.MySqlClient;
using Report.Dataset;
using Report.Models;
using Report.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Report.Operation
{
    public partial class LateReport1 : System.Web.UI.Page
    {
        private DBConnect db = new DBConnect();
        public static string systemDateStr;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                DataHelper.checkLoginSession();
                systemDateStr = DataHelper.getSystemDate().ToString("dd/MM/yyyy");
                DataHelper.populateBranchDDL(ddBranchName, DataHelper.getUserId());

            }

        }
       

       

        private void GenerateReport(DataTable AccountReceivableAgingReportDT)
        {
            ReportParameterCollection reportParameters = new ReportParameterCollection();
            reportParameters.Add(new ReportParameter("Branch", ddBranchName.SelectedItem.Text));
         
            var lateDS = new ReportDataSource("AccountReceivableAgingReportDS", AccountReceivableAgingReportDT);

            DataHelper.generateOperationReport(ReportViewer1, "AccountReceivableAgingReport", reportParameters, lateDS);

        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            var dayFormat = DataHelper.getSystemDate().ToString("yyyy-MM-dd");

            var ar = "ps_AccountReceivableAgingReport";

            List<Procedure> parameters = new List<Procedure>();
            parameters.Add(item: new Procedure() { field_name = "@pBranch", sql_db_type = MySqlDbType.VarChar, value_name = ddBranchName.SelectedItem.Value });

            DataTable lateReportDT = db.getProcedureDataTable(ar, parameters);

            GenerateReport(lateReportDT);
        }
    }
}