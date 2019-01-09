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
using System.Data;
using System.Data.SqlClient;
using System.Text;

public partial class cambiopasswrd : System.Web.UI.Page
{
	public string msg = "";
	public string codice = "";
	public static user utenti = new user();

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
		{
			if (Request.QueryString["l"] != null)
				codice = Request.QueryString["l"].ToString();
			else
				codice = "";
			Session.Add("assistenza", "0461-496456");
			checkSession();
		}
	}
	private void checkSession()
	{
		Int32 idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		if (idu <= 0 || !utenti.cercaid(idu))
		{
			string s = "Sessione scaduta. Prego ricollegarsi.";
			ShowPopUpMsg(s);
			Response.Redirect("login.aspx");
		}
     	codice = utenti.iduser.ToString();
	}
	protected void cbRegistrati_Click(object sender, EventArgs e)
    {
        // controllo correttezza password
        // registro la password nuova e piallo forocambiopassword registro dtla
        // in base session("Arrivoda") decido dove devo andare
        // poi controllo validità e consistenza nuova poassword
        // poi controllo uguaglianza
        // devo verificare che la vecchia password sia quella nella db; 
        // poi memorizzo la nuova password e smorzo Forzatura cambio password
        // string nuova, vecchia, conferma;
        string vecchia = Server.HtmlEncode(tVecchia.Text);
        string nuova = Server.HtmlEncode(tNuovaPwd.Text);
        string conferma = Server.HtmlEncode(tNuovaPwd2.Text);
        lconferma.Enabled = false;
        lnuova.Enabled = false;
        lpwd.Enabled = false;

        bool ok = false;
        if (nuova.Length < 8)
        {  //cbShowPopUpMsg("La password deve essere di almeno 8 caratteri !");
            tStato.Text = "La nuova password deve essere almeno di 8 caratteri!";
            lnuova.Enabled = true;
            lnuova.Text = "* almeno 8 caratteri.";
            lnuova.Enabled = false;
            tNuovaPwd2.Text = "";
            tNuovaPwd.Text = ""; // mi serve per avere il focus
            Console.Beep();
        }
        else
        {
            if (nuova != conferma)
            {
                // cbShowPopUpMsg("La nuova password e la conferma della password devono essere uguali !");
                lconferma.Enabled = true;
                lconferma.Text = "* non coincide con la password.";
                lconferma.Enabled = false;
                tStato.Text = "La conferma della password non coincide con la password. Nuova password e conferma password devono essere uguali!";
                tNuovaPwd2.Text = ""; // mi serve per avere il focus
                tNuovaPwd.Text = nuova;
            }
            else
            {
                ConnessioneSQL cn = new ConnessioneSQL();
                SqlConnection cnn = cn.openaSQLConn(out msg);
                if (cnn == null)
                {
                    tStato.Text = msg;
                    return;
                }
                Int32 id;
                Int32.TryParse(Session["iduser"].ToString(), out id);
                utenti.iduser = id;
                if (!utenti.cercaid(id))
                {
                    tStato.Text = "Problemi su ricerca utente: contattare l'amministratore al n. 0461 496466";
                    return;
                }
                utenti.password = nuova;
                utenti.forzocambiopassword = false;
                ok = utenti.registradatiutente("Forzo cambio password", utenti.iduser);
                cnn.Close();
                if (!ok)
                {
                    ShowPopUpMsg("Cambio della password non riuscito! Premi un tasto per continuare.");
                    return;
                }
                ShowPopUpMsg("La password è stata cambiata correttamente ! Premi un tasto per continuare.");
				Session.Timeout = 30;
                if (Session["arrivo_da"].ToString() == "login" || (Request.QueryString["l"] != null && Request.QueryString["l"].ToString() == "y"))  // da sostituire con qualcosa di altro
                    Response.Redirect("menu.aspx?p=" + utenti.iduser.ToString().Trim());
                else
                    Response.Redirect("Default.aspx");
            }
        }
    }
    protected void ShowPopUpMsg(string msg)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("alert('");
        sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
        sb.Append("');");
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
    }
    protected void Logout_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("default.aspx");
    }
}