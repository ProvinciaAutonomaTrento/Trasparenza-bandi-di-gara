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
using System.Linq;
using iTextSharp.text.pdf;
using System.Threading;
using System.Web;
using System.Net.Mime;
using System.Data.SqlClient;
using System.Net.Mail;

public partial class menu : System.Web.UI.Page
{
	private ConnessioneSQL SQLClass = new ConnessioneSQL();
	private user utenti = new user();
	private Gare gara = new Gare();
	private www Log = new www();
	private DataSet ds = new DataSet();
	private DataTable tbl = null;
	public string formatodata = "dd-MM-yyyy";
	public string msg = "";
	public string s = "";
	public string sqlfiltro;
	public string sqlsort;
	public bool ok = false;
	public Color rosso = Color.Red;
	public Color nero = Color.Black;
	private bool modificato;
	private string codice; //mi serve solo per sapere se si tratta di una convalid
	private string idgara; // contiene il codice della gara
	public string id = "";
	public string[] stato = new string[6]; // = { "Inserita", "Confermata", "Accettata", "Respinta", "Inidonea", "Ritirata" }; // 0, 1, 2, 3....

	protected void Page_Load(object sender, EventArgs e)
	{
		stato[0] = "Inserita"; 	stato[1] = "Confermata"; stato[2] = "Accettata";
		stato[3] = "Respinta";	stato[4] = "Inidonea";	stato[5] = "Ritirata";

		if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
		{
			if (Request.QueryString["c"] != null)
			{
				codice = Request.QueryString["c"].ToString();
				Session.Add("idgara", codice);
			}
			else
			{
				codice = "";
				Session.Add("idgara", "");
			}
			modificato = false;	Session.Add("modificato", "NO");

			Session.Add("assistenza", "0461-496456");
			if (!checkSession())
			{
				Stato("ATTENZIONE: sessione scaduta. Prego identificarsi.", Color.Red);
				return;
			};
			Inizializza();
			sqlfiltro = " a.user_ek=\'" + utenti.iduser + "\' ";
			sqlsort = " a.dtc desc ";

			Session.Add("filtro", sqlfiltro);
			Session.Add("sort", sqlsort);

			// cerco gare per quell'Utente
			msg = "";
			SQLClass.openaSQLConn(out msg);
			if (msg.Length > 0)
			{
				Stato("ERRORE: " + msg + ". Contattare servizio assistenza al n. " + Session["assistenza"].ToString(), rosso);
				return;
			}
			s = "select a.user_ek, a.id, a.titolo, a.importoasta, a.tipologia_ek, left(cast(a.convalida as date),10) as convalidaasdata, ";
			//s += "b.tiposerviziorichiestoinizialmente_ek, b.tempiapprovazioneavvio, b.sistemaaffidamento_ek, b.finanziamentoPAT, b.lotti, b.stato, b.soglia, b.procedura_ek ";
			s += "a.tiposerviziorichiestoinizialmente_ek, a.sistemaaffidamento_ek, a.finanziamentoPAT, a.lotti, a.stato, a.soglia, a.procedura_ek, a.rup, a.uscente ";

			sqlfiltro = Session["filtro"].ToString() != null ? Session["filtro"].ToString() : " a.user_ek =\'" + utenti.iduser + "\' ";
			sqlsort = Session["sort"].ToString() != null ? Session["sort"].ToString() : " a.dtc desc ";

			s += "from nextworks as a ";
			s += "left join lotto as c on a.id = c.lottorichiesta_ek ";
			s += "left join utenti as d on a.user_ek = d.id ";
			s += "where " + sqlfiltro; //a.user_ek=\'" + utenti.iduser + "\' ";
			s += "order by " + sqlsort; 

			msg = "";
			tbl = new DataTable();
			tbl = SQLClass.getfromDSet(s, "gareutente", out msg);

			if (codice.Trim().Length > 0)  // SI TRATTA DI CONVALIDA
			{
				VisualizzaGara(codice, "Cliccare pulsante \"Conferma Richiesta\", per produrre la documentazione da inviare ad APAC firmata digitalmente e per confermare la richiesta!");
				return;
			}
			bAdd.Visible = true;
			if (tbl.Rows.Count > 0)
			{
				ok = RiempiGrid(tbl);
				pGara.Visible = false;
			}
			else
			{
				// nessuna gara per questo utente; se non ci sono gare già caricare entro in modalità edit nuova gara
				ddlSortExpression.Visible = false;
				dgvComm.Visible = false;
				pGara.Visible = true;
				pconferma.Visible = false;
				bDel.Visible = false;
				cbMyAccount.Visible = false;
				if (utenti.tipoente_ek != "1" && utenti.tipoente_ek != "") // se non è PAT allora visualizzo Finanziamento si-no e basta....
				{
					lFinanziamentoPAT.Visible = true; ddlFinanziamento.Visible = true;
					//lMop.Visible = false; tMop.Visible = false;
				}
				else
				{
					//lFinanziamentoPAT.Visible = false; ddlFinanziamento.Visible = false;
					//lMop.Visible = false; tMop.Visible = false;
				}
				bAdd.Text = "Salva";
				bUscita.Text = "Le mie richieste";
			}
		}
	}

	private bool checkSession()
	{
		Int32 idu = Session["iduser"] != null ? Int32.Parse(Session["iduser"].ToString()) : -1;
		if (idu <= 0 || !utenti.cercaid(idu))
		{
			s = "Sessione scaduta. Prego ricollegarsi.";
			Session.Clear();
			Session.Abandon();
			ShowPopUpMsg(s);
			return (false);
		}
		Session.Timeout = 30; // ritacco il conteggio!!!
		id = utenti.iduser.ToString();
		return(true);
	}

	private void Inizializza()
	{
		LBenvenuto.Text = " Benvenuto " + utenti.nome + " " + utenti.cognome;
		// carico comunque le tabelle nella maschera di input/output
		Leggiddl("proposta", ddlServizio, "", "id desc");
		Leggiddl("affidamento", ddlSistemaAffidamento, "pubblico = 1", "descrizioneaffidamento");
		Leggiddl("tipologia", ddlTipologia, "pubblico = 1", "");
		Leggiddl("procedura", ddlAffidamento, "pubblico = 1", "");
		Leggiddl("criterioaggiudicazione", ddlCriterio, "", "");
		Leggiddl("settore", ddlSettore, "abilitato = 1", "");
		ddlSortExpression.Items.Clear();
		ddlSortExpression.Items.Add(new ListItem("", "a.dtc desc", true));
		ddlSortExpression.Items.Add(new ListItem("Data conferma", "a.convalida desc", true));
		ddlSortExpression.Items.Add(new ListItem("Oggetto", "a.Titolo", true));
		ddlSortExpression.Items.Add(new ListItem("Base d'asta", "a.importoasta desc", true));
		ddlSortExpression.Items.Add(new ListItem("Stato richiesta", "a.stato desc", true));

		ddlLotti.Items.Clear(); ddlLotti.Items.Insert(0, new System.Web.UI.WebControls.ListItem("NO", "0")); ddlLotti.Items.Insert(1, new System.Web.UI.WebControls.ListItem("SI", "1"));
		ddlFinanziamento.Items.Clear(); ddlFinanziamento.Items.Insert(0, new System.Web.UI.WebControls.ListItem("NO", "0")); ddlFinanziamento.Items.Insert(1, new System.Web.UI.WebControls.ListItem("SI", "1"));
		ddlSoglia.Items.Clear(); ddlSoglia.Items.Insert(0, new System.Web.UI.WebControls.ListItem("NO", "0")); ddlSoglia.Items.Insert(1, new System.Web.UI.WebControls.ListItem("SI", "1"));
		lEnte.Text = utenti.ente;
		//tBando.Text = "";
		tDocumentazione.Text = "";
		tOgg.Text = "";
		tImporto.Text = "";
		tMop.Text = "";
		tRef.Text = "";
		tRef_Tel.Text = "";
		tRef_Mail.Text = "";
		tNote.Text = "";
		tRup.Text = "";
		tUscente.Text = "";
		sStato.Text = "";
		bDel.Visible = true;
		bAdd.Text = "Salva";
	}

	public void Leggiddl(string tab, DropDownList ddl, string filtro, string order) // devo leggere la tabella Proposta
	{
		msg = "";
		SQLClass.openaSQLConn(out msg);
		if (msg.Length >= 1)
		{
			Stato("ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"], rosso);
			SQLClass.closeaSQLConn(out msg);
		}
		else
		{
			msg = "";
			s = "SELECT * from " + tab;
			if (filtro.Trim() != "")
				s += " where " + filtro;
			if (order.Trim() != "")
				s += " order by " + order;
			tbl = new DataTable();
			tbl = SQLClass.getfromDSet(s, tab, out msg);
			//SQLClass.closeaSQLConn(out msg);
			if (msg.Length >= 1 || tbl == null)
				Stato("ATTENZIONE: si è verificato un\'errore: " + msg + ". Contattare l'assistenza al numero " + (string)Session["assistenza"], rosso);
			else
			{
				if (tbl.Rows.Count > 0)
				{
					ddl.Items.Clear();
					string ss = ""; s = "";
					for (int i = 0; i < tbl.Rows.Count; i++)
					{
						s = tbl.Rows[i][1] == DBNull.Value ? "" : tbl.Rows[i][1].ToString();
						ss = tbl.Rows[i][0] == DBNull.Value ? "" : tbl.Rows[i][0].ToString();
						ddl.Items.Insert(i, new System.Web.UI.WebControls.ListItem(s, ss));
					}
				}
				else
					Stato("ATTENZIONE: si è verificato un\'errore: non ci sono occorrenze nella tabella " + tab.ToUpper() + ". Contattare l'assistenza al numero " + (string)Session["assistenza"], rosso);
				tbl.Clear();
			}
			tbl = null;
		}
	}

	protected bool RiempiGrid(DataTable tbl)
	{
		try
		{
			//int gvHasRows = gElenco.Rows.Count;
			dgvComm.DataSource = tbl;
			dgvComm.DataBind();
			dgvComm.Visible = true;
		}
		catch (Exception ex)
		{
			Stato("Riscontrato errore durante la ricerca committenze. Errore: " + ex.ToString() + " Avvertire l'amministratore al n. " + (string)Session["assistenza"].ToString(), rosso);
			return (false);
		}
		dgvComm.Visible = true;
		bDel.Visible = false;
		bAdd.Text = "Nuova Richiesta";
		//bConvalida.Visible = false;
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
		if (!dgvComm.Visible) // non è uscita è Home
		{
			modificato = Session["modificato"] != null ? (Session["modificato"].ToString() != "SI" ? false : true) : false;
			if (modificato)
			{
				Stato("ATTENZIONE: dati modificati ma non salvati!", rosso);
				bConferma.Text = "Uscire senza salvare le modifiche!";
				bConferma.Visible = true;
				bAnnulla.Visible = true;
				return;
			}
			Session.Timeout = 30;
			preparacruscotto();
			return;
		}
		Session.Clear();
		Session.Abandon();
		Response.Redirect("Login.aspx");
	}

	protected void preparacruscotto()
	{
		cbMyAccount.Visible = true;
		pGara.Visible = false;
		bConferma.Visible = false;
		bAnnulla.Visible = false;
		dgvComm.Visible = true;
		bUscita.Text = "Uscita";
		showRegia(""); // VISUALIZZA CRUSCOTTO
		bDel.Visible = false;
		//bConvalida.Visible = false;
		//hlRichiesta.Visible = false;
		//hlModulo.Visible = false;
		btdwnloadfacsimile.Visible = false;
		bdwnloadrichiesta.Visible = false;
		sStato.Text = "";
		modificato = false; Session.Add("modificato", "NO");
	}

	protected void bAdd_Click(object sender, EventArgs e) // qunado cliccato devo o salvare o inserire
	{
		if (!checkSession())
		{
			Stato("ATTENZIONE: sessione scaduta. Prego identificarsi.", Color.Red);
			return;
		};
		// se aggiungo devo aggiungere anche alla tabella usergare, se modifico update nextworks e basta!
		if (bAdd.Text == "Salva")
		{
			bUscita.Text = "Le mie richieste";
			// faccio i controlli e poi salvo
			string s = "";
			if (tOgg.Text.Trim().Length == 0 || tOgg.Text.Trim().Length > 254) s += "Ogetto: assente o lunghezza maggiore ai 255 caratteri; ";
			if (tImporto.Text.Trim().Length == 0 || tImporto.Text.Trim().Length > 16) s += "Importo a based'asta nullo o lunghezza testo maggiore di 16 caratteri; ";
			DateTime dt, dt0;
			DateTime.TryParse(tDocumentazione.Text, out dt);
			if (dt.Date.ToShortDateString() == "01/01/1900" || dt.Date < DateTime.Now.Date) s += "Data invio documemtazione non valida; ";
			if (dt <= utenti.limite) s += string.Format("Data invio documentazione deve essere successiva al {0:dd-MM-yyyy}. Eventualmente contattare il servizio assistenza al n. {1}", utenti.limite, Session["assistenza"].ToString());
			
			//DateTime.TryParse(tBando.Text, out dt0);
			//if (dt0.Date.ToShortDateString() == "01/01/1900" || dt0.Date < DateTime.Now.Date) s += "Data presunta pubblicazione bando non valida; ";
			//if (dt > dt0)
			//	s += "data invio documentazione DEVE essere antecedente data presunta bando/invio inviti; ";
			if (tRef.Text.Trim().Length <= 5 || tRef.Text.Trim().Length > 50)
				s += "Nome referente mancante o lunghezza maggiore di 50 caratteri; ";
			if (tRef_Tel.Text.Trim().Length <= 9 || tRef_Tel.Text.Trim().Length > 33)
				s += "Telefono referente incompleto/mancante o lunghezza maggiore 33 caratteri; ";
			if (tRef_Mail.Text.Trim().Length <= 6 || tRef_Mail.Text.IndexOf("@") < 1 || tRef_Mail.Text.Trim().Length > 100)
				s += "Email referente mancante o email non corretta o lunghezza maggiore di 100 caratteri; ";
			if (tRup.Text.Trim().Length <= 9 || tRup.Text.Trim().Length > 99)
				s += "Nome e Cognome del Rup mancante/incompleto (min. 9 caratteri) o lunghezza maggiore 99 caratteri; ";
			if (tUscente.Text.Trim().Length > 150)
				s += "Ditta/Azienda/Rti/Ati lunghezza massima 155 caratteri; ";
			if (s.Trim().Length > 0)
			{
				Stato("ATTENZIONE, dati mancanti. Inserire i seguenti dati: " + s, rosso);
				return;
			}
			if (ddlFinanziamento.SelectedItem.Text == "SI")
			{
				double v;
				double.TryParse(tMop.Text.Trim(), out v);
				tMop.Text = v.ToString();
			}
			string msg = "";
			dt0 = DateTime.Parse("1900-01-01");
			idgara = Session["idgara"] != null ? Session["idgara"].ToString() : "";
			if (idgara != "" && idgara != null) // altrimenti è un nuovo inserimento
			{
				msg = "";
				if (!gara.SalvaProposta(idgara.ToString(), "", utenti.ente, ddlServizio.SelectedValue, tOgg.Text.Trim(), "0", tImporto.Text.Trim(), ddlLotti.SelectedValue, ddlTipologia.SelectedValue, ddlAffidamento.SelectedValue, ddlCriterio.SelectedValue, ddlSettore.SelectedValue, dt0, ddlFinanziamento.SelectedValue, tMop.Text, tRef.Text, tRef_Tel.Text, tRef_Mail.Text, tNote.Text, dt, ddlSoglia.SelectedValue, ddlSistemaAffidamento.SelectedValue, tRup.Text, tUscente.Text, utenti.iduser.ToString(), out msg))
				{
					Stato("Modifica richiesta non andata a buon fine. Prego contattare servizio assistenza al n. " + Session["assistenza"].ToString(), rosso);
					return;
				}
			}
			else // devo aggiungere la nuova gara
			{    // Add(string idu, string codiceente, string ente, string servizio, string ogg, string stato,   string asta, string divisa,                 string tipologia,            string affidamento,           string criterio, string settore, DateTime datapresunta, string finanziata, string mop, string referente, string ref_tel, string ref_mail, string note, DateTime Documentazione, string ddlSoglia, string ddlSistemaAffidamento, out string msg)
				if (!gara.Add(utenti.iduser.ToString(), utenti.ente_ek, utenti.ente, ddlServizio.SelectedValue, tOgg.Text.Trim(), "0", tImporto.Text.Trim(), ddlLotti.SelectedValue, ddlTipologia.SelectedValue, ddlAffidamento.SelectedValue, ddlCriterio.SelectedValue, ddlSettore.SelectedValue, dt0, ddlFinanziamento.SelectedValue, tMop.Text, tRef.Text, tRef_Tel.Text, tRef_Mail.Text, tNote.Text, dt, ddlSoglia.SelectedValue, ddlSistemaAffidamento.SelectedValue, tRup.Text, tUscente.Text, out msg))
				{
					Stato("Inserimento nuova richiesta non andata a buon fine. Prego contattare servizio assistenza al n. " + Session["assistenza"].ToString(), rosso);
					return;
				}
			}
			showRegia(""); // devo refressare la grid
		}
		else // AGGIUNGI -> devo preparare la videata per aggiungere una nuova gara
		{
			if (bAdd.Text == "Nuova Richiesta") // è ovvio che se non è salva è aggiungi
			{
				// preparo ambiene per inserire una nuova richiesta
				pconferma.Visible = false;
				btConvalida.Visible = false;
				bAdd.Text = "Salva";
				bDel.Visible = false;
				Inizializza(); bUscita.Text = "Le mie richieste";
				disabilitaconvalidati(true); // abilito input
				dgvComm.Visible = false;				
				pGara.Visible = true; // se non ci sono gare già caricare entro in modalità edit nuova gara
				bDel.Visible = false;
				cbMyAccount.Visible = false;
				ddlSortExpression.Visible = false;
			
				//tMop.Visible = false;
				//lMop.Visible = false;

				Session.Add("idgara", ""); // nuova gara
				idgara = ""; // nuova gara
				codice = ""; // nuova gara
				/* tolto il 4/10/18
				if (utenti.tipoente_ek != "1" && utenti.tipoente_ek != "")
				{
					lFinanziamentoPAT.Visible = true; ddlFinanziamento.Visible = true;
				}
				else
				{
					lFinanziamentoPAT.Visible = false; ddlFinanziamento.Visible = false;
				}*/
			}
		}
	}

	protected void showRegia(string messaggio)
	{
		if (!checkSession())
		{
			Stato("ATTENZIONE: sessione scaduta. Prego identificarsi.", Color.Red);
			return;
		};
		if (messaggio.Length > 0)
			Stato(messaggio, nero);
		else
			Stato("Dati salvati correttamente. I dati salvati devono essere confermati per poter essere inviati ad APAC. Sarà inviata in automatico un'e-mail con allegati i documenti inerenti la richiesta confermata da inviare tramite pi.tre o pec. Gli stessi documenti possono essere scaricati successivamente alla conferma cliccando sul tasto \"Visualizza\". Per ulteriori informazioni consultare la guida.", nero);

		codice = ""; // piallo codice per non dover modificare gare
		Session.Add("idgara", "");

		sqlfiltro = Session["filtro"].ToString() != null ? Session["filtro"].ToString() : " a.user_ek =\'" + utenti.iduser + "\' ";
		sqlsort = Session["sort"].ToString() != null ? Session["sort"].ToString() : " a.dtc desc ";

		s = "select a.user_ek, a.id, a.titolo, a.importoasta, a.tipologia_ek, left(cast(a.convalida as date),10) as convalidaasdata, ('..\\Moduli\\Scheda_richiesta_fabbisogno_' + cast(a.user_ek as varchar) + '_' + cast(a.id as varchar) + '.pdf') as url, ";
		s += "a.tiposerviziorichiestoinizialmente_ek, a.sistemaaffidamento_ek, a.finanziamentoPAT, a.lotti, a.stato, a.soglia, a.procedura_ek ";
		s += "from nextworks as a ";
		s += "left join lotto as c on a.id = c.lottorichiesta_ek ";
		s += "left join utenti as d on a.user_ek = d.id ";
		s += "where " + sqlfiltro; // a.user_ek=\'" + utenti.iduser + "\' ";
		s += "order by " + sqlsort; // a.dtc desc";

		msg = "";
		tbl = new DataTable();
		tbl = SQLClass.getfromDSet(s, "gareutente", out msg);
		ok = RiempiGrid(tbl);
		if (tbl.Rows.Count > 0)
		{
			pGara.Visible = false;
		}
		cbMyAccount.Visible = true;
		bAdd.Text = "Nuova Richiesta";
		bUscita.Text = "Uscita";
		bAdd.Visible = true;
		ddlSortExpression.Visible = true;

		tbl.Clear();
		tbl = null;
	}
	protected void disabilitaconvalidati(bool disa)
	{
		ddlServizio.Enabled = disa;
		ddlSistemaAffidamento.Enabled = disa;
		tOgg.Enabled = disa;
		tImporto.Enabled = disa;
		ddlSoglia.Enabled = disa;
		ddlLotti.Enabled = disa;
		ddlTipologia.Enabled = disa;
		ddlAffidamento.Enabled = disa;
		ddlCriterio.Enabled = disa;
		ddlSettore.Enabled = disa;
		tDocumentazione.Enabled = disa;
		//tBando.Enabled = disa;
		ddlFinanziamento.Enabled = disa;
		tMop.Enabled = disa;
		tRef.Enabled = disa;
		tRef_Tel.Enabled = disa;
		tRef_Mail.Enabled = disa;
		tNote.Enabled = disa;
		tRup.Enabled = disa;
		tUscente.Enabled = disa;
		//hlRichiesta.Visible = !disa;
		//hlModulo.Visible = !disa;
		btdwnloadfacsimile.Visible = !disa;
		bdwnloadrichiesta.Visible = !disa;		
		return;
	}
	protected void dgvComm_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (!checkSession())
		{
			Stato("ATTENZIONE: sessione scaduta. Prego identificarsi.", Color.Red);
			return;
		};
		bUscita.Text = "Le mie richieste";
		cbMyAccount.Visible = false;
		sStato.Text = "";
		string key = dgvComm.SelectedValue != null ? dgvComm.SelectedValue.ToString() : "";		
		if (key == null || key == "")
		{
			Session.Add("idgara", ""); 
			sStato.Text = "Nessuna gara caricata....";
			return;
		}
		Session.Add("idgara", key); // mi prendo nota della riga selezionata
		VisualizzaGara(key, "");
	}

	protected void VisualizzaGara(string key, string msgdavisualizzare)
	{
		if (key == null || key == "")
		{
			showRegia("ATTEZIONE: problemi con visualizzazione gara!");
			return;
		}
		/* arrivo qui per tre casi:
		1) modifica: allora visualizzo e nonnascondo convalida
		2) convalida: allora visualizza e visualizzo convalida
		3) modifica di gara già convalidata: allora visualizzo e disabilito e nascondo pannello e pulsanti convalida

		*/
		// preparo ambiene
		cbMyAccount.Visible = false;
		dgvComm.Visible = false;
		ddlSortExpression.Visible = false;
		bUscita.Text = "Le mie richieste";
		pGara.Visible = true; // attenzione a non mettere le robe da visualizzare sopra pgare=true..
		modificato = false; Session.Add("modificato", "NO");
		if (utenti.tipoente_ek != "1" && utenti.tipoente_ek != "")
		{
			lFinanziamentoPAT.Visible = true; ddlFinanziamento.Visible = true;
			if (utenti.tipoente_ek == "10" || utenti.tipoente_ek == "11")
			{
				lMop.Visible = true; tMop.Visible = true;
			}
			else
			{
				//lMop.Visible = false; tMop.Visible = false;
			}
		}
		else
		{
			// tolto il 4/10/18
			//lFinanziamentoPAT.Visible = false; ddlFinanziamento.Visible = false;
			//lMop.Visible = false; tMop.Visible = false;
		}
		Gare gare = new Gare();
		tbl = new DataTable();
		tbl = gare.CercaId(key.ToString(), "nextworks", out msg);  // cerco la gara key
		if (msg.Length == 0 && tbl.Rows.Count == 1) // solo modifica: visualizzo dati
		{
			DateTime dt;
			lEnte.Text = tbl.Rows[0]["Ente"] != DBNull.Value ? tbl.Rows[0]["Ente"].ToString() : "";
			ddlServizio.SelectedValue = tbl.Rows[0]["tiposerviziorichiestoinizialmente_ek"] != DBNull.Value ? tbl.Rows[0]["tiposerviziorichiestoinizialmente_ek"].ToString() : "";
			ddlSistemaAffidamento.SelectedValue = tbl.Rows[0]["sistemaaffidamento_ek"] != DBNull.Value ? tbl.Rows[0]["sistemaaffidamento_ek"].ToString() : "";
			tOgg.Text = tbl.Rows[0]["Titolo"] != DBNull.Value ? tbl.Rows[0]["Titolo"].ToString() : "";
			tImporto.Text = tbl.Rows[0]["importoasta"] != DBNull.Value ? string.Format("{0:N2}", double.Parse(tbl.Rows[0]["importoasta"].ToString())) : "";
			//tImporto.Text = tbl.Rows[0]["importoasta"] != DBNull.Value ? tbl.Rows[0]["importoasta"].ToString() : "";
			ddlSoglia.SelectedValue = tbl.Rows[0]["Soglia"] != DBNull.Value ? tbl.Rows[0]["Soglia"].ToString() : "0";
			ddlLotti.SelectedValue = tbl.Rows[0]["lotti"] != DBNull.Value ? tbl.Rows[0]["lotti"].ToString() : "0";
			ddlTipologia.SelectedValue = tbl.Rows[0]["tipologia_ek"] != DBNull.Value ? tbl.Rows[0]["tipologia_ek"].ToString() : "";
			ddlAffidamento.SelectedValue = tbl.Rows[0]["procedura_ek"] != DBNull.Value ? tbl.Rows[0]["procedura_ek"].ToString() : "";
			ddlCriterio.SelectedValue = tbl.Rows[0]["criterioaggiudicazione_ek"] != DBNull.Value ? tbl.Rows[0]["criterioaggiudicazione_ek"].ToString() : "";
			ddlSettore.SelectedValue = tbl.Rows[0]["settoretipologialavoro_ek"] != DBNull.Value ? tbl.Rows[0]["settoretipologialavoro_ek"].ToString() : "21";
			tRup.Text = tbl.Rows[0]["Rup"] != DBNull.Value ? tbl.Rows[0]["Rup"].ToString() : "";
			tUscente.Text = tbl.Rows[0]["Uscente"] != DBNull.Value ? tbl.Rows[0]["Uscente"].ToString() : "";
			tDocumentazione.Text = "";
			//tBando.Text = "";
			dt = DateTime.Now.AddDays(90);
			if (tbl.Rows[0]["DataDocumentazione"] != DBNull.Value)
			{
				DateTime.TryParse(tbl.Rows[0]["DataDocumentazione"].ToString(), out dt);
				tDocumentazione.Text = dt.ToString("dd/MM/yyyy");
			}
			lQuadrimestre.Text = dt.Month <= 4 ? "Primo quadrimestre" : dt.Month <= 8 ? "Secondo quadrimestre" : "Terzo quadrimestre";
			ddlFinanziamento.SelectedValue = tbl.Rows[0]["finanziamentoPAT"] != DBNull.Value ? tbl.Rows[0]["finanziamentoPAT"].ToString() : "";
			bool ok = false;
			if (ddlFinanziamento.SelectedItem.Text == "NO") ok = false; else ok = true;
			//tMop.Visible = ok;
			//lMop.Visible = ok;
			ok = false;
			if (ddlTipologia.SelectedItem.Text != "lavori") ok = true;
			tUscente.Visible = ok;
			lUscente.Visible = ok;
			tMop.Text = tbl.Rows[0]["mop"] != DBNull.Value ? tbl.Rows[0]["mop"].ToString() : "";
			tRef.Text = tbl.Rows[0]["referente"] != DBNull.Value ? tbl.Rows[0]["referente"].ToString() : "";
			tRef_Tel.Text = tbl.Rows[0]["ref_tel"] != DBNull.Value ? tbl.Rows[0]["ref_tel"].ToString() : "";
			tRef_Mail.Text = tbl.Rows[0]["ref_mail"] != DBNull.Value ? tbl.Rows[0]["ref_mail"].ToString() : "";
			tNote.Text = tbl.Rows[0]["note"] != DBNull.Value ? tbl.Rows[0]["note"].ToString() : "";

			if (tbl.Rows[0]["Convalida"] != DBNull.Value) // gara già convalidata
			{
				bAdd.Visible = false; // non si salva nulla se validata
				disabilitaconvalidati(false);
				sStato.Text = "Richiesta convalidata. Non è più possibile apportare modifiche.";
				bDel.Visible = false;
				btConvalida.Visible = false;
				pconferma.Visible = false;
				//bConvalida.Visible = false;
				string path = Server.MapPath("Moduli");
				modificato = false; Session.Add("modificato", "NO"); // a scanso di equivoci
				bdwnloadrichiesta.Visible = true;
				btdwnloadfacsimile.Visible = true;
				tbl.Clear();
				tbl = null;
				gare = null;
				return;
			}
			else
			{
				// da visualizzare e convalidare
				disabilitaconvalidati(true);
				if (msgdavisualizzare.Length > 0)
				{
					Stato(msg, rosso); // da convalidare
					pconferma.Visible = true;
					btConvalida.Visible = true;
				}
				else
				{
					pconferma.Visible = false;
					btConvalida.Visible = false;
				}
				bAdd.Text = "Salva";
				bAdd.Visible = true;
				bDel.Visible = true;
				tbl.Clear();
				tbl = null;
				gare = null;
			}
		}
		else
		{
			Stato("ATTENZIONE: problema con ricerca gara per id. Contattare il servizio assistenza al n. " + Session["assistenza"].ToString(), rosso);
			tbl.Clear();
			tbl = null;
			gare = null;
		}
	}

	protected void bDel_Click(object sender, EventArgs e)
	{
		bConferma.Text = "Confermi la CANCELLAZIONE della richiesta ?";
		bConferma.Visible = true;
		bAnnulla.Visible = true;
	}

	protected void bConferma_Click(object sender, EventArgs e)
	{
		if (!checkSession())
		{
			Stato("ATTENZIONE: sessione scaduta. Prego identificarsi.", Color.Red);
			return;
		};
		if (bConferma.Text == "Uscire senza salvare le modifiche!")
		{
			modificato = false; Session.Add("modificato", "NO");
			preparacruscotto();
			return;
		}
		bUscita.Text = "Le mie richieste";
		bConferma.Visible = false;
		bAnnulla.Visible = false;
		string msg = "";
		codice = ""; // altrimenti continuo....
		idgara = Session["idgara"] != null ? Session["idgara"].ToString() : "";
		if (idgara != "" && idgara != null)
		{
			if (bConferma.Text == "Confermi la CANCELLAZIONE della richiesta ?")
			{
				string specifica = "Cancellazione richiesta";
				if (gara.CancellaProposta(utenti.iduser.ToString(), idgara, specifica, out msg)) // elimino proposta
				{
					sStato.Text = "Proposta di committenza cancellata.";
					pGara.Visible = false;
					showRegia("Richiesta eliminata con successo");
					return;
				}
			}
			else
				Stato("Problemi durante la cancellazione o la conferma della proposta. Contattare il servzio assistenza al n. " + Session["assistenza"].ToString(), rosso);
		}
		else
			Stato("Problemi durante la cancellazione o la conferma della proposta. Contattare il servzio assistenza al n. " + Session["assistenza"].ToString(), rosso);
		return;
	}

	protected void bAnnulla_Click(object sender, EventArgs e)
	{
		bConferma.Visible = false;
		bAnnulla.Visible = false;
	}

	// automatismi e pre impostazioni
	protected void ddlTipologia_TextChanged(object sender, EventArgs e)
	{
		bool ok = false;
		if (ddlTipologia.SelectedItem.Text != "lavori") ok = true;
		tUscente.Visible = ok;
		lUscente.Visible = ok;
		tImporto_TextChanged(this, e);
	}

	protected void tImporto_TextChanged(object sender, EventArgs e)
	{
		modificato = true;  Session.Add("modificato", "SI");
		double dImporto = 0.0;
		Double.TryParse(tImporto.Text, out dImporto);

		// Criterio di aggiudicazione: anche qui se lavori sotto 2.000.000 - prezzo più basso di default; in tutti gli altri casi OEPV
		sStato.Text = "";
		/*switch (ddlTipologia.SelectedValue)
        {
            case "1": //  lavori dai 500.000 ai 2.000.000 negoziata senza bando;  lavori maggiori di 2.000.000 procedura aperta e epv
                if (dImporto >= 2000000) { ddlCriterio.SelectedValue = "2"; ddlAffidamento.SelectedValue = "1"; }
                if (dImporto >= 500000 && dImporto < 2000000) { ddlCriterio.SelectedValue = "1"; ddlAffidamento.SelectedValue = "7"; } // negozoata senza bando
                if (dImporto < 500000) { ddlCriterio.SelectedValue = "1"; ddlAffidamento.SelectedValue = "3"; }
                if (dImporto < 150000 && ddlLotti.SelectedValue == "0" && utenti.id_ente_ek == "11") sStato.Text = "Ricordiamo che potete procedere autonomamente per lavori con importi inferioriori a 150.000€ per gare non suddivise in lotti.";
                break;
            case "2": //servizi e forniture di importo inferiore a 209.000 confronto concorrenziale ex lp. 23 / 90
            case "3":
                if (dImporto <= 209000) { ddlCriterio.SelectedValue = "2"; ddlAffidamento.SelectedValue = "9"; }
                else { ddlCriterio.SelectedValue = "2"; ddlAffidamento.SelectedValue = "1"; }
                break;
            default:
                { ddlCriterio.SelectedValue = "1"; ddlAffidamento.SelectedValue = "1"; }
                break;
        }
		*/
			double impo;
		double.TryParse(tImporto.Text, out impo);
		if (impo == 0)
		{
			Stato("ATTENZIONE: importo non valido. Usare solo la virgola per separare la parte intera dai decimali!", rosso);
			tImporto.Text = "0,0";
		}
		else
			tImporto.Text = string.Format("{0:N2}", impo);
		ddlSoglia.Focus();
	}

	protected void tDocumentazione_TextChanged(object sender, EventArgs e)
	{
		modificato = true; Session.Add("modificato", "SI");
		DateTime dt;
		double dImporto = 0.0;
		Double.TryParse(tImporto.Text, out dImporto);
		DateTime.TryParse(tDocumentazione.Text.Trim(), out dt);
		//DateTime.TryParse(tBando.Text.Trim(), out dt0);
		//int scarto = 60;
		if ((ddlTipologia.SelectedIndex == 1 || ddlTipologia.SelectedIndex == 2) &&
			 dImporto > 209000 && ddlCriterio.SelectedIndex == 1) // scattano i 90gg
		{
			//scarto = 90;
			//Stato("Per Servizi o forniture di importo superiore a 209.000€, con criterio di aggiudicazione \"Offerta economicamente più vantaggio\", serve un tempo sufficientemente ampio (90gg) da consentire all'ente di perfezionare la documentazione di gara secondo le indicazioni di APAC e di conseguenza ad APAC di predisporre e pubblicare il bando di gara.", rosso);
		}

		if (dt.ToString("dd/MM/yyyy") == "01/01/1900")
		{
			dt = DateTime.Now;
			/*if (dt0.ToString("dd/MM/yyyy") != "01/01/0001")
				if (dt0 > DateTime.Now.Date)
					dt = dt0.AddDays(-scarto);
				else
					dt = DateTime.Now;
			else
				if (dt <= DateTime.Now.Date) dt = DateTime.Now;
			*/
			//tDocumentazione.Text = dt.ToString("dd/MM/yyyy");
		}
		else
		{
			if (dt < DateTime.Now.Date)
				Stato("ATTENZIONE: la produzione della documentazione non può essere antecedente la data odierna!", rosso);
			else
			{
				//if (scarto > 60)
					//Stato("Per Servizi o forniture di importo superiore a 209.000€, con criterio di aggiudicazione \"Offerta economicamente più vantaggio\", serve un tempo sufficientemente ampio (90gg) da consentire all'ente di perfezionare la documentazione di gara secondo le indicazioni di APAC e di conseguenza ad APAC di predisporre e pubblicare il bando di gara.", rosso);
				//else
					sStato.Text = "";
			}
		}
		/*if (tBando.Text.Trim() == "")
		{
			if (dt > DateTime.Now.AddDays(-scarto))
				tBando.Text = dt.Date.AddDays(scarto).ToString("dd/MM/yyyy");
			else
				tBando.Text = DateTime.Now.AddDays(scarto).ToString("dd/MM/yyyy");
			tBando_TextChanged(this, new EventArgs());
		}
		else
		{
		if (dt > dt0)
			{
				Stato("ATTENZIONE: data invio documentazione per avvio procedura DEVE essere antecedente alla data bando/invio inviti... ", rosso);
			}
			else
			{
				if (dt < DateTime.Now.Date)
					Stato("ATTENZIONE: la produzione della documentazione non può essere inferiore alla data odierna!", rosso);
				else
				{
					if (scarto > 60)
						Stato("Per Servizi o forniture di importo superiore a 209.000€, con criterio di aggiudicazione \"Offerta economicamente più vantaggio\", serve un tempo sufficientemente ampio (90gg) da consentire all'ente di perfezionare la documentazione di gara secondo le indicazioni di APAC e di conseguenza ad APAC di predisporre e pubblicare il bando di gara.", rosso);
					else
						sStato.Text = "";
				}
				//return;
			}
		}
		*/
		lQuadrimestre.Text = dt.Month <= 4 ? "Primo quadrimestre" : dt.Month <= 8 ? "Secondo quadrimestre" : "Terzo quadrimestre";
		tDocumentazione.Text = dt.ToString("dd/MM/yyyy");
		if (ddlFinanziamento.Visible) ddlFinanziamento.Focus(); else tNote.Focus();
	}
	/*
	protected void tBando_TextChanged(object sender, EventArgs e)
	{
		modificato = true;
		DateTime dt, dt0;
		DateTime.TryParse(tDocumentazione.Text.Trim(), out dt);
		DateTime.TryParse(tBando.Text.Trim(), out dt0);
		int scarto = 60;
		double dImporto = 0.0;
		Double.TryParse(tImporto.Text, out dImporto);
		if ((ddlTipologia.SelectedIndex == 1 || ddlTipologia.SelectedIndex == 2) &&
			 dImporto > 209000 && ddlCriterio.SelectedIndex == 1) // scattano i 90gg
		{
			scarto = 90;
		}
		if (dt0.ToString("dd/MM/yyyy") == "01/01/0001" || tBando.Text.Trim() == "")
		{
			dt0 = dt.AddDays(scarto);
			if (dt0 <= DateTime.Now.Date) dt0 = DateTime.Now;
		}
		lQuadrimestre.Text = dt0.Month <= 4 ? "Primo quadrimestre" : dt0.Month <= 8 ? "Secondo quadrimestre" : "Terzo quadrimestre";
		if (dt > dt0)
			Stato("ATTENZIONE: data invio documentazione per avvio procedura DEVE essere antecedente alla data presunta bando/invio inviti... ", Color.Red);
		else
			if (dt0.AddDays(-scarto) < dt)
		{
			Stato("ATTENZIONE! il lasso di tempo intercorrente dalla data presunta di invio della documentazione alla data presunta di pubblicazione del bando potrebbe non essere sufficientemente ampio da consentire all'ente di perfezionare la documentazione di gara secondo le indicazioni di APAC e di conseguenza ad APAC di predisporre e pubblicare il bando di gara.", Color.Red);
		}
		else
				if (dt0 < DateTime.Now.Date)
			Stato("ATTENZIONE: la data pubblicazione bando/invio inviti non può essere inferiore alla data odierna!", rosso);
		else
		{
			if (scarto > 60)
				Stato("Per Servizi o forniture di importo superiore a 209.000€, con criterio di aggiudicazione \"Offerta economicamente più vantaggio\", serve un tempo sufficientemente ampio (90gg) da consentire all'ente di perfezionare la documentazione di gara secondo le indicazioni di APAC e di conseguenza ad APAC di predisporre e pubblicare il bando di gara.", rosso);
			else
				sStato.Text = "";
		}
		tBando.Text = dt0.ToString("dd/MM/yyyy");
	}
	*/
	protected void cbMyAccount_Click(object sender, EventArgs e)
	{
		if (!checkSession())
		{
			Stato("ATTENZIONE: sessione scaduta. Prego identificarsi.", Color.Red);
			return;
		};
		Session["arrivo_da"] = "home";
		Session.Timeout = 30;
		Response.Redirect("mydata.aspx");
	}
	protected void Stato(string s, Color c)
	{
		sStato.Text = s;
		if (c == null) c = Color.Black;
		sStato.ForeColor = c;
	}

	protected void dgvComm_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.Cells[1].Text.Trim() == "&nbsp;" && e.Row.RowIndex >= 0)
		{
			string go = dgvComm.DataKeys[e.Row.RowIndex].Value.ToString();
			e.Row.Cells[1].Text = "<a href = \'menu.aspx?c=" + go + "\' > Conferma</ a >";
		}
		if (e.Row.Cells[4].Text.Trim() != "&nbsp;" && e.Row.RowIndex >= 0) e.Row.Cells[4].Text = stato[(int.Parse(e.Row.Cells[4].Text))];
	}

	protected void btConvalida_Click(object sender, EventArgs e)
	{
		idgara = Session["idgara"] != null ? Session["idgara"].ToString().Trim() : "";
		if (!checkSession() || idgara == null || idgara == "")
		{
			Stato("ATTENZIONE: sessione scaduta. Prego identificarsi.", Color.Red);
			return;
		};
		bUscita.Text = "Le mie richieste";
		// faccio i controlli e poi salvo
		string s = "";
		if (tOgg.Text.Trim().Length == 0 || tOgg.Text.Trim().Length > 254) s += "Ogetto: assente o lunghezza maggiore ai 255 caratteri; ";
		if (tImporto.Text.Trim().Length == 0 || tImporto.Text.Trim().Length > 16) s += "Importo a based'asta nullo o lunghezza testo maggiore di 16 caratteri; ";
		DateTime dt, dt0=DateTime.Now;
		DateTime.TryParse(tDocumentazione.Text, out dt);
		if (dt.Date.ToShortDateString() == "01/01/1900" || dt.Date < DateTime.Now.Date) s += "Data invio documemtazione non valida; ";
		if (dt <= utenti.limite) s += string.Format("Data invio documentazione deve essere successiva al {0:dd-MM-yyyy}. Eventualmente contattare il servizio assistenza al n. {1}", utenti.limite, Session["assistenza"].ToString());

		//DateTime.TryParse(tBando.Text, out dt0);
		//if (dt0.Date.ToShortDateString() == "01/01/1900" || dt0.Date < DateTime.Now.Date) s += "Data presunta pubblicazione bando non valida; ";
		//if (dt > dt0)
		//	s += "data invio documentazione DEVE essere antecedente data presunta bando/invio inviti; ";
		if (tRef.Text.Trim().Length <= 5 || tRef.Text.Trim().Length > 50)
			s += "Nome referente mancante o lunghezza maggiore di 50 caratteri; ";
		if (tRef_Tel.Text.Trim().Length <= 9 || tRef_Tel.Text.Trim().Length > 33)
			s += "Telefono referente mancante o lunghezza maggiore 33 caratteri; ";
		if (tRef_Mail.Text.Trim().Length <= 6 || tRef_Mail.Text.IndexOf("@") < 1 || tRef_Mail.Text.Trim().Length > 100)
			s += "Email referente mancante o email non corretta o lunghezza maggiore di 100 caratteri; ";
		if (tRup.Text.Trim().Length <= 9 || tRup.Text.Trim().Length > 99)
			s += "Indicazione del Rup mancante o lunghezza maggiore 99 caratteri; ";
		if (tUscente.Text.Trim().Length > 150)
			s += "Ditta/Azienda/Rti/Ati lunghezza massima 150 caratteri; ";
		if (s.Trim().Length > 0)
		{
			Stato("ATTENZIONE, dati mancanti. Inserire i seguenti dati: " + s, rosso);
			return;
		}

		id = Session["iduser"] != null ? Session["iduser"].ToString().Trim() : "";
		btConvalida.Enabled = false;
		bAdd.Enabled = false;

		string msg = "";
		Gare gare = new Gare();
		string SaveLocation = Server.MapPath("Moduli") + "\\";
		string nome_base = "Scheda_richiesta_fabbisogno";
		string nf = SaveLocation + nome_base;
		msg = "";

		if (!gare.SalvaProposta(idgara.ToString(), "", utenti.ente, ddlServizio.SelectedValue, tOgg.Text.Trim(), "0", tImporto.Text.Trim(), ddlLotti.SelectedValue, ddlTipologia.SelectedValue, ddlAffidamento.SelectedValue, ddlCriterio.SelectedValue, ddlSettore.SelectedValue, dt0, ddlFinanziamento.SelectedValue, tMop.Text, tRef.Text, tRef_Tel.Text, tRef_Mail.Text, tNote.Text, dt, ddlSoglia.SelectedValue, ddlSistemaAffidamento.SelectedValue, tRup.Text, tUscente.Text, utenti.iduser.ToString(), out msg))
		{
			Stato("Modifica richiesta non andata a buon fine. Prego contattare servizio assistenza al n. " + Session["assistenza"].ToString(), rosso);
			return;
		}
		if (gare.Convalida(idgara, id, out msg)) // 
		{
			Stato("registrazione eseguita con successo.", rosso);
			pGara.Visible = true;
			bUscita.Text = "Le mie richieste";
			cbMyAccount.Visible = false;
			sStato.Text = "";
			modificato = false; Session.Add("modificato", "NO");
			btConvalida.Enabled = true;
			bAdd.Enabled = true;
			MemoryStream ms = new MemoryStream();
			string dest = nf + "_" + id + "_" + idgara + ".pdf";
			try
			{
				if (System.IO.File.Exists((nf + ".pdf"))) // se esiste il master layout....
				{
					PdfReader pdfReader = new PdfReader(nf + ".pdf");
					PdfStamper pdfStamper = new PdfStamper(pdfReader, ms);  // inn ram....
					AcroFields form = pdfStamper.AcroFields;
					//string[] fill = { "Questo è un testo", "1.344.787,22", "24/02/2017" };
					int i = 0;
					string[] keys = form.Fields.Keys.ToArray();
					foreach (string k in keys)  // rinomino i campi
						form.RenameField(k, "Campo_" + (i++).ToString());
					keys = form.Fields.Keys.ToArray();
					DateTime.TryParse(tDocumentazione.Text, out dt);
					s = dt.ToString("yyyy");  // la data deve essere giusta.... altrimenti errore
					s += "/" + (dt.Month < 5 ? "primo quadrimestre" : dt.Month > 8 ? "terzo quadrimestre" : "secondo quadrimestre");
					form.SetField(keys[0], s); form.SetFieldProperty(keys[0], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[1], DateTime.Now.Date.ToString("dd-MM-yyyy")); form.SetFieldProperty(keys[1], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[2], utenti.ente); form.SetFieldProperty(keys[2], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[3], ddlServizio.SelectedItem.Text); form.SetFieldProperty(keys[3], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[4], ddlTipologia.SelectedItem.Text); form.SetFieldProperty(keys[4], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[5], ddlSistemaAffidamento.SelectedItem.Text); form.SetFieldProperty(keys[5], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[6], tOgg.Text.Trim()); form.SetFieldProperty(keys[6], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[7], String.Format("{0,12:N2}", Double.Parse(tImporto.Text))); form.SetFieldProperty(keys[7], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[8], ddlSoglia.SelectedItem.Text); form.SetFieldProperty(keys[8], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[9], ddlLotti.SelectedItem.Text); form.SetFieldProperty(keys[9], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[10], ddlAffidamento.SelectedItem.Text); form.SetFieldProperty(keys[10], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[11], ddlCriterio.SelectedItem.Text); form.SetFieldProperty(keys[11], "setfflags", PdfFormField.FF_READ_ONLY, null);
					//form.SetField(keys[10], ddlSettore.SelectedItem.Text); form.SetFieldProperty(keys[10], "setfflags", PdfFormField.FF_READ_ONLY, null);
					//(keys[10], " "); form.SetFieldProperty(keys[10], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[12], tDocumentazione.Text); form.SetFieldProperty(keys[12], "setfflags", PdfFormField.FF_READ_ONLY, null);
					//form.SetField(keys[13], tBando.Text); form.SetFieldProperty(keys[13], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[13], ddlFinanziamento.Visible ? ddlFinanziamento.SelectedItem.Text : " "); form.SetFieldProperty(keys[13], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[14], ddlFinanziamento.SelectedItem.Text == "NO" ? "" : tMop.Text.Trim() == "0" ? "" : tMop.Text.Trim()); form.SetFieldProperty(keys[14], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[15], tRup.Text); form.SetFieldProperty(keys[15], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[16], tNote.Text); form.SetFieldProperty(keys[16], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[17], tRef.Text); form.SetFieldProperty(keys[17], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[18], tRef_Tel.Text); form.SetFieldProperty(keys[18], "setfflags", PdfFormField.FF_READ_ONLY, null);
					form.SetField(keys[19], tRef_Mail.Text); form.SetFieldProperty(keys[19], "setfflags", PdfFormField.FF_READ_ONLY, null);
					/*for (int n = 0; n < i; n++)
					{
						form.SetField(keys[n], fill[n]);
						form.SetFieldProperty(keys[n], "setfflags", PdfFormField.FF_READ_ONLY, null);
					}*/
					pdfStamper.FormFlattening = false;
					pdfStamper.Writer.CloseStream = false;
					pdfStamper.Close();
					pdfReader.Close();
					ms.Position = 0;
					// ok inoltro la richiesta
					MailMessage mail = new MailMessage();
					mail.From = new MailAddress("apac@provincia.tn.it", "Raccolta fabbisogno");
					string[] achi = { utenti.mail };
					mail.To.Add(new MailAddress(utenti.mail));
					string[] achicc = { "tiziano.donati@provincia.tn.it" };
					mail.CC.Clear();
					if (achicc != null)
					{
						foreach (string ac in achicc)
							mail.CC.Add(new MailAddress(ac)); // è una lista
					}
					// AGGIUNGO FILE RICHIESTA ACCOMPAGNATORIA
					s = SaveLocation + "fac_simile_lettera_richiesta.rtf";
					Attachment data = new Attachment(s, MediaTypeNames.Application.Octet);
					ContentDisposition disposition = data.ContentDisposition;
					disposition.CreationDate = System.IO.File.GetCreationTime(s);
					disposition.ModificationDate = System.IO.File.GetLastWriteTime(s);
					disposition.ReadDate = System.IO.File.GetLastAccessTime(s);
					mail.Attachments.Add(data);
					// System.Net.Mime.MediaTypeNames.Application.Pdf
					Attachment data1 = new Attachment(ms, "Richiesta.pdf", "application/pdf");
					mail.Attachments.Add(data1);

					mail.Subject = "Richiesta fabbisogno.";
					mail.Body = "Buongiorno gent.le " + utenti.nome + " " + utenti.cognome + "\n\n";
					mail.Body += "    in allegato, trovera\' due moduli. Il primo (fac_simile_lettera_richiesta.rtf) dovra\' essere personalizzato con la carta\n";
					mail.Body += "intestata dell'Ente/Struttura richiedente e la firma del legale rappresentante. Il corpo della lettera puo\' essere integrato con\n";
					mail.Body += "ulteriori dettagli, ritenuti importanti ai fini della pianificazione delle attivita\' di APAC oltre che per proprie esigenze.\n";
					mail.Body += "Il secondo(scheda_richiesta_fabbisogno.pdf) contiene le informazioni relative alla procedura di gara per la quale è stato richiesto di avvalersi di APAC in qualita\' di centrale di committenza o consulente. Entrambe i moduli vanno verificati, firmati digitalmente ed inoltrati attraverso la via formale(Pit3 o pec). I fabbisogni che verranno inoltrati non rispettando le regole di cui sopra non saranno ammessi alla programmazione di APAC.\n";
					mail.Body += "E' gradita l'occasione per porgerLe cordiali saluti.\n\n";
					mail.Body += "Grazie.\n\n\n";
					mail.Body += "Allegato: \tfac_simile_lettera_richiesta.rtf;\n";
					mail.Body += "          \tscheda_richiesta_fabbisogno_" + id + "_" + idgara + ".pdf\n";
					SmtpClient mailSender = new SmtpClient("smtp.gmail.com", 587);
					mailSender.UseDefaultCredentials = true;
					mailSender.EnableSsl = true;
					mailSender.Credentials = new System.Net.NetworkCredential("provinciaautonomatn@gmail.com", "ProvinciaPAT");
					try
					{
						mailSender.Send(mail);
					}
					catch (Exception ex)
					{
						Stato("ATTENZIONE: conferma prenotazione non inviata correttamente. Contatare il servizio assistenza al n. " + Session["assistenza"].ToString() + "  ERR:" + ex.ToString(), rosso);
						return;
					}
				}
				else
				{
					Stato("ERRORE: file di layout non esiste o danneggiato!.", Color.Red);
					return;
				}
			}
			catch (Exception ex)
			{
				Stato("Problemi salvataggio richiesta precompilata: " + ex.ToString() + "  Contattare il servzio assistenza al n. " + Session["assistenza"].ToString(), rosso);
				btConvalida.Enabled = true;
				bAdd.Enabled = true;
				return;
			}
		}
		else
		{
			Stato("Conferma non eseguita. Contattare il servizio assistenza al numero " + Session["assistenza"].ToString() + ".  ERR: " + msg, rosso);
			pGara.Visible = true;
			bUscita.Text = "Le mie richieste";
			cbMyAccount.Visible = false;
			modificato = false; Session.Add("modificato", "NO");
			return;
		}

		// pulizia file su server ???, si potrebbe togliere
		try
		{
			System.IO.File.Delete(nf + "_" + id + "_" + idgara + ".pdf");
		}
		catch (IOException ex)
		{
			string m = ex.ToString();
		}

		btConvalida.Enabled = true;
		bAdd.Enabled = true;
		showRegia("");
	}
	protected void bcessa_Click(object sender, EventArgs e)
	{
		btConvalida.Visible = false;
	}

	protected void ddlFinanziamento_SelectedIndexChanged(object sender, EventArgs e)
	{
		bool ok = false;
		//if (ddlFinanziamento.SelectedItem.Text == "NO") ok = false; else ok = false;
		if (ok && (utenti.tipoente_ek == "10" || utenti.tipoente_ek == "11"))
		{
			//lMop.Visible = ok; tMop.Visible = ok;
		}
		else
		{
			//lMop.Visible = false; tMop.Visible = false;
		}
	}
	protected void bdwnloadrichiesta_Click(object sender, EventArgs e)
	{
		if (!checkSession())
		{
			Stato("ATTENZIONE: sessione scaduta. Prego identificarsi.", Color.Red);
			ShowPopUpMsg(sStato.Text);
			return;
		};
		idgara = Session["idgara"] != null ? Session["idgara"].ToString().Trim() : "";
		id = Session["iduser"] != null ? Session["iduser"].ToString().Trim() : "";
		string SaveLocation = Server.MapPath("Moduli") + "\\";
		string nome_base = "Scheda_richiesta_fabbisogno";
		string nf = SaveLocation + nome_base;
		// cerchiamo di pulire il server ?!?!?! da file che non serviranno più... ma si potrebbe anche togliere
		try
		{
			System.IO.File.Delete(nf + "_" + utenti.iduser.ToString().Trim() + "_" + idgara.ToString() + ".pdf");
		}
		catch (IOException ex)
		{
			string m = ex.ToString();
		}
		string dest = nf + "_" + utenti.iduser.ToString().Trim() + "_" + idgara.ToString() + ".pdf";
		MemoryStream ms = new MemoryStream();
		try
		{
			if (System.IO.File.Exists((nf + ".pdf")))
			{
				PdfReader pdfReader = new PdfReader(nf + ".pdf");
				PdfStamper pdfStamper = new PdfStamper(pdfReader, ms);
				AcroFields form = pdfStamper.AcroFields;
				int i = 0;
				string[] keys = form.Fields.Keys.ToArray();
				foreach (string k in keys)  // rinomino i campi
					form.RenameField(k, "Campo_" + (i++).ToString());
				keys = form.Fields.Keys.ToArray();
				//form = pdfStamper.AcroFields; // rileggo i nomi dei campi
				string s = "";
				DateTime dt;
				DateTime.TryParse(tDocumentazione.Text, out dt);
				s = dt.ToString("yyyy");  // la data deve essere giusta.... altrimenti errore
				s += "/" + (dt.Month < 5 ? "primo quadrimestre" : dt.Month > 8 ? "terzo quadrimestre" : "secondo quadrimestre");
				form.SetField(keys[0], s); form.SetFieldProperty(keys[0], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[1], DateTime.Now.Date.ToString("dd-MM-yyyy")); form.SetFieldProperty(keys[1], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[2], utenti.ente); form.SetFieldProperty(keys[2], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[3], ddlServizio.SelectedItem.Text); form.SetFieldProperty(keys[3], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[4], ddlTipologia.SelectedItem.Text); form.SetFieldProperty(keys[4], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[5], ddlSistemaAffidamento.SelectedItem.Text); form.SetFieldProperty(keys[5], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[6], tOgg.Text.Trim()); form.SetFieldProperty(keys[6], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[7], String.Format("{0,12:N2}", Double.Parse(tImporto.Text))); form.SetFieldProperty(keys[7], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[8], ddlSoglia.SelectedItem.Text); form.SetFieldProperty(keys[8], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[9], ddlLotti.SelectedItem.Text); form.SetFieldProperty(keys[9], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[10], ddlAffidamento.SelectedItem.Text); form.SetFieldProperty(keys[10], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[11], ddlCriterio.SelectedItem.Text); form.SetFieldProperty(keys[11], "setfflags", PdfFormField.FF_READ_ONLY, null);
				//form.SetField(keys[10], ddlSettore.SelectedItem.Text); form.SetFieldProperty(keys[10], "setfflags", PdfFormField.FF_READ_ONLY, null);
				//(keys[10], " "); form.SetFieldProperty(keys[10], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[12], tDocumentazione.Text); form.SetFieldProperty(keys[12], "setfflags", PdfFormField.FF_READ_ONLY, null);
				//form.SetField(keys[13], tBando.Text); form.SetFieldProperty(keys[13], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[13], ddlFinanziamento.Visible ? ddlFinanziamento.SelectedItem.Text : " "); form.SetFieldProperty(keys[13], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[14], ddlFinanziamento.SelectedItem.Text == "NO" ? "" : tMop.Text.Trim() == "0" ? "" : tMop.Text.Trim()); form.SetFieldProperty(keys[14], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[15], tRup.Text); form.SetFieldProperty(keys[15], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[16], tNote.Text); form.SetFieldProperty(keys[16], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[17], tRef.Text); form.SetFieldProperty(keys[17], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[18], tRef_Tel.Text); form.SetFieldProperty(keys[18], "setfflags", PdfFormField.FF_READ_ONLY, null);
				form.SetField(keys[19], tRef_Mail.Text); form.SetFieldProperty(keys[19], "setfflags", PdfFormField.FF_READ_ONLY, null);
				/*for (int n = 0; n < i; n++)
				{
					form.SetField(keys[n], fill[n]);
					form.SetFieldProperty(keys[n], "setfflags", PdfFormField.FF_READ_ONLY, null);
				}*/
				pdfStamper.FormFlattening = false;
				pdfStamper.Close();
				pdfReader.Close();
				DownloadAsPDF(ms, "Richiesta.pdf");
			}
			else
				Stato("ATTENZIONE: problema lettura layout richiesta... Contattare il servizio assistenza al numero " + Session["assistenza"].ToString(), Color.Red);
		}
		catch (Exception ex)
		{
			Stato("Problemi salvataggio richiesta precompilata: " + ex.ToString() + "  Contattare il servzio assistenza al n. " + Session["assistenza"].ToString(), rosso);
			return;
		}
		showRegia("");
	}

	private void DownloadAsPDF(MemoryStream ms, string nome_file)
	{
		Response.Clear();
		Response.ClearContent();
		Response.ClearHeaders();
		Response.ContentType = "application/pdf";
		Response.AppendHeader("Content-Disposition", "attachment; filename=" + nome_file);
		Response.AddHeader("Pragma", "no-cache");
		Response.AddHeader("Cache-Control", "no-cache");
		Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
		// Sends the response buffer
		//Response.OutputStream.Flush();
		//Response.OutputStream.Close();
		// Prevents any other content from being sent to the browser
		//Response.SuppressContent = true;
		// Directs the thread to finish, bypassing additional processing
		try
		{
			HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
			HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
			HttpContext.Current.ApplicationInstance.CompleteRequest();
			// Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
			//HttpContext.Current.Response.End();
			//Response.End();
			Thread.Sleep(1);
		}
		catch (Exception ex)
		{
			Stato("Errore nel mandare il registro al client: " + ex.ToString(), Color.Red);
		}
		finally
		{
			/*
			//Sends the response buffer
			HttpContext.Current.Response.Flush();
			// Prevents any other content from being sent to the browser
			HttpContext.Current.Response.SuppressContent = true;
			//Directs the thread to finish, bypassing additional processing
			HttpContext.Current.ApplicationInstance.CompleteRequest();
			*/
			//Suspends the current thread
		}
	}

	protected void btdwnloadfacsimile_Click(object sender, EventArgs e)
	{
		string dest = "~\\Moduli\\fac_simile_lettera_richiesta.rtf";
		Response.Clear();
		Response.ClearContent();
		Response.ClearHeaders();
		Response.ContentType = "Application/msword";
		Response.AppendHeader("Content-Disposition", "attachment; filename=" + dest);
		Response.TransmitFile(Server.MapPath(dest));
		//Response.AddHeader("Pragma", "no-cache");
		//Response.AddHeader("Cache-Control", "no-cache");
		//Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
		// Sends the response buffer
		//Response.OutputStream.Flush();
		//Response.OutputStream.Close();
		// Prevents any other content from being sent to the browser
		//Response.SuppressContent = true;
		// Directs the thread to finish, bypassing additional processing
		try
		{
			HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
			HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
			HttpContext.Current.ApplicationInstance.CompleteRequest();
			// Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
			//HttpContext.Current.Response.End();
			//Response.End();
			Thread.Sleep(1);
		}
		catch (Exception ex)
		{
			Stato("Errore nel mandare il registro al client: " + ex.ToString(), Color.Red);
		}
		finally
		{
			/*
			//Sends the response buffer
			HttpContext.Current.Response.Flush();
			// Prevents any other content from being sent to the browser
			HttpContext.Current.Response.SuppressContent = true;
			//Directs the thread to finish, bypassing additional processing
			HttpContext.Current.ApplicationInstance.CompleteRequest();
			*/
			//Suspends the current thread
		}
		//Response.Redirect(dest);
	}

	protected void bHelp_Click(object sender, EventArgs e)
	{
		Session.Timeout = 30;
	}

	protected void ddlSortExpression_SelectedIndexChanged(object sender, EventArgs e)
	{
		sqlsort = ddlSortExpression.SelectedValue.ToString();
		Session.Add("sort", sqlsort);
		showRegia("Ordinamento richieste per " + ddlSortExpression.SelectedItem.ToString());
	}
}
