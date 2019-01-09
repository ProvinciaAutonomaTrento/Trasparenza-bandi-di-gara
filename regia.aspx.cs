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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.IO;
using System.Drawing;

public partial class regia : System.Web.UI.Page
{
    public ConnessioneSQL SQLConn = new ConnessioneSQL();
    public user utenti = new user();
    public user dettutente = new user();
    public Gare gara = new Gare();
    public DataSet ds = new DataSet();
    private DataTable tbl = null;
    public string formatodata = "dd-MM-yyyy";
    public string msg = "";
    public string s = "";
    public bool ok = false;
    public static string id = "";
    public static bool riabilitato = false;
	public static string where = "";
	public static string order = " order by ente ";
	public static string sSelect; // = "select *, CASE abilitato WHEN null THEN '' when 0 then '' WHEN 1 THEN 'OK' END AS abilitatoOK from utenti as a "; 
	public static string sSelect2;

	protected void Page_Load(object sender, EventArgs e)
    {
		sSelect = "SELECT a.id, a.nikname, a.nome, a.cognome, a.ente, a.telefono, a.mail, a.abilitato,  a.power, count(b.id) as ric, ";
		sSelect += "CASE abilitato WHEN null THEN '' when 0 then '' WHEN 1 THEN 'OK' END AS abilitatoOK ";
	    sSelect += "from utenti as a ";
		sSelect += "left join nextworks as b on b.user_ek=a.id ";
		sSelect2 = "group by a.id, a.nikname, a.nome, a.cognome, a.ente, a.telefono, a.mail, a.abilitato, a.power  ";

		Session.Timeout = 30;
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
            Session.Add("assistenza", "0461-496456");
            Int32 idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
            if (idu <= 0 || !utenti.cercaid(idu))
            {
                s = "Sessione scaduta. Prego ricollegarsi.";
                ShowPopUpMsg(s);
                Response.Redirect("default.aspx");
            }
            if (!utenti.getUserData(idu.ToString(), out msg))
            {
                Stato("Problema con lettura dati utente. Contattare il servizio assistenza al n. " + Session["asistenza"].ToString(), Color.Red);
            }
            id = utenti.iduser.ToString();
            Inizializza();
			pDettaglio.Visible = false;

			// cerco gare per quell'Utente
			msg = "";
            SQLConn.openaSQLConn(out msg);
            if (msg.Length > 0)
            {
                Stato("ERRORE: " + msg + ". Contattare servizio assistenza al n. " + Session["assistenza"].ToString(), Color.Red);
                return;
            }
			s = "select tipoente_ek from elencostrutture where struttura ='" + utenti.ente +"'";
			tbl = new DataTable();
			tbl = SQLConn.getfromDSet(s, "tipoente", out msg);
			Session.Add("tipoente", "");
			if (msg.Length == 0 && tbl.Rows.Count > 0)
				Session.Add("tipoente", tbl.Rows[0]["tipoente_ek"].ToString());

            showRegia();
            msg = "";
            
            if (dgvComm.Rows.Count > 0) {
                ok = RiempiGrid(tbl);
                pGara.Visible = false;
            }
            else
            {   
                dgvComm.Visible = false;
                pGara.Visible = true; // se non ci sono gare già caricare entro in modalità edit nuova gara
                bDel.Visible = true;
            }
        }
    }

    private void Inizializza()
    {
        LBenvenuto.Text = " Benvenuto " + utenti.nome + " " + utenti.cognome;
        // carico comunque le tabelle nella maschera di input/output
        ddlPotere.Items.Insert(0, new ListItem("Utente", "0"));
        ddlPotere.Items.Insert(1, new ListItem("Admin", "100"));
        lNome.Text = "";
        lCognome.Text = "";
        lTel.Text = "";
        lMail.Text = "";
        lEnte.Text = "";
        sStato.Text = "";
    }

    protected bool RiempiGrid(DataTable tbl)
    {
		Stato("Occorrenze trovate: " + (tbl != null ? tbl.Rows.Count.ToString() : "0"), Color.Black);
        try
        {
            //int gvHasRows = gElenco.Rows.Count;
            dgvComm.DataSource = tbl;
            dgvComm.DataBind();
            dgvComm.Visible = true;
        }
        catch (Exception ex)
        {
            Stato("Riscontrato errore durante la ricerca committenze. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString(), Color.Red);
            return (false);
        }
        dgvComm.Visible = true;
        bDel.Visible = false;
        return (true);
    }
    protected void ShowPopUpMsg(string msg)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("alert('");
        sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
        sb.Append("');");
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
    }

    protected void bUscita_Click(object sender, EventArgs e)
    {
        Session.Timeout = 70;
        if (!dgvComm.Visible) // non è uscita è Home
        {
            pGara.Visible = false;
            bAnnulla.Visible = false;
            bDel.Visible = false;
            bSalva.Visible = false;
            sStato.Text = "";
            showRegia();
			dgvComm.Visible = true;
			return;
        }
        bSalva.Visible = false;
        Session.Clear();
        Session.Abandon();
        Response.Redirect("Login.aspx");
    }

    protected void bSalva_Click(object sender, EventArgs e)
    {
        Session.Timeout = 70;
		Int32 idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		if (idu < 0)
        {
            Stato("ATTENZIONE: sessione scaduta. Ricollegarsi.", Color.Red);
            return;
        }
		utenti.cercaid(idu); // nonso perchè mi cerco ???
        Int32 userid = -1;
		Int32.TryParse(dgvComm.SelectedValue != null ? dgvComm.SelectedValue.ToString() : "", out userid);  // mi prendo l'id dell'utente da abilitare
		dettutente.cercaid(userid);
		dettutente.abilitato = cbAbilitato.Checked;
        dettutente.potere = int.Parse(ddlPotere.SelectedValue);
		dettutente.limite = cldLimite.SelectedDate;
        if (userid >= 0)
        {
            if (!dettutente.registradatiutente("registrazioni", utenti.iduser))
            {
                Stato("Modifica richiesta non andata a buon fine. Prego contattare servizio assistenza al n. " + Session["assistenza"].ToString(), Color.Red);
				return;
            }
            // ok inoltro la richiesta
            if (riabilitato == false && dettutente.abilitato == true)
            {
                gmail gm = new gmail();
                gm.subject = "Abilitazione form richieste servizi Agenzia per gli Appalti e Contratti.";
                gm.body = "Buongiorno gentile " + dettutente.nome + " " + dettutente.cognome + "\n\n";
                gm.body += "    di seguito le credenziali per l'accesso all'applicazione riservata alla gestione delle richieste di servizi, rivolte all'APAC.\n\n";
                gm.body += "Username\t" + dettutente.nikname + "\n";
                gm.body += "Password\t" + dettutente.password + "\n\n\n";
                gm.body += "Cordiali saluti.\n";
                string[] achi = { dettutente.mail };
                gm.achi = achi;
                string[] achiccn = { "tiziano.donati@provincia.tn.it", "carlo.martinelli@provincia.tn.it" };
                gm.achiccn = achiccn; // PER ORA NON INVIO A NESSUN ALTRO
                gm.dachi = "apac@provincia.tn.it";
                gm.mandamail("", 0, "", "", out msg);

                if (msg.Trim() != "")
                    sStato.Text = "ATTENZIONE: credenziali utente non inviate correttamente. Contatare il servizio assistenza al n. " + Session["assistenza"].ToString();
                else
                    sStato.Text = "Utente abilitato e inviata mail con nuove credenziali!";
            }
            else
                sStato.Text = "Modifica effettuata.";
        }
        else // devo aggiungere la nuova gara
        {
            Stato("Inserimento nuova richiesta non andata a buon fine. Prego contattare servizio assistenza al n. " + Session["assistenza"].ToString(), Color.Red);
            return;
        }
    }

    protected void showRegia()
    {
        //sStato.Text = "Dati salvati correttamente.";

        s = sSelect + where + sSelect2 + " order by ente";
		//s += "where a.user_ek=\'" + utenti.iduser + "\'";
		bDisabilitati.Visible = true;
		pDettaglio.Visible = false;
		msg = "";
		//tbl.Clear();
		tbl = new DataTable();
        tbl = SQLConn.getfromDSet(s, "utenti", out msg);
        if (tbl.Rows.Count > 0)
        {
            ok = RiempiGrid(tbl);
			Stato("Occorrenze trovate " + tbl.Rows.Count, Color.Black);
        }
    }
    protected void dgvComm_SelectedIndexChanged(object sender, EventArgs e)
    {
		Session.Timeout = 30;
		Int32 idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		if (idu <= 0)
        {
            Stato("ATTENZIONE: sessione scaduta. Ricollegarsi.", Color.Red);
			return;
        }
        string key = dgvComm.SelectedValue != null ? dgvComm.SelectedValue.ToString(): "";
        
        if (key == null || key == "")
        {
            Stato("Nessuna utente caricato....", Color.Red);
			return;
        }
        dgvComm.Visible = false;
        pGara.Visible = true;
        
        long idl = long.Parse(key);
        if (!dettutente.cercaid(idl))
        {
            sStato.Text = "Utente non trovato!";
            return;
        }
        lEnte.Text = dettutente.ente;
        ddlPotere.SelectedValue = dettutente.potere.ToString();
        lNome.Text = dettutente.nome;
        lCognome.Text = dettutente.cognome;
        lTel.Text = dettutente.telefono;
        lMail.Text = dettutente.mail;
        cbAbilitato.Checked = dettutente.abilitato;
        riabilitato = cbAbilitato.Checked;
		lCitta.Text = dettutente.città;
		lCap.Text = dettutente.cap;
		lIndirizzo.Text = dettutente.indirizzo;
		lCivico.Text = dettutente.civico;
		cldLimite.SelectedDate = dettutente.limite;
		cldLimite.SelectionMode = CalendarSelectionMode.Day;
		cldLimite.Caption = dettutente.limite.ToShortDateString();
        bSalva.Visible = true;
		// visualizzo grid richieste
		Session.Timeout = 30;
		//tbl.Clear();
		// andrebbe fatto tutto nextworkscentrico
		s = "select a.id, a.titolo, a.ente, a.dtc, a.dtla, a.convalida, a.stato, a.importoasta, b.nome, b.cognome, b.id as userID, c.gara_ek as garaID, ";
		s += "d.classificazione as tipologia, e.proposta as servizio, f.descrizioneaffidamento as affidamento, g.procedura, ";
		s += "CASE a.finanziamentoPAT WHEN null THEN '' when 0 then '' WHEN 1 THEN 'SI' END AS [fin.a], ";
		s += "CASE a.stato WHEN null THEN 'Inserita' when 0 then 'Inserita' WHEN 1 THEN 'CONFERMATA' END AS Stat ";
		s += "from nextworks as a ";
		s += "left join utenti as b on a.user_ek = b.id ";
		s += "left join usergare as c on a.id = c.gara_ek ";
		s += "left join tipologia d on a.tipologia_EK = d.id ";
		s += "left join proposta e on a.tiposerviziorichiestoinizialmente_EK = e.id ";
		s += "left join affidamento f on a.sistemaaffidamento_EK = f.id ";
		s += "left join procedura g on a.criterioaggiudicazione_EK = g.id ";
		s += "where a.user_ek = " + dettutente.iduser.ToString() + " ";
		s += "order by a.dtc ";

		/*
		s = "select a.*, b.*, c.classificazione as tipologia, d.proposta as servizio, e.descrizioneaffidamento as affidamento, f.procedura, ";
		s += "CASE finanziamentoPAT WHEN null THEN '' when 0 then '' WHEN 1 THEN 'SI' END AS [fin.a], ";
		s += "CASE stato WHEN null THEN 'Inserita' when 0 then 'Inserita' WHEN 1 THEN 'CONFERMATA' END AS Stat ";
		s += "from usergare a ";
		s += "left join nextworks b on a.gara_ek = b.id ";
		s += "left join tipologia c on b.tipologia_EK = c.id ";
		s += "left join proposta d on b.tiposerviziorichiestoinizialmente_EK = d.id ";
		s += "left join affidamento e on b.sistemaaffidamento_EK = e.id ";
		s += "left join procedura f on b.criterioaggiudicazione_EK = f.id ";
		s += "where a.user_ek = " + dettutente.iduser.ToString() + " ";
		s += "order by dtla desc";
		*/
		tbl = new DataTable();
		tbl = SQLConn.getfromDSet(s, "dettaglio", out msg);
		if (msg.Length != 0)
		{
			Stato("Errore: dettaglio gare utente -> " + msg.ToString(), Color.Red);
			return;
		}
		//showRegia();
		msg = "";
		bDisabilitati.Visible = false;
		if (tbl.Rows.Count > 0)
		{
			ok = RiempiDettaglio(tbl);
			pDettaglio.Visible = true;
			Stato("Occorrenze trovate " + tbl.Rows.Count, Color.Black);
		}
		else
		{
			gvDettaglio.Visible = false;
			sStato.Text = "Non ci sono gare per questo user.";
		}
	}
	protected void bDel_Click(object sender, EventArgs e)
    {
        bConferma.Text = "Confermi la CANCELLAZIONE ?";
        bConferma.Visible = true;
        bAnnulla.Visible = true;
    }

    protected void bAnnulla_Click(object sender, EventArgs e)
    {
        bConferma.Visible = false;
        bAnnulla.Visible = false;
    }

    protected void bConferma_Click(object sender, EventArgs e)
    {
        if (utenti.iduser <= 0 && id == "")
        {
            Stato("ATTENZIONE: sessione scaduta. Ricollegarsi.", Color.Red);
            return;
        }
    }
    protected void cbMyAccount_Click(object sender, EventArgs e)
    {
        Session["arrivo_da"] = "home";
        Response.Redirect("mydata.aspx?l=si");
    }

	protected void bDett_Click(object sender, EventArgs e)
	{
	}
	protected bool RiempiDettaglio(DataTable tbl)
	{
		try
		{
			//int gvHasRows = gElenco.Rows.Count;
			gvDettaglio.DataSource = tbl;
			gvDettaglio.DataBind();
			gvDettaglio.Visible = true;
		}
		catch (Exception ex)
		{
			Stato("Riscontrato errore durante la ricerca committenze. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString(), Color.Red);
			return (false);
		}
		pDettaglio.Visible = true;
		gvDettaglio.Visible = true;
		//bDel.Visible = false;
		return (true);
	}

	protected void dgvComm_Sorting(object sender, GridViewSortEventArgs e)
	{
		string sortcriteria;
		sortcriteria = " order by \"" + e.SortExpression + "\"";

		msg = "";
		//tbl.Clear();
		tbl = SQLConn.getfromDSet(sSelect + sortcriteria + sSelect2, "utenti", out msg);
		if (tbl.Rows.Count > 0)
		{
			pDettaglio.Visible = false;
			ok = RiempiGrid(tbl);
		}
	}
	protected void Stato(string s, Color c)
	{
		sStato.Text = s;
		if (c == null) c = Color.Black;
		sStato.ForeColor = c;
	}

	protected void bDisabilitati_Click(object sender, EventArgs e)
	{
		if (bDisabilitati.Text == "NON Abilitati")
		{
			where = "where abilitato !=1 ";
			bDisabilitati.Text = "Tutti";
		}
		else
		{
			where = " ";
			bDisabilitati.Text = "NON Abilitati";
		}
		tbl = new DataTable();
		tbl = SQLConn.getfromDSet(sSelect + where + sSelect2 + order, "utenti", out msg);
		pDettaglio.Visible = false;
		ok = RiempiGrid(tbl);
	}
}
