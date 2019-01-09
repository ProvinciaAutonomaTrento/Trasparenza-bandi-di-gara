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
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

public partial class registrati : System.Web.UI.Page
{
	private ConnessioneSQL SQLClass = new ConnessioneSQL();
	private user utenti = new user();
	private DataSet ds = new DataSet();
	private DataTable tbl = null;
	public string test = "NO";

    protected void Page_Load(object sender, EventArgs e)
    {
        lAsterisconn.Text = ""; lAsteriscopwd.Text = "";
		string msg = "";
		SQLClass.openaSQLConn(out msg);
		tbl = SQLClass.getfromDSet("select * from manutenzione where attivo='1' and pagina='logoff'", "manutenzione", out msg);
		
		if ( tbl.Rows.Count > 0 && test != "TEST")
		{
			Stato(tbl.Rows[0]["Descrizione"].ToString(), Color.Red);
			nikname.Enabled = false;
			password.Enabled = false;
			cbpwddimenticata.Enabled = false;
			cbRegistrati.Enabled = false;
			cbAccedi.Enabled = false;
			SQLClass.closeaSQLConn(out msg);
			return;
		}
		cbAccedi.Focus();
    }

	protected void Stato(string s, Color c)
	{
		tStato.Text = s;
		if (c == null) c = Color.Black;
		tStato.ForeColor = c;
	}
	protected void cbpwddimenticata_Click1(object sender, EventArgs e)
    {
        string nn = Server.HtmlDecode(nikname.Text).ToString().Trim();
        utenti.nikname = nn;
        string msg;
		/*if (utenti.nikname.Trim().Length < 5)
        {
            tStato.Text = "Indicare lo Username prima di richiedere l'invio della password!";
            return;
        }*/
		utenti.nikname = Server.HtmlDecode(nikname.Text);
		msg = "";
        bool ok = utenti.dimenticatolapassword(nn, out msg);
		if (!utenti.abilitato)
			msg = "Utente non ancora abilitato! Attendere l'abilitazione prima di tentare l'accesso!";
		utenti.clearuser();
        ShowPopUpMsg(msg);        
    }

    protected void cbAccedi_Click(object sender, EventArgs e)
    {
        string msg;
        utenti.nikname = Server.HtmlDecode(nikname.Text);
        utenti.password = Server.HtmlDecode(password.Text);
        if (!utenti.cercanikname(utenti.nikname, utenti.password))
        {
            tStato.Text = "Username o password non trovati!";
            lAsterisconn.Enabled = true;
            lAsterisconn.Text = "*";
            lAsterisconn.Enabled = false;
            lAsteriscopwd.Enabled = true;
            lAsteriscopwd.Text = "*";
            lAsteriscopwd.Enabled = false;
            Console.Beep();
            utenti.clearuser();
        }
        else
        {
			// ok, utente ha fatto l'accesso
			Session["iduser"] = utenti.iduser;  // devo registrarlo anche se non è un accesso definitivo
			Session.Add("assistenza", "0461-496456");
            if (utenti.forzocambiopassword)
            {
				if (!utenti.abilitato)
				{
					tStato.Text = "Utenete nonancora abilitato. Prego contattare l'assistenza al n. " + Session["assistenza"].ToString();
					Session["arrivo_da"] = "login";
					// devo registrare last access
					msg = utenti.registraDTLA("Login utente", utenti.iduser);
					return;
				}
				tStato.Text = string.Format("Benvenuto {0} {1}. La tua password deve essere cambiata!", utenti.nome, utenti.cognome);
                Session["arrivo_da"] = "login";                
                // devo registrare last access
                msg = utenti.registraDTLA("Login utente", utenti.iduser);
                if (msg != "")
                {
                    tStato.Text = msg;
                    return;
                }
                Response.Redirect("cambiopassword.aspx?p=" + utenti.iduser.ToString());
            }
            else
            {
				if (!utenti.abilitato)
				{
					tStato.Text = "Utenete nonancora abilitato. Prego contattare l'assistenza al n. " + Session["assistenza"].ToString();
					Session["arrivo_da"] = "login";
					// devo registrare last access
					msg = utenti.registraDTLA("Login utente", utenti.iduser);
					return;
				}
				tStato.Text = string.Format("Benvenuto {0} {1}", utenti.nome, utenti.cognome);
                // devo registrare last access
                msg = utenti.registraDTLA("Login utente", utenti.iduser);
                if (msg != "")
                {
                    tStato.Text = msg;
                    return;
                }
            }
            lAsterisconn.Text = "";
            lAsteriscopwd.Text = "";
            Session.Timeout = 60;
            Session.Add("assistenza", "0461-496456");
            if (utenti.potere > 50)
                Response.Redirect("regia.aspx?p=" + utenti.iduser.ToString());
			else
                Response.Redirect("menu.aspx?p=" + utenti.iduser.ToString());
        }
    }

    protected void cbRegistrati_Click(object sender, EventArgs e)
    {
        Session.Add("assistenza", "0461-496456");
        Response.Redirect("mydata.aspx");
    }

    protected void ShowPopUpMsg(string msg)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("alert('");
        sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
        sb.Append("');");
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
    }
}