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

public partial class myapac : System.Web.UI.Page
{
    public bool loggato;
    public user utente = new user();
    public ConnessioneSQL SQLClass = new ConnessioneSQL();
    public DataSet ds = new DataSet();
    private DataTable tbl = null;
    public string msg = "";
	public Int32 id = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        loggato = false;
		tNikname.Focus();
		if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
			Leggiddl("select struttura, tipoente_ek from elencostrutture order by struttura", ddlEnte, true); // devo leggere la tabella Proposta          
			id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
			if (id >=0) 
			{
				Session.Timeout = 30;
				loggato = true;
				hlHome.Text = "Le mie richieste";
				hlhometop.Text = "Le mie richieste";
				ddlEnte.Enabled = false;
				cbNonPresente.Enabled = false;
			}
			if (loggato) // riempio la maschera con i dati
            {
                if (utente.cercaid(id))
                {
                    cbRegistrati.Text = "Salva";
                    tNikname.Text = utente.nikname;
                    tNome.Text = utente.nome;
                    tCognome.Text = utente.cognome;
                    tMail1.Text = utente.mail;
                    //tMatricola.Text = utente.matricola;
                    tEnte.Text = utente.ente;
                    cbNonPresente.Checked = false;
                    if (ddlEnte.Items.IndexOf(ddlEnte.Items.FindByText(utente.ente)) < 0)
                    {
						// ente non presente in elenco
                        cbNonPresente.Checked = true;
                        tEnte.Visible = true;
						tEnte.Text = utente.ente.Trim();
						ddlEnte.Visible = false;
                    }
                    else
                    {
                        tEnte.Visible = false;
                        ddlEnte.Visible = true;
						ddlEnte.SelectedIndex = ddlEnte.Items.IndexOf(ddlEnte.Items.FindByText(utente.ente));
					}
                    tIndirizzo.Text = utente.indirizzo;
                    tCivico.Text = utente.civico;
                    tCap.Text = utente.cap;
                    tCitta.Text = utente.città;
                    tTelefono.Text = utente.telefono;
                    tMail2.Text = utente.mail;
                    cbConsenso.Checked = true;					
					hlHome.NavigateUrl = "~/menu.aspx?p=" + utente.iduser.ToString().Trim();
					hlhometop.NavigateUrl = "~/menu.aspx?p=" + utente.iduser.ToString().Trim();
				}
                else
                    tStato.Text = "Utente non trovato. Effettuare il login.";
            }
        }
    }

    protected void cbRegistrati_Click(object sender, EventArgs e)
    {
		id = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		if (id >= 0) utente.cercaid(id); // mi assicuro di leggere abilitato e pwd (quei dati che non chiedo a video)
		// avvia i controlli
		string er = "";
        if (tNikname.Text.Length < 7 || tNikname.Text.Trim().Length > 24) er += "Username con almeno 8 caratteri (max 24); ";
        if (tNome.Text.Length < 2) er += "Nome con almeno 3 caratteri; ";
        if (tCognome.Text.Length < 2) er += "Cognome con almeno 3 caratteri; ";
        if (tMail1.Text.Length < 6 || (tMail1.Text.IndexOf("@") < 1) || (tMail1.Text.IndexOf(".") < 1) ) er += "E-Mail con almeno 7 caratteri e con il simbolo @ e il simbolo . ; ";
        if (tMail1.Text != tMail2.Text) er += "E-Mail e Conferma E-Mail; ";
		//if (tMatricola.Text.Length < 4) er += "Matricola; ";
		if (cbNonPresente.Checked) // ente non presente    -> può essere che ddlselezionato ma nonpresente = vero!!!!!
		{
			if (tEnte.Text.Trim().Length < 3) er += "Ente con almeno 3; ";
		}
		else
			if (ddlEnte.SelectedItem.Text.Trim().Length < 2) er += "Ente non selezionato; ";
        if (tIndirizzo.Text.Length < 5 ) er += "Indirizzo con almeno 6 caratteri; ";
        if (tCivico.Text.Trim().Length < 1) er += "Civico; ";
        if (tCap.Text.Trim().Length < 5 ) er += "Cap con almeno 6 caratteri; ";
        if (tCitta.Text.Trim().Length < 2 ) er += "Città con almeno 3 caratteri; ";
        if (tTelefono.Text.Trim().Length < 8 ) er += "Telefono con almeno 9 caratteri; ";
        if (tTelefono.Text.IndexOf(" ") > 0 ) er += "Telefono: solo numeri senza spazi; ";
        if (!cbConsenso.Checked) er += "Consenso al trattamento: è necessario; ";
		if (cbNonPresente.Checked && tEnte.Text.Trim().Length < 3) er += "Ente: con almeno 3 caratteri; ";
		if (!cbNonPresente.Checked && ddlEnte.SelectedItem.Text.Trim().Length < 2) er += "Ente: con almeno 2 caratteri; ";
		if (er != "")
        {
            er = "I valori dei campi indicati sono errati o mancanti: " + er;
            tStato.Text = er;
            return;
        }
        utente.nikname = tNikname.Text.Trim();
        utente.nome = tNome.Text.Trim();
        utente.cognome = tCognome.Text.Trim();
        utente.mail = tMail1.Text.Trim();
		//utente.matricola = tMatricola.Text.Trim();
		utente.ente = cbNonPresente.Checked ? tEnte.Text.Trim() : ddlEnte.SelectedItem.Text.Trim();
		utente.ente_ek = "";
		utente.tipoente_ek = "";
		Gare gara = new Gare();
		tbl = gara.CercaEnte(null, utente.ente, out msg);
		if (tbl != null && tbl.Rows[0]["tipoente_ek"] != DBNull.Value)
			utente.tipoente_ek = tbl.Rows[0]["tipoente_ek"].ToString().Trim();
		utente.indirizzo = tIndirizzo.Text.Trim();
        utente.civico = tCivico.Text.Trim();
        utente.cap = tCap.Text.Trim();
        utente.città = tCitta.Text.Trim();
        utente.telefono = tTelefono.Text.Trim();
        utente.scadenza = DateTime.Now.AddMonths(12);

		// non ci sono errori di inserimento e posso registrare la richiesta o la modifica
		if (id > 0) // è una modifica
        {
            bool ok = utente.registradatiutente("Modifica anagrafica utente", (Int32)id);
            if (!ok)
            {
                tStato.Text = "Modifiche non apportate. Contattare il servizio assistenza al n. " + Session["assistenza"].ToString();
                return;
            }
            Response.Redirect("menu.aspx?p=" + utente.iduser.ToString().Trim() + "&l=si");
        }
        else
        {   // devo controllare se l'utente c'è già, se no lo registro ed invio mail di richiesta registrazione ed abilitazione
            // poi ritorno alla pagina di default con loggato = no
            DataSet ds = new DataSet();
			SQLClass.SQLc.Parameters.Clear();
			SQLClass.SQLc.Parameters.Add("@nikname", SqlDbType.NVarChar); SQLClass.SQLc.Parameters["@nikname"].Value = System.Convert.ToString(utente.nikname);
			if (SQLClass.getSQLdata("select * from utenti where nikname=@nikname", "utenti", out msg) > 0)
            {
                tStato.Text = "Utente con username " + utente.nikname + " già presente. Accertarsi di non essere già stati registrati o cambiare Username!";
                //ShowPopUpMsg("Utente con username " + utente.nikname + " già presente. Accertarsi di non essere già stati registrati o cambiare Username!");
                return;
            }
            else
            {
                utente.password = utente.CalcolaPasswordCasuale(8, 3, 3, 3);
                utente.abilitato = false;
                utente.forzocambiopassword = true;
                utente.giornalizza = false;
                //utente.ente = (utente.ente);
                if (utente.aggiungiutente())
                {
                    // ok registrazione effettuata. ora manda mail agli amministratori
                    // devo rileggere il record aggiunto per avere l'id
                    if (!utente.cercanikname(utente.nikname, utente.password))
                    {
                        tStato.Text = "Richiesta di registrazione non andata a buon fine. Prego contattare l'assistenza al n. " + (string)Session["assistenza"];
						return;
						//ShowPopUpMsg("Richiesta di registrazione non andata a buon fine. Prego contattare l'assistenza al n. " + (string)Session["assistenza"]);
                    }

                    Session.Add("Utente", utente.nikname);
                    Session.Add("iduser", utente.iduser);
					
					// inoltro mail a tutti gli amministratori

					msg = "";
                    ds.Clear();
                    tbl = SQLClass.getfromDSet("select id, nikname, nome, cognome, mail, power from utenti where (power >= 50)", "Admin", out msg);
                    if (msg.Trim() != "")
                    {   tStato.Text = "Attenzione: problema di connessione. Prego contattare il n. " + (string)Session["assistenza"];
                        return ;
                    }
                    int k = 0, n = tbl.HasErrors ? 0 : tbl.Rows.Count;
                    string[] achisccn = new string[n];
                    string[] achi = new string[1];
                    achi[0] = utente.mail;

                    for (int i = 0; i < n; i++)
                    {
                        if (tbl.Rows[i]["Mail"] != DBNull.Value)
                            achisccn[k++] = tbl.Rows[i]["Mail"].ToString();
                    }
                    if (k == 0)
                    {
                        tStato.Text = "Attenzione: non ci sono amministratori che posso confermare la tua richiesta. Prego contattare il n. " + (string)Session["assistenza"];
                        //ShowPopUpMsg("Attenzione: non ci sono amministratori che posso confermare la tua richiesta. Prego contattare il n. " + (string)Session["assistenza"]);
                    }
                    else
                    {
                        gmail gm = null;
                        gm = new gmail();
                        gm.dachivisualizzato = "Agenzia prov. pre gli Appalti e Contratti";
                        gm.achi = achi;
                        gm.achiccn = achisccn;
                        gm.numeritel = "0461-496456";
                        gm.subject = "Raccolta fabbisogno: richiesta registrazione nuovo utente";
                        gm.body = "Buongiorno,\n";
                        gm.body += "\tla informaiamo che la sua domanda di registrazione all'applicazione web\n";
                        gm.body += "per la presentazione delle richieste di gara per le quali s'intende avvalersi\n";
                        gm.body += "dei servizi offerti da APAC è stata inoltrata correttamente.\n";
                        gm.body += "\n\nI dati inseriti sono:\n";
                        gm.body += "Username:          \t" + utente.nikname + "\n";
                        gm.body += "Nome:              \t" + utente.nome + "\n";
                        gm.body += "Cognome:           \t" + utente.cognome + "\n";
                        gm.body += "Mail:              \t" + utente.mail + "\n";
                        gm.body += "Tel.:              \t" + utente.telefono + "\n\n\n";
                        gm.body += "Dopo l'approvazione, riceverà automaticamente la mail con le credenziali per il primo accesso.\n\n";
                        gm.body += "Cordiali saluti.\n";
                        if (!gm.mandamail("", 0, "", "", out msg))
                        {
                            tStato.Text = "Richiesta di registrazione non eseguita! MAIL DI CONFERMA NON INOLTRATA!. CONTATTARE IL NUMERO " + (string)Session["assistenza"] + " Err: " + msg;
                            //ShowPopUpMsg("Richiesta di registrazione effettuata con successo, MAIL DI CONFERMA NON INOLTRATA!. CONTATTARE IL NUMERO " + (string)Session["assistenza"] + " Err: " + msg);
                        }
                        else
                        {
                            tStato.Text = "Richiesta di registrazione effettuata con successo. Riceverà via email le credenziali per l'accesso.";
							cbRegistrati.Enabled = false;
                            //ShowPopUpMsg("Richiesta di registrazione effettuata con successo. Riceverà, entro 24 ore via email, le credenziali per l'accesso.");
                        }
                    }
                }
                else
                    tStato.Text = "Richiesta di registrazione non andata a buon fine. Prego contattare l'assistenza al n. " + (string)Session["assistenza"];
            }
        }
    }
    protected string virgolette(string s)
    {
        string ss = "";
        string virgoletta = "'";
        string doppie = string.Format("{0}", "\"");
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == virgoletta[0])
                ss += virgoletta + virgoletta;
            else ss += s[i];
            /*else
                if (s[i] == doppie[0])
                    ss += virgoletta;
            else
                ss += s[i]; */
        }
        return (ss);
    }
	//Leggiddl("select struttura, tipo_ente_ek order by struttura", ddlEnte, true); // devo leggere la tabella Proposta
	public void Leggiddl(string select, DropDownList ddl, bool vuota) // devo leggere la tabella Proposta
    {
        string msg = "";
        SQLClass.openaSQLConn(out msg);
        if (msg.Length >= 1)
        {
            tStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            SQLClass.closeaSQLConn(out msg);
        }
        else
        {
            msg = "";            
            //tbl.Clear();
            tbl = SQLClass.getfromDSet(select, "tb", out msg);
            SQLClass.closeaSQLConn(out msg);
            if (msg.Length >= 1)
                tStato.Text = "ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            else
            {
                if (tbl.Rows.Count > 0)
                {
                    ddl.Items.Clear();
                    string s = "";
                    for (int i = 0; i < tbl.Rows.Count; i++)
                    {
                        s = tbl.Rows[i][0] == DBNull.Value ? "" : tbl.Rows[i][0].ToString();
						//ss = tbl.Rows[i][1] == DBNull.Value ? "" : tbl.Rows[i][1].ToString();
						//ddl.Items.Insert(i, new ListItem(s, ss));
						ddl.Items.Insert(i, s);
					}
                    if (vuota)
                        ddl.Items.Insert(0, "");                    
                }
                else
                    tStato.Text = "ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella " + select.Substring(select.IndexOf( "FROM ") + 4, 10).ToUpper() + ". Contattare l'assistenza al numero " + (string)Session["assistenza"];
            }
        }
    }

    protected void cbNonPresente_CheckedChanged(object sender, EventArgs e)
    {
        ddlEnte.Visible = !ddlEnte.Visible;
        tEnte.Visible = !tEnte.Visible;       
    }

	protected void bCambiaPassword_Click(object sender, EventArgs e)
	{
		Response.Redirect("cambiopassword.aspx?l=y");
	}
}