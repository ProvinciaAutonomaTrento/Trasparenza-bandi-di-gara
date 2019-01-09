/**
 * Copyright (C) 2017 Provincia Autonoma di Trento
 *
 * This file is part of <nome applicativo>.
 * Pitre is free software: you can redistribute it and/or modify
 * it under the terms of the LGPL as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Pitre is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the LGPL v. 3
 * along with Pitre. If not, see <https://www.gnu.org/licenses/lgpl.html>.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

public partial class rappresentazionedinamica : System.Web.UI.Page
{
	public static ConnessioneSQL SQLClass = new ConnessioneSQL();
    public string formatotimestamp = "yyyy-MM-dd HH:mm:ss";
    public string formatoshowdata = "dd-MM-yyyy HH:mm:ss";
    public string formatoshowora = "HH:mm:ss";
    public string formatodata = "yyyy-MM-dd";
    //public string sqlstr = "select vpassnumero as 'n. pass', (trim(vcognome) || ' ' || trim(vnome)) as Nominativo, vpersona as 'Persona cercata', vufficio as Struttura, voraentrata as Entrata, vorauscita as Uscita from accessi ", sqlfiltro = "";
    public string sqlstr, msg = "";
    public string sqlfiltro = "", sortcriteria = "order by idvisita asc";
    //public static DataTable srcdt = new DataTable();
    //public FbDataAdapter fda;
    public SqlDataAdapter sqlda = new SqlDataAdapter();
    public SqlDataReader reader;
    public DataSet ds = new DataSet();
    public DataTable tbl = new DataTable();
    public DateTime Data = DateTime.Now;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ArrivoDa"] == null || Session["ArrivoDa"].ToString() == "") Response.Redirect("Default.aspx");
        if (!Page.IsPostBack) initChartCollection();
    }
    string titolo = "";
    protected void initChartCollection()
    {
        DateTime dada = DateTime.Now, ada = DateTime.Now;
        switch (rblist.SelectedValue)
        {
            case "Oggi": dada = DateTime.Now; ada = DateTime.Now; titolo = "Accessi di oggi"; break;
            case "Ultima settimana":
                ada = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek - 1 + 1)); dada = DateTime.Now.AddDays(-((int)DateTime.Now.DayOfWeek - 1 + 7)); titolo = "Accessi ultima settimana";
                break;
            case "Ultimo mese":
                ada = DateTime.Now; dada = DateTime.Now.AddDays(-(DateTime.Now.Day - 1)); titolo = "Accessi ultimo mese";
                break;
            case "Da inizio anno":
                ada = DateTime.Now; dada = DateTime.Now.AddDays(-(DateTime.Now.DayOfYear - 1)); titolo = "Accessi da inizio anno";
                break;
            case "Anno precedente":
                ada = new DateTime(DateTime.Now.Year - 1, 12, 31);
                dada = new DateTime(DateTime.Now.Year - 1, 1, 1);
                titolo = "Accessi anno precedente";
                break;
        }

        //string dadata = "", adata = "";
        tStato.Text = "";

        //dadata = (da == null || da.ToShortDateString() == "1900-01-01") ? DateTime.Now.ToString(formatodata) : da.ToString(formatodata);
        //adata = (a == null || a.ToShortDateString() == "1900-01-01") ? DateTime.Now.ToString(formatodata) : a.ToString(formatodata);
        sqlfiltro = " CAST (a.voraentrata as date) BETWEEN (DATE '" + dada.ToString(formatodata) + "') and ( DATE '" + ada.ToString(formatodata) + "' )";
        Giornaliera.Titles.Clear();
        switch (rblTipo.SelectedValue)
        {
            case "Numero registrazioni per SEDE":
                sqlstr = "SELECT count(a.IDVISITA) as Visite, b.SSEDE as Sede FROM ACCESSI as a left join sedi as b on a.VSEDE_EK=b.SID where " + sqlfiltro + " group by b.ssede";
                sqlstr = "SELECT count(a.IDVISITA) as Registrazioni, coalesce(sum(vgruppo),0) as accompagnati, (count(idVisita) + coalesce(sum(vgruppo),0)) as Visite , b.SSEDE as Sede FROM ACCESSI as a left join ";
                sqlstr += "sedi as b on a.VSEDE_EK = b.SID where " + sqlfiltro + " group by b.ssede";
                break;
            case "Numero registrazioni per orario":
                //sqlstr = "SELECT  count (a.IDVISITA) as Registrazioni, (count(idVisita) + sum(vgruppo)) as Visite, extract ( hour from a.VORAENTRATA ) as ore FROM ACCESSI a where " + sqlfiltro + " group by ore having extract(hour from a.VORAENTRATA ) >= 7 and extract (hour from a.VORAENTRATA ) <= 19";
                sqlstr = "SELECT  count (a.IDVISITA) as Registrazioni, (count(idVisita) + coalesce(sum(vgruppo),0)) as Visite, extract ( hour from a.VORAENTRATA ) as dalleore, ";
                sqlstr += "(extract(hour from a.voraentrata ) || ' - ' || cast(extract(hour from a.voraentrata ) + 1 as varchar(2))) as labelx FROM ACCESSI a where ";
                sqlstr += sqlfiltro + " group by dalleore having extract(hour from a.VORAENTRATA ) >= 7 and extract (hour from a.VORAENTRATA ) <= 19 ";
				//sqlstr += "order by mese";
				break;
            case "Numero registrazioni per mese":
                sqlstr = "SELECT  count (a.IDVISITA) as Registrazioni, sum(vgruppo), (count(idVisita) + coalesce(sum(vgruppo), 0)) as Visite, ";
                sqlstr += "trim(iif(EXTRACT(MONTH FROM a.VORAENTRATA) > 9, '', '0') || EXTRACT(MONTH FROM a.VORAENTRATA)) || '-' || cast(extract(year from a.VORAENTRATA) as varchar(4)) as mese ";
                sqlstr += "FROM ACCESSI a where " + sqlfiltro + " group by extract(year from a.VORAENTRATA), mese ";
				sqlstr += "order by extract(year from a.VORAENTRATA), mese ";
				break;
        }
        tTitolo.Text = "Rilevazione accessi a partire dal " + dada.ToString("dd-MM-yyyy") + " sino al " + ada.ToString("dd-MM-yyyy");
        tTitolo.Enabled = false;
        SqlCommand cmd = new SqlCommand();
        try
        {
			SQLClass.openaSQLConn(out msg);
			if (SQLClass.SQLConn.State != ConnectionState.Open)
			{
				SQLClass.closeaSQLConn(out msg);
				SQLClass.openaSQLConn(out msg);
			}
			SqlDataReader reader = cmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            tStato.Text = string.Format("ERRORE: non è possibile leggere la tabella ACCESSI E SEDI. {0}", ex.Message);
            return;
        }

		long rr = SQLClass.getSQLdata(sqlstr, "AccessiXSedi", out msg);

		//tStato.Text = string.Format("Numero sedi attive in data {0} : {1}", Data.ToShortDateString(), rr);

		if (ds.Tables["AccessiXSedi"].Rows.Count > 0 && rblTipo.SelectedValue == "Numero registrazioni per SEDE")
        {
            Giornaliera.Series.Clear();            
            Giornaliera.DataSource = ds.Tables["AccessiXSedi"];
            Giornaliera.Titles.Add(titolo).Font = new System.Drawing.Font("Thaoma", 12);
            Giornaliera.Series.Add("Serie Sedi");
            Giornaliera.Series["Serie Sedi"].BorderWidth = 1;
            Giornaliera.Series["Serie Sedi"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 7);
            Giornaliera.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.LabelStyle.TruncatedLabels = false;
            Giornaliera.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.FromName("AliceBlue");
            Giornaliera.ChartAreas["ChartArea1"].BackSecondaryColor = System.Drawing.Color.FromName("Red");
            Giornaliera.ChartAreas["ChartArea1"].BackGradientStyle = System.Web.UI.DataVisualization.Charting.GradientStyle.TopBottom;            
            Giornaliera.Series["Serie Sedi"].XValueMember = "Sede";
            Giornaliera.Series["Serie Sedi"].AxisLabel = "Sede";
            Giornaliera.Series["Serie Sedi"].Color = System.Drawing.Color.FromName("RoyalBlue");
            Giornaliera.ChartAreas["ChartArea1"].AxisY.LabelStyle.TruncatedLabels = false;
            Giornaliera.Series["Serie Sedi"].YValueMembers = "Visite";
            Giornaliera.Series["Serie Sedi"].IsValueShownAsLabel = true;
            Giornaliera.Series["Serie Sedi"].Font = new System.Drawing.Font("Thaoma", 12);
            Giornaliera.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //Giornaliera.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //Giornaliera.ChartAreas["Serie Sedi"].
            Giornaliera.ChartAreas["ChartArea1"].AxisY2.Enabled = new System.Web.UI.DataVisualization.Charting.AxisEnabled();
            //Giornaliera.Series["Serie Sedi"].Label = Giornaliera.Series["Serie Sedi"].YValueMembers;
            Giornaliera.DataBind();
        }
        if (ds.Tables["AccessiXSedi"].Rows.Count > 0 && rblTipo.SelectedValue == "Numero registrazioni per orario")
        {
            Giornaliera.Series.Clear();            
            Giornaliera.DataSource = ds.Tables["AccessiXSedi"];
            Giornaliera.Titles.Add(titolo).Font = new System.Drawing.Font("Thaoma", 12);
            Giornaliera.Series.Add("Serie Ore");
            Giornaliera.Series["Serie Ore"].BorderWidth = 1;
            Giornaliera.Series["Serie Ore"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 7);
            Giornaliera.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.LabelStyle.TruncatedLabels = false;
            Giornaliera.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.FromName("Bisque");
            Giornaliera.ChartAreas["ChartArea1"].BackSecondaryColor = System.Drawing.Color.FromName("Red");
            Giornaliera.ChartAreas["ChartArea1"].BackGradientStyle = System.Web.UI.DataVisualization.Charting.GradientStyle.TopBottom;
            Giornaliera.Series["Serie Ore"].XValueMember = "labelx";
            Giornaliera.Series["Serie Ore"].AxisLabel = "labelx";
            Giornaliera.Series["Serie Ore"].Color = System.Drawing.Color.FromName("Coral");
            Giornaliera.ChartAreas["ChartArea1"].AxisY.LabelStyle.TruncatedLabels = false;
            Giornaliera.Series["Serie Ore"].YValueMembers = "Visite";
            Giornaliera.Series["Serie Ore"].IsValueShownAsLabel = true;
            Giornaliera.Series["Serie Ore"].Font = new System.Drawing.Font("Thaoma", 12);
            Giornaliera.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //Giornaliera.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //Giornaliera.ChartAreas["Serie Sedi"].
            Giornaliera.ChartAreas["ChartArea1"].AxisY2.Enabled = new System.Web.UI.DataVisualization.Charting.AxisEnabled();
            //Giornaliera.Series["Serie Sedi"].Label = Giornaliera.Series["Serie Sedi"].YValueMembers;
            Giornaliera.DataBind();
        }
        if (ds.Tables["AccessiXSedi"].Rows.Count > 0 && rblTipo.SelectedValue == "Numero registrazioni per mese")
        {
            Giornaliera.Series.Clear();
            Giornaliera.DataSource = ds.Tables["AccessiXSedi"];
            Giornaliera.Titles.Add(titolo).Font = new System.Drawing.Font("Thaoma", 12);
            Giornaliera.Series.Add("Serie Mesi");
            Giornaliera.Series["Serie Mesi"].BorderWidth = 1;
            Giornaliera.Series["Serie Mesi"].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 7);
            Giornaliera.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;
            Giornaliera.ChartAreas["ChartArea1"].AxisX.LabelStyle.TruncatedLabels = false;
            Giornaliera.ChartAreas["ChartArea1"].BackColor = System.Drawing.Color.FromName("Azure");
            Giornaliera.ChartAreas["ChartArea1"].BackSecondaryColor = System.Drawing.Color.FromName("Red");
            Giornaliera.ChartAreas["ChartArea1"].BackGradientStyle = System.Web.UI.DataVisualization.Charting.GradientStyle.TopBottom;
            Giornaliera.Series["Serie Mesi"].XValueMember = "Mese";
            Giornaliera.Series["Serie Mesi"].AxisLabel = "Mese";
            Giornaliera.Series["Serie Mesi"].Color = System.Drawing.Color.FromName("LightGoldenrodYellow");
            Giornaliera.ChartAreas["ChartArea1"].AxisY.LabelStyle.TruncatedLabels = false;
            Giornaliera.Series["Serie Mesi"].YValueMembers = "Visite";
            Giornaliera.Series["Serie Mesi"].IsValueShownAsLabel = true;
            Giornaliera.Series["Serie Mesi"].Font = new System.Drawing.Font("Thaoma", 12);
            Giornaliera.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //Giornaliera.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            //Giornaliera.ChartAreas["Serie Sedi"].
            Giornaliera.ChartAreas["ChartArea1"].AxisY2.Enabled = new System.Web.UI.DataVisualization.Charting.AxisEnabled();
            //Giornaliera.Series["Serie Sedi"].Label = Giornaliera.Series["Serie Sedi"].YValueMembers;
            Giornaliera.DataBind();
        }
        SQLClass.closeaSQLConn(out msg);
    }

    protected void Display(System.Web.UI.DataVisualization.Charting.Chart s)
    {
        string ss = "";
        for (int i = 0; i < s.Titles.Count(); i++)
        {
            ss += s.Titles[i].Text + "; ";
        }
        tStato.Text = ss.ToString();
    }

    protected void rblist_SelectedIndexChanged(object sender, EventArgs e)
    {
        initChartCollection();
    }
    protected void rbltipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblTipo.SelectedValue == "Numero registrazioni per mese")
        {
            rblist.Items[0].Enabled = false;
            rblist.Items[1].Enabled = false;
            rblist.ClearSelection();
            rblist.Items[2].Selected = true;            
        }
        else
        {
            rblist.Items[0].Enabled = true;
            rblist.Items[1].Enabled = true;
        }
        initChartCollection();
    }

    protected void bExit_Click(object sender, EventArgs e)
    {
        Response.Redirect("Accessi.aspx");
    }
}
