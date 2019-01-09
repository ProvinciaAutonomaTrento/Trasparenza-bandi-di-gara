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
using System.Drawing;

public partial class _Default : Page
{
    public string TEST = "NO";
    public DataSet DSet = new DataSet();
    public SqlConnection sqlcnn = null;
    public ConnessioneSQL SQLClass = new ConnessioneSQL();
    public SqlDataAdapter SQLda = null; // new SqlDataAdapter();
    public SqlCommand SQLc = null; // new SqlCommand();
    public string strSelect = "";
    public DataSet ds = new DataSet();
    public string formatoeuro = "###.###.###";
    public string formatodata = "dd-MM-yyyy";
    public bool loggato;
    public string ms = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA....
        {
            string par = Request.QueryString["loggato"];
            loggato = par == "si" ? true : false;  
            Session.Add("arrivo_da", (string)"default"); // vediamo se lo fa ogni volta che carico la pagina o solo la prima volta

            ddlstato.Items.Insert(0, "");
            ddlstato.Items.Insert(1, "bandita");
            ddlstato.Items.Insert(2, "in corso");
            ddlstato.Items.Insert(3, "aggiudicata");
            ddlstato.Items.Insert(4, "stipula del contratto");
			ddlstato.Items.Insert(5, "contratto stipulato"); 
			ddlstato.Items.Insert(6, "procedura conclusa");
			ddlstato.Items.Insert(7, "revocata/annullata");

			ddltipo.Items.Insert(0, "");
            ddltipo.Items.Insert(1, "forniture");
            ddltipo.Items.Insert(2, "lavori");
            ddltipo.Items.Insert(3, "servizi");
            ddltipo.Items.Insert(4, "concorso di idee/progettazione");

            /*strSelect = "SELECT richieste.ente ";
            strSelect += "FROM(proposta RIGHT JOIN(tipoente RIGHT JOIN(ElencoStrutture RIGHT JOIN((richieste LEFT JOIN Datigara ON richieste.id = Datigara.Richiesta_EK) LEFT JOIN Lotto ON richieste.id = Lotto.Lottorichiesta_EK) ON ElencoStrutture.Struttura = richieste.ente) ON tipoente.id = ElencoStrutture.TipoEnte_EK) ON proposta.id = richieste.proposta_EK) LEFT JOIN Datimonitoraggio ON (Lotto.Lottorichiesta_EK = Datimonitoraggio.IDgara_EK) AND(Lotto.Lotto = Datimonitoraggio.Lotto) ";
            strSelect += "WHERE(((Datimonitoraggio.ultimafaseconclusa) > 3)) and web = 1 ";
            strSelect += "GROUP BY richieste.ente, richieste.proposta_EK, richieste.idonea, richieste.Revocata, Lotto.RevocataAnnullata, richieste.Ritirata, Datigara.Soprasogliacomunitaria ";
            strSelect += "HAVING (((richieste.proposta_EK) = 2) AND ((richieste.idonea) = 0 Or (richieste.idonea) Is Null) AND ((Lotto.RevocataAnnullata) = 0 Or (Lotto.RevocataAnnullata) Is Null) AND ((richieste.Ritirata) = 0 Or (richieste.Ritirata) Is Null) ) ";
            strSelect += "ORDER BY richieste.ente";
            */
            strSelect = "SELECT richieste.ente, Lotto.web ";
            strSelect += "FROM(proposta RIGHT JOIN(tipoente RIGHT JOIN(ElencoStrutture RIGHT JOIN((richieste LEFT JOIN Datigara ON richieste.id = Datigara.Richiesta_EK) LEFT JOIN Lotto ON richieste.id = Lotto.Lottorichiesta_EK) ON ElencoStrutture.Struttura = richieste.ente) ON tipoente.id = ElencoStrutture.TipoEnte_EK) ON proposta.id = richieste.proposta_EK) LEFT JOIN Datimonitoraggio ON (Lotto.Lottorichiesta_EK = Datimonitoraggio.IDgara_EK) AND(Lotto.Lotto = Datimonitoraggio.Lotto) ";
            strSelect += "GROUP BY richieste.ente, Lotto.web, richieste.proposta_EK, richieste.idonea, richieste.Revocata, Lotto.RevocataAnnullata, richieste.Ritirata ";
            strSelect += "HAVING (((Lotto.web) = 1) AND ((richieste.proposta_EK) = 2) AND ((richieste.idonea) = 0 Or(richieste.idonea) Is Null) AND ((richieste.Ritirata) = 0 Or (richieste.Ritirata) Is Null)) ";
            strSelect += "ORDER BY richieste.ente";
			sqlcnn = SQLClass.openaSQLConn(out ms);  // creo e apro la connessione
			if (sqlcnn == null)
				Response.Redirect("manutenzione.aspx");
			if (sqlcnn.State == ConnectionState.Open)
            {
                if (getdata(strSelect, ds, "Enti", sqlcnn) > 0)  // carico gli enti e li aggiungo alla ddlEnti 
                {
                    // tStato.Text = string.Format("Nome tabella letta: {0}, n. righe: {1}", ds.Tables[0].TableName, ds.Tables[0].Rows.Count);
                    ddlente.Items.Insert(0, ""); // inserisco uno spazio
                    for (int i = 0; i < ds.Tables["enti"].Rows.Count; i++)
                        ddlente.Items.Insert(i+1, ds.Tables["enti"].Rows[i]["ente"].ToString());
                }
            }
            sqlcnn.Close();
            //tStato.Text = string.Format("Benvenuto {0}.", (Environment.UserDomainName + "\\" + Environment.UserName));
            cbGo_Click(this, new EventArgs());
        }
        if (loggato)
        {
             Login.Text = "Logout";
        }
    }

    protected void cbGo_Click(object sender, EventArgs e)
    {
        string s = "";
        sqlcnn = SQLClass.openaSQLConn(out ms);  // creo e apro la connessione
        if (sqlcnn.State != ConnectionState.Open)
        {
            tStato.Text = "Connessione non aperta: " + ms;
            return;
        }

        int rr = 0;
        SQLda = new SqlDataAdapter();
        SQLc = new SqlCommand();
        SQLc.Parameters.Clear();
        SQLc.Connection = sqlcnn;
        SQLda.SelectCommand = SQLc;
        SQLc.CommandText = "select * from manutenzione where attivo ='1'";
        s = "";
        try
        {
            SQLda.Fill(ds, "manutenzione");
            rr = ds.Tables["manutenzione"].Rows.Count;
            if ( rr > 0 ) s = ds.Tables["manutenzione"].Rows[0]["pagina"].ToString();
        }
        catch (Exception ex)
        {
            tStato.Text = string.Format("Error: Failed to retrieve the required data from the DataBase.\n{0}", ex.Message);
            return;
        }      
            
        if (s.Length > 10 &&  TEST != "SI" && TEST != "NI")
        {
            sqlcnn.Close();
            Response.Redirect(s);
        }
        string f = "";

        f += cbContenzioso.Checked ? " contenzioso = 1 and " : " (contenzioso = 0 or contenzioso is null) and ";
        /*if ( cbContenzioso.Checked)
            f += " contenzioso = 1 and ";
        else
            f += " contenzioso = 0 and ";*/

        if (ddltipo.Text != null)
        {
            switch (ddltipo.Text)
            {
                case "forniture":
                    f += " classificazione = 'forniture' and "; break;
                case "lavori":
                    f += " classificazione = 'lavori' and "; break;
                case "servizi":
                    f += " classificazione = 'servizi' and "; break;
                case "concorso di idee/progettazione":
                    f += " classificazione = 'concorso di idee/progettazione' and "; break;
            }
        }

        SQLc.Connection = sqlcnn;
        SQLda.SelectCommand = SQLc;
        if (toggetto.Text.Length > 0) // mi preparo la select
        {
            f += " lotto.oggetto like @poggetto and ";
            SQLc.Parameters.AddWithValue("@poggetto", "%" + toggetto.Text.ToString() + "%");  // '%Desk%'
        }
        if (ddlente.Text.Length > 0)
        {
            f += " richieste.ente like @pente and ";
            string tx = ddlente.Text.ToString().Trim();
            if (tx.Substring(tx.Length - 1) == "'") tx = tx.Substring(0, tx.Length - 1);
            SQLc.Parameters.AddWithValue("@pente", "%" + tx + "%");
        }
        f = f.Length > 0 ? f.Substring(0, f.Length - 4) : ""; // tolgo ultimo and

		s = "SELECT richieste.codice, richieste.id, richieste.anno, richieste.titolo, richieste.datarichiesta, richieste.ente, tipoente.enteesterno, richieste.tipologia_EK, richieste.settoretipologialavoro_EK, settore.descrizione, richieste.RiNote, richieste.tempiapprovazioneavvio, richieste.sistemaaffidamento_EK, richieste.procedura_EK, procedura.procedura, richieste.criterioaggiudicazione_EK, richieste.proposta_EK, proposta.proposta, richieste.propostadata, richieste.funzionarioindividuato_EK, richieste.dataaccettazioneproposta, richieste.idonea, richieste.Revocata, lotto.data_contratto, Lotto.RevocataAnnullata, richieste.Ritirata, Lotto.web, richieste.TipologiaLavori_EK, tipologia.classificazione, Criterioaggiudicazione.Criterio, Datigara.Soprasogliacomunitaria, Datigara.DatarichiestachiarimentistrutturaEnte, Datigara.DatarispostachiarimentistrutturaEnte, Datigara.Datapubblicazionebando, Datigara.Datascadenzapresentazioneistanzedipartecipazione, Datigara.DataSpedizioneInvito, Datigara.Datascadenzapresentazioneofferte, Datigara.DataPRESUNTASCADENZAPRESENTAZIONEOFFERTE, Datigara.Dataricezioneverbalicommissionetecnica, Lotto.Lotto, Lotto.*, statogara.descrizione, qryTizTempisticaSeduta3.dataSeduta3, qryTizTempisticaSeduta1.dataSeduta1, Datigara.Soprasogliacomunitaria, Fasi.cod, Datimonitoraggio.ultimafaseconclusa, Datimonitoraggio.*, Fasi.*, Datimonitoraggio.ultimafaseconclusa AS duratafa, descrizionefasi.[descrizione fase], Ditte.Denominazione, richieste.dataassegnazioneincarico, richieste.Servizio_competente, tipoente.tipoente, richieste.RiNote, richieste.Note, richieste.GaraTelematica, Datigara.DataNominaCommTec, Datigara.url, Ditte.Denominazione, Lotto.web, Datimonitoraggio.ultimafaseconclusa, Lotto.Contenzioso, qryUltimaPianificazione.Bando, qryUltimaPianificazione.InCorso, qryUltimaPianificazione.Rideterminazione ";
		s += "FROM(Ditte RIGHT JOIN((procedura RIGHT JOIN((proposta RIGHT JOIN(settore RIGHT JOIN(Criterioaggiudicazione RIGHT JOIN(tipologia RIGHT JOIN(funzionari RIGHT JOIN((((tipoente RIGHT JOIN(ElencoStrutture RIGHT JOIN(((richieste LEFT JOIN Datigara ON richieste.id = Datigara.Richiesta_EK) LEFT JOIN Lotto ON richieste.id = Lotto.Lottorichiesta_EK) LEFT JOIN qryTizTempisticaSeduta3 ON Lotto.id = qryTizTempisticaSeduta3.Consulenza_EK) ON ElencoStrutture.Struttura = richieste.ente) ON tipoente.id = ElencoStrutture.TipoEnte_EK) LEFT JOIN Datimonitoraggio ON(Lotto.Lottorichiesta_EK = Datimonitoraggio.IDgara_EK) AND(Lotto.Lotto = Datimonitoraggio.Lotto)) LEFT JOIN Fasi ON Datimonitoraggio.idtipogara_ek = Fasi.idtipogara) LEFT JOIN statogara ON Lotto.StatoBando_EK = statogara.id) ON funzionari.id = richieste.funzionarioindividuato_EK) ON tipologia.id = richieste.tipologia_EK) ON Criterioaggiudicazione.Idca = richieste.criterioaggiudicazione_EK) ON settore.ID = richieste.settoretipologialavoro_EK) ON proposta.id = richieste.proposta_EK) LEFT JOIN qryTizTempisticaSeduta1 ON Lotto.id = qryTizTempisticaSeduta1.Consulenza_EK) ON procedura.id = richieste.procedura_EK) LEFT JOIN descrizionefasi ON Datimonitoraggio.ultimafaseconclusa = descrizionefasi.cofa) ON Ditte.id = Lotto.Impresaaggiudicataria_EK) LEFT JOIN qryUltimaPianificazione ON Lotto.id = qryUltimaPianificazione.lotto_ek ";
		s += "WHERE(((richieste.proposta_EK) = 2) AND((richieste.idonea) = 0 Or(richieste.idonea) Is Null) AND((richieste.Revocata) = 0)  AND((richieste.Ritirata) = 0 Or(richieste.Ritirata) Is Null) AND((Lotto.web) = 1)  AND ((Lotto.Contenzioso) = 0 Or(Lotto.Contenzioso) Is Null)) ";
		s += (f != "" ? " and " + f + " " : " "); // aggiungo filtro da pharsing
		s += "ORDER BY richieste.codice desc, Lotto.Lotto ";

		if (TEST == "SI")
        {    // solo per test: include tutte le gare
			s = "SELECT richieste.codice, richieste.id, richieste.anno, richieste.titolo, richieste.datarichiesta, richieste.ente, tipoente.enteesterno, richieste.tipologia_EK, richieste.settoretipologialavoro_EK, settore.descrizione, richieste.RiNote, richieste.tempiapprovazioneavvio, richieste.sistemaaffidamento_EK, richieste.procedura_EK, procedura.procedura, richieste.criterioaggiudicazione_EK, richieste.proposta_EK, proposta.proposta, richieste.propostadata, richieste.funzionarioindividuato_EK, richieste.dataaccettazioneproposta, richieste.idonea, richieste.Revocata, lotto.data_contratto, Lotto.RevocataAnnullata, richieste.Ritirata, Lotto.web, richieste.TipologiaLavori_EK, tipologia.classificazione, Criterioaggiudicazione.Criterio, Datigara.Soprasogliacomunitaria, Datigara.DatarichiestachiarimentistrutturaEnte, Datigara.DatarispostachiarimentistrutturaEnte, Datigara.Datapubblicazionebando, Datigara.Datascadenzapresentazioneistanzedipartecipazione, Datigara.DataSpedizioneInvito, Datigara.Datascadenzapresentazioneofferte, Datigara.DataPRESUNTASCADENZAPRESENTAZIONEOFFERTE, Datigara.Dataricezioneverbalicommissionetecnica, Lotto.Lotto, Lotto.*, statogara.descrizione, qryTizTempisticaSeduta3.dataSeduta3, qryTizTempisticaSeduta1.dataSeduta1, Datigara.Soprasogliacomunitaria, Fasi.cod, Datimonitoraggio.ultimafaseconclusa, Datimonitoraggio.*, Fasi.*, Datimonitoraggio.ultimafaseconclusa AS duratafa, descrizionefasi.[descrizione fase], Ditte.Denominazione, richieste.dataassegnazioneincarico, richieste.Servizio_competente, tipoente.tipoente, richieste.RiNote, richieste.Note, richieste.GaraTelematica, Datigara.DataNominaCommTec, Datigara.url, Ditte.Denominazione, Lotto.web, Datimonitoraggio.ultimafaseconclusa, Lotto.Contenzioso, qryUltimaPianificazione.Bando, qryUltimaPianificazione.InCorso, qryUltimaPianificazione.Rideterminazione ";
			s += "FROM(Ditte RIGHT JOIN((procedura RIGHT JOIN((proposta RIGHT JOIN(settore RIGHT JOIN(Criterioaggiudicazione RIGHT JOIN(tipologia RIGHT JOIN(funzionari RIGHT JOIN((((tipoente RIGHT JOIN(ElencoStrutture RIGHT JOIN(((richieste LEFT JOIN Datigara ON richieste.id = Datigara.Richiesta_EK) LEFT JOIN Lotto ON richieste.id = Lotto.Lottorichiesta_EK) LEFT JOIN qryTizTempisticaSeduta3 ON Lotto.id = qryTizTempisticaSeduta3.Consulenza_EK) ON ElencoStrutture.Struttura = richieste.ente) ON tipoente.id = ElencoStrutture.TipoEnte_EK) LEFT JOIN Datimonitoraggio ON(Lotto.Lottorichiesta_EK = Datimonitoraggio.IDgara_EK) AND(Lotto.Lotto = Datimonitoraggio.Lotto)) LEFT JOIN Fasi ON Datimonitoraggio.idtipogara_ek = Fasi.idtipogara) LEFT JOIN statogara ON Lotto.StatoBando_EK = statogara.id) ON funzionari.id = richieste.funzionarioindividuato_EK) ON tipologia.id = richieste.tipologia_EK) ON Criterioaggiudicazione.Idca = richieste.criterioaggiudicazione_EK) ON settore.ID = richieste.settoretipologialavoro_EK) ON proposta.id = richieste.proposta_EK) LEFT JOIN qryTizTempisticaSeduta1 ON Lotto.id = qryTizTempisticaSeduta1.Consulenza_EK) ON procedura.id = richieste.procedura_EK) LEFT JOIN descrizionefasi ON Datimonitoraggio.ultimafaseconclusa = descrizionefasi.cofa) ON Ditte.id = Lotto.Impresaaggiudicataria_EK) LEFT JOIN qryUltimaPianificazione ON Lotto.id = qryUltimaPianificazione.lotto_ek ";
			s += "WHERE(((richieste.proposta_EK) = 2) AND((richieste.idonea) = 0 Or(richieste.idonea) Is Null) AND((richieste.Revocata) = 0)  AND((richieste.Ritirata) = 0 Or(richieste.Ritirata) Is Null) AND((Lotto.web) = 1)  AND ((Lotto.Contenzioso) = 0 Or(Lotto.Contenzioso) Is Null)) ";
			s += (f != "" ? " and " + f + " " : " "); // aggiungo filtro da pharsing
			s += "ORDER BY richieste.codice desc, Lotto.Lotto ";
		}

        SQLc.CommandText = s;
        try
        {
            SQLda.Fill(ds, "gare");
            rr = ds.Tables["gare"].Rows.Count;
            if (rr > 0) tStato.Text = "Trovate righe: " + rr.ToString();
        }
        catch (Exception ex)
        {
            tStato.Text = string.Format("Error: Failed to retrieve the required data from the DataBase.\n{0}", ex.Message);
            return;
        }
        if (rr > 0)  //  ci sono gare che soddisfano i criteri...
        {
            int tabelle = ds.Tables.Count;
            int i;            
            tStato.Text = "";

            // definisco uno stile da applicare alla cella
            TableItemStyle tcStyle = new TableItemStyle();
            tcStyle.HorizontalAlign = HorizontalAlign.Left;
            tcStyle.VerticalAlign = VerticalAlign.Top;
            tcStyle.Width = Unit.Pixel(20);

			int r = 0;
            int righetrovate = 0;
            bool okloop = false;
            string stato = "";
            DateTime oggi = DateTime.Now;
            DateTime dufc = Convert.ToDateTime("1900-1-1");  // data ultima fase conclusa
            DateTime dsf = DateTime.Now; // data scadenza fase attuale
            DateTime dif5 = Convert.ToDateTime("1900-1-1");  // data inizio fase 5
            DateTime dff5 = Convert.ToDateTime("1900-1-1");  // data fine fase 5
            DateTime dsa = Convert.ToDateTime("1900-1-1");   // data seduta di aggiudicazione
            DateTime daa = Convert.ToDateTime("1900-1-1");   // data avviso di aggiudicazione
            DateTime dcg = Convert.ToDateTime("1900-1-1");   // data conclusione prevista della gara
			DateTime dsc = Convert.ToDateTime("1900-1-1");   // data stipula contratto (lotto)
			bool ritirata = false;
			int sefp = 0; // somma teorica
            int step = 0; // somma teorica parziale
            string semaforo = "";
            int lotti = 0;
            double sommabaseasta = 0;
            string garagiàconsiderata = "";
            string titolo = "";
            while (r < ds.Tables["gare"].Rows.Count)
            {
                oggi = DateTime.Now;
                dufc = Convert.ToDateTime("1900-1-1");  // data ultima fase conclusa
                dsf = DateTime.Now; // data scadenza fase attuale
                dif5 = Convert.ToDateTime("1900-1-1");  // data inizio fase 5
                dff5 = Convert.ToDateTime("1900-1-1");  // data fine fase 5
                dsa = Convert.ToDateTime("1900-1-1");   // data seduta di aggiudicazione
                daa = Convert.ToDateTime("1900-1-1");   // data avviso di aggiudicazione
                dcg = Convert.ToDateTime("1900-1-1");   // data conclusione prevista della gara
				dsc  = Convert.ToDateTime("1900-1-1");  // data contratto
				int gbs = 0;  // giorni bando stimati
                int gcs = 0;  // giorni in corso stima
                int gcns = 0; // giorni in corso rideterminati
                Int32.TryParse(ds.Tables["gare"].Rows[r]["bando"].ToString(), out gbs);
                Int32.TryParse(ds.Tables["gare"].Rows[r]["incorso"].ToString(), out gcs);
                Int32.TryParse(ds.Tables["gare"].Rows[r]["rideterminazione"].ToString(), out gcns);
                int bando = 0, incorso = 0;
                string codgara;                
                codgara = ds.Tables["gare"].Rows[r]["Codice"].ToString();
                if (codgara.ToString() == "2838")
                    incorso = 0;
                int ufc = 0;   // ultima fase conclusa
                int dfa = 0;   // durata fase attuale
                               // int gt = 0;  // giorni trascorsi
                if (ds.Tables["gare"].Rows[r]["dataultimafaseconclusa"] != DBNull.Value)
                    dufc = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["dataultimafaseconclusa"]);
                Int32.TryParse(ds.Tables["gare"].Rows[r]["duratafaseattuale"].ToString(), out dfa);
                Int32.TryParse(ds.Tables["gare"].Rows[r]["ultimafaseconclusa"].ToString(), out ufc);

                if (ds.Tables["gare"].Rows[r]["datapubblicazionebando"] != DBNull.Value)
                    dif5 = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["datapubblicazionebando"]);
                else
                    if (ds.Tables["gare"].Rows[r]["DataSpedizioneInvito"] != DBNull.Value)
                    dif5 = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["DataSpedizioneInvito"]);
                if (ds.Tables["gare"].Rows[r]["dataseduta1"] != DBNull.Value)
                    dsa = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["dataseduta1"]);
                if (ds.Tables["gare"].Rows[r]["Dataavvisodiaggiudicazione"] != DBNull.Value)
                    daa = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["Dataavvisodiaggiudicazione"]);
                if (gbs > 0) bando = gbs;
                if (ds.Tables["gare"].Rows[r]["datascadenzapresentazioneofferte"] != DBNull.Value)
                    dff5 = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["datascadenzapresentazioneofferte"]);
                else
                    dff5 = dif5.AddDays(bando);
				if (ds.Tables["gare"].Rows[r]["Data_contratto"] != DBNull.Value)
					dsc = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["Data_contratto"]);

				step = gbs + gcs;
                if (dsa.ToShortDateString().ToString() == "01/01/1900")
                    sefp = (oggi - dif5).Days;
                else
                    sefp = (dsa - dif5).Days;

                if ((int)(step * 0.9) >= sefp || (ufc >= 14 && sefp <= step))
                    semaforo = "1";
                else
                    if (step > sefp)
                        semaforo = "2";
                    else
                        semaforo = "3";
                if (dif5.ToShortDateString().ToString() != "01/01/1900")
                    stato = "bandita";
                if (dff5.ToShortDateString().ToString() != "01/01/1900" && dff5 <= oggi)
                    stato = "in corso";
                if (dsa.ToShortDateString().ToString() != "01/01/1900")
                    stato = "aggiudicata";

				if (dsc.ToShortDateString().ToString() != "01/01/1900")
				{
					stato = "contratto stipulato";
					if (dsc.ToShortDateString().ToString() != "01/01/1900" && dsc.AddDays(60) < oggi)
						stato = "procedura conclusa";
				}
				else
				{
					if (dsa.ToShortDateString().ToString() != "01/01/1900")
					{
						if (dsa.AddDays(45 + 90) < oggi) stato = "procedura conclusa";
						else
							if (dsa.AddDays(45) < oggi) stato = "stipula del contratto";
							
					}
				}

				if (ds.Tables["gare"].Rows[r]["RevocataAnnullata"] != DBNull.Value)
				{
					ritirata = ds.Tables["gare"].Rows[r]["RevocataAnnullata"].ToString() == "True" ? true : false;
					stato = ritirata ? "revocata/annullata" : stato;
				}

				// devo saltare le gare che hanno superato la fase di bandida se la richiesta è solo bandita
				DateTime dt = DateTime.Now; 
                if (ds.Tables["gare"].Rows[r]["datascadenzapresentazioneofferte"] != DBNull.Value)
                    dt = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["datascadenzapresentazioneofferte"]);                
                if ( ddlstato.Text.Length > 3 && ddlstato.Text != stato)
                    okloop = false; else okloop = true;
                okloop = garagiàconsiderata == codgara ? false : okloop;
                if (okloop) // se la gara non è un lotto
                {
                    garagiàconsiderata = codgara;
                    // vedo se gara a lotti 
                    lotti = 1;
                    sqlcnn = SQLClass.openaSQLConn(out ms);
                    if (sqlcnn.State == ConnectionState.Open)
                    {
                        // SELECT richieste.id, richieste.codice, Sum(Lotto.basedasta) AS SommaDibasedasta, Count(Lotto.id) AS ConteggioDiid
                        // FROM richieste LEFT JOIN Lotto ON richieste.id = Lotto.Lottorichiesta_EK
                        // GROUP BY richieste.id, richieste.codice
                        s = "select richieste.codice, richieste.titolo, sum(lotto.basedasta) as sba, count(lotto.id) as nl from richieste left join lotto on richieste.id=lottorichiesta_ek ";
                        s += "group by richieste.codice, richieste.titolo ";
                        s += "having richieste.codice = " + codgara;
                        if (getdata(s, ds, "sommalotti", sqlcnn) > 0)  // carico i dati della gara 
                        {
                            lotti = ds.Tables["sommalotti"].Rows[0]["nl"] != DBNull.Value ? Convert.ToInt16(ds.Tables["sommalotti"].Rows[0]["nl"]) : 1;
                            sommabaseasta = ds.Tables["sommalotti"].Rows[0]["sba"] != DBNull.Value ? Convert.ToSingle(ds.Tables["sommalotti"].Rows[0]["sba"]) : -1;
                            ds.Tables["sommalotti"].Clear();
                        }
                        sqlcnn.Close();
                    }

                    righetrovate++;
                    TableRow tRow = new TableRow();
                    tdati.Rows.Add(tRow);  // aggiungo riga

                    // 1^ riga ----------------------------
                    // 1^ riga: 1^ colonna
                    TableCell tCell11 = new TableCell();
                    if (lotti == 1) // cambio pagina in base a n. lotti
                        s = string.Format("<a href=dettaglio.aspx?id={0};l={1};s={2}>", ds.Tables["gare"].Rows[r]["id"].ToString(), ds.Tables["gare"].Rows[r]["id1"].ToString(), semaforo);
                    else
                        s = string.Format("<a href=dettagliopiu.aspx?id={0};l={1};s={2}>", ds.Tables["gare"].Rows[r]["id"].ToString(), ds.Tables["gare"].Rows[r]["id1"].ToString(), semaforo);
                    s += string.Format("<img src='{0}' alt='clicca per accedere alla pagina di dettaglio.' /></a>", "lentepiu20x20.png");
                    tCell11.Text = s;
                    tcStyle.Width = Unit.Pixel(17);
                    tcStyle.Height = Unit.Pixel(17);
                    tcStyle.BorderStyle = BorderStyle.None;
                    tcStyle.BorderColor = Color.Transparent;
                    tCell11.ApplyStyle(tcStyle);
                    tRow.Cells.Add(tCell11);

                    // 1^ riga: 2^ colonna
                    TableCell tCell12 = new TableCell();                    
                    s = (TEST == "SI" ? ds.Tables["gare"].Rows[r]["codice"].ToString() + "  TEST " : "" ) + ds.Tables["gare"].Rows[r]["ente"].ToString();
                    tCell12.Text = s;
                    tcStyle.Font.Bold = true;
                    tcStyle.BorderColor = Color.Transparent;
                    tcStyle.HorizontalAlign = HorizontalAlign.Left;
                    tCell12.ApplyStyle(tcStyle);
                    tRow.Cells.Add(tCell12);

                    // 1^ riga: 3^ colonna
                    TableCell tCell13a = new TableCell();
                    f = "";
                    if (ds.Tables["gare"].Rows[r]["url"] != DBNull.Value)
                        f = ds.Tables["gare"].Rows[r]["url"].ToString();
                    else
                        f = "http://www.appalti.provincia.tn.it";
                    s = String.Format("<a href=\"{0}\" target=\"_blank\">", f);
                    s += string.Format("<img src='{0}' alt='clicca per accedere alla documentazione di gara.' /></a>", "Folder20x20.png");
                    tCell13a.Text = s;
                    tcStyle.HorizontalAlign = HorizontalAlign.Center;
                    tcStyle.Font.Bold = false;
                    tcStyle.BorderColor = Color.Transparent;
                    tcStyle.BorderStyle = BorderStyle.None;
                    tcStyle.Width = Unit.Pixel(17);
                    tcStyle.Height = Unit.Pixel(17);
                    tCell13a.ApplyStyle(tcStyle);
                    tRow.Cells.Add(tCell13a);


					// 4^ colonna STATO
					// se gara con + lotti:     gara con xx lotti
					// se gara NON aggiudicata: ''
					// se gara RITIRATA:        'Revocata/Annullata'

					int nonaggiudicata = 0; // solo se gara ad n unico lotto
                    if (ds.Tables["gare"].Rows[r]["Nonaggiudicata"] != DBNull.Value)
                        nonaggiudicata = ds.Tables["gare"].Rows[r]["Nonaggiudicata"].ToString() == "False" ? 0 : 1;

                    TableCell tCell14 = new TableCell();
                    tCell14.Text = lotti == 1 ? stato : "gara con " + lotti.ToString() + " lotti";
                    tCell14.Text = nonaggiudicata == 0 ? tCell14.Text : "non aggiudicata";
                    tRow.Cells.Add(tCell14); // 3                
                    tcStyle.HorizontalAlign = HorizontalAlign.Center;
                    tCell14.ApplyStyle(tcStyle);
                    tCell14.Font.Bold = true;
					if ((stato == "procedura conclusa" && lotti == 1 && nonaggiudicata == 0) || stato == "revocata/annullata")
					{
						tCell14.ForeColor = Color.Black;
						tCell14.BackColor = Color.LightGray;
					}
					else
					{
						tCell14.ForeColor = Color.Black;
						tCell14.BackColor = Color.White;
					}

					// 5^ colonna                    
					TableCell tCell14a = new TableCell();
                    if (lotti > 1)
                        s = "";
                    else
                        s = nonaggiudicata == 1 ? "" : ds.Tables["gare"].Rows[r]["Denominazione"].ToString();
                    tCell14a.Text = s;
                    tcStyle.HorizontalAlign = HorizontalAlign.Left;
                    tcStyle.VerticalAlign = VerticalAlign.Top;
                    tCell14a.ApplyStyle(tcStyle);
                    tRow.Cells.Add(tCell14a);

					// si comincia.. mi calcolo lo stato per determinare se hiperlink o meno

					//TableCell tCell23 = new TableCell();
					int[] de = new int[21]; // durata effettiva
					int[] te = new int[21]; // durata teorica               
					int of1 = ds.Tables["gare"].Columns["fase5"].Ordinal;
					//int sef = 0; // somma effettiva
					sefp = 0; // somma effettiva parziale
					s = "eff: ";
					// calcolo il tempo teorico a disposizione delle fasi passate e aggiungo la durata della fase attuale se gara non ancora conclusa
					string ss = "";
					// int ste = 0; // somma teorica
					step = 0; // somma teorica parziale
					int stm = 0; // somma teorica tempi mancanti alla conclusione della gara
									//int stmottimistica = 0; // somma teorica tempi mancanti vista ottimistica
									//int stmpessimistica = 0; // somma teorica tempi mancanti vista ottimistica
					string codicetipogara = ds.Tables["gare"].Rows[r]["idtipogara"] == DBNull.Value ? "" : ds.Tables["gare"].Rows[r]["idtipogara"].ToString();
					of1 = ds.Tables["gare"].Columns["f1"].Ordinal;

					// 6^ colonna
					TableCell tCell16 = new TableCell();
					tCell16.Text = sommabaseasta.ToString("c");
					tRow.Cells.Add(tCell16);
					tcStyle.HorizontalAlign = HorizontalAlign.Right;
					tCell16.ApplyStyle(tcStyle);

					if (stato != "revocata/annullata")
					{
						// tempistica: semaforo sesta colonna
						//incorso = te[8] + te[9] + te[10] + te[11] + te[12]; // tempi pessimistici
						if (gcs > 0) incorso = gcs;                         // tempi pianificati
						int ttfec = 0; // tempi teorici fasi effettivamente coinvolte 
						of1 = ds.Tables["gare"].Columns["fase1"].Ordinal;
						ttfec = bando; // il bando ci deve essere per forza
						if (ufc >= 9 && (ds.Tables["gare"].Rows[r]["Richiestechiarimenti"] != DBNull.Value || ds.Tables["gare"].Rows[r]["Riscontrochiarimenti"] != DBNull.Value))
						{
							ttfec += te[8];  // riscontro chiarimenti fase 9
						}
						if (ufc >= 10 && (ds.Tables["gare"].Rows[r]["Datarichiestadomumentiperverificaart48"] != DBNull.Value || ds.Tables["gare"].Rows[r]["Datariscontrorichiestaart48"] != DBNull.Value))
						{
							ttfec += te[9];  // Verifica a campione fase 10
						}
						if (ufc >= 11 && (ds.Tables["gare"].Rows[r]["Dataricezioneverbalicommissionetecnica"] != DBNull.Value || ds.Tables["gare"].Rows[r]["dataseduta3"] != DBNull.Value))
						{
							ttfec += te[10];  // Valutazione Tecnica (c'è sempre nelle offerte economicamente + vantaggiose) fase 11
						}
						if (ufc >= 12 && (ds.Tables["gare"].Rows[r]["datainvioofferteperanomalia"] != DBNull.Value || ds.Tables["gare"].Rows[r]["Dataricezioneesitoanomalia"] != DBNull.Value))
						{
							ttfec += te[11];  // Valutazione anomalia fase 12
						}
						if (ufc >= 13 && (ds.Tables["gare"].Rows[r]["richiestarinnovo"] != DBNull.Value || ds.Tables["gare"].Rows[r]["riscontrorinnovo"] != DBNull.Value))
						{
							ttfec += te[12];  // Richiesta rinnovo polizza fase 13
						}
						if (ufc >= 14 && (ds.Tables["gare"].Rows[r]["dataseduta1"] != DBNull.Value || ds.Tables["gare"].Rows[r]["Dataavvisodiaggiudicazione"] != DBNull.Value))
						{
							ttfec += te[13];  // Seduta di aggiudicazione fase 14
						}

						step = gcs > 0 ? (bando + incorso) : ttfec; // sostituisco i tempi teorici con i tempi teorici delle fasi effettivamente trascorse e 
						step = gcns > 0 ? (bando + gcns) : step;
						// vediamo se la gara è conclusa: se si dataseduta avviso di aggiudicazione - data pubblicazione bando
						if (dsa.ToShortDateString().ToString() != "01/01/1900") // controloo solo fino alla seduta di aggiudicazione DA CAMBIARE
						{
							sefp = (dsa - dif5).Days; // tempi effettivi parziali     
						}
						else  // se gara in corso: oggi - data pubblicazione
						{
							sefp = (oggi - dif5).Days;
						}
						if (nonaggiudicata == 1)
							sefp = (dsa - dif5).Days;

						// 7^ colonna luce semaforica
						TableCell tCell17 = new TableCell();  // luce semaforica
						tCell17.Text = "";
						dcg = dif5;
						string vialibera = "";
						if ((int)(step * 0.9) >= sefp || (ufc >= 14 && sefp <= step))
						{ s = "luceverde20x20.png"; vialibera = "verde"; }
						else
							if (step >= sefp)
						{ s = "lucegialla20x20.png"; vialibera = "gialla"; }
						else
						{
							s = "lucerossa20x20.png"; vialibera = "rossa";
							// devo aggiungere a oggi il tempo teorico 
							dcg = oggi;
						}
						dcg = dcg.AddDays(stm);
						dsf = dufc;
						dsf.AddDays(dfa);
						ss = string.Format("<img src='{0}' alt='Semaforo con luce " + vialibera + "'/>", s); // + " " + step.ToString() + "-" + sef.ToString();
						ss = nonaggiudicata != 1 ? ss : "";
						ss = lotti == 1 ? ss : "";
						tcStyle.HorizontalAlign = HorizontalAlign.Center;
						tcStyle.Width = Unit.Pixel(17);
						tcStyle.Height = Unit.Pixel(17);
						tCell17.ApplyStyle(tcStyle);
						tCell17.Text = ss;

						// sefp = somma effettiva parziale; step = somma teorica parziale
						tRow.Cells.Add(tCell17); // aggiungo cmq la cella sia che sia vuota che con img
												 //tcStyle.HorizontalAlign = HorizontalAlign.Left;
												 //tcStyle.VerticalAlign = VerticalAlign.Middle;
												 //tcStyle.Width = Unit.Pixel(30);
												 //tCell26.ApplyStyle(tcStyle);

						// 8^ riga (determinazione)
						TableCell tCell18 = new TableCell();  // luce semaforica
															  //tCell26.Text += " " + sefp.ToString() + ":" + step.ToString()+" u"+ufc.ToString()+ " 5f"+dif5.ToShortDateString();
															  // se gara conclusa ?
						if (nonaggiudicata == 0)  // solo se un unico lotto
						{
							if (ds.Tables["gare"].Rows[r]["dataseduta1"] != DBNull.Value)
								ss = String.Format("({0}gg.)", ((sefp - step) > 0 ? "+" : "") + (sefp - step).ToString("G"));
							else
								ss = String.Format("{0:" + formatodata + "}", dif5.AddDays(bando + incorso));
						}
						else
							ss = "";
						ss = lotti == 1 ? ss : "";
						tCell18.Text = ss;

						tcStyle.HorizontalAlign = HorizontalAlign.Center;
						tCell18.ApplyStyle(tcStyle);
						tRow.Cells.Add(tCell18);

						// 9^ riga rideterminazione
						TableCell tCell19 = new TableCell();  // luce semaforica
															  //s = ds.Tables["gare"].Rows[r]["rideterminazione"] == DBNull.Value ? "" : ds.Tables["gare"].Rows[r]["rideterminazione"].ToString();
						ss = gcns > 0 ? dif5.AddDays(bando + gcns).ToString(formatodata) + " *" : "";
						ss = lotti == 1 ? ss : "";
						tCell19.Text = ss;
						tcStyle.HorizontalAlign = HorizontalAlign.Center;
						tCell19.ApplyStyle(tcStyle);
						tRow.Cells.Add(tCell19);
					}

                    // 2^ riga ----------------------------------
                    TableRow tRow1 = new TableRow();
                    tdati.Rows.Add(tRow1);  // aggiungo riga

                    // 2^ riga prima colonna
                    TableCell tCell21 = new TableCell();
                    tCell21.Text = "".ToString();
                    tRow1.Cells.Add(tCell21); // 0
                    
                    //if (codgara == "1211")
                    //    ss = "";
                    /*
                    for (i = of1; i < of1 + 14; i++)  // calcolo tempi teorici
                    {
                        Int32.TryParse(ds.Tables["gare"].Rows[r][i].ToString(), out te[i - of1]);
                        ste += te[i - of1];
                        step += i - of1 >= 4 && i - of1 <= ufc ? te[i - of1] : 0;
                        stm += i - of1 >= 4 ? te[i - of1] : 0;
                        stmpessimistica += (i - of1 + 1) >= ufc ? te[i - of1] : 0;
                        stmottimistica += (i - of1 + 1) >= ufc && ((i - of1 + 1) == 14 || (i - of1) < 8 || (i - of1 + 1) == 11) ? te[i - of1] : 0;
                        sss += ds.Tables["gare"].Columns[i].ColumnName.ToString() + ": " + ds.Tables["gare"].Rows[r][i].ToString();
                        ss += te[i - of1].ToString() + "-";
                    }
                    */
                    //if (codgara == "2346")
                    //    ss = "";
                    // cerco di capire i tempi delle fasi effettivamente coinvolte (vale oper le gare chiuse e per le fasi trascorse)
                    // bando = te[4] + te[5] + te[6] + te[7];
                    
                    Int32.TryParse(ds.Tables["gare"].Rows[r]["revocata"].ToString(), out i);
                    if (i == 1) s = "revocata"; // solo se non è a lotti.....
                    // 2^ colonna
                    TableCell tCell22 = new TableCell();
                    titolo = lotti > 1 && ds.Tables["gare"].Rows[r]["Titolo"] != DBNull.Value ? ds.Tables["gare"].Rows[r]["Titolo"].ToString() : ds.Tables["gare"].Rows[r]["oggetto"].ToString();
                    tCell22.Text = titolo;
                    tcStyle.HorizontalAlign = HorizontalAlign.Left;
                    tCell22.ApplyStyle(tcStyle);                
                    tRow1.Cells.Add(tCell22); // 2

                    // 2^ riga: 3^ colonna
                    TableCell tCell23 = new TableCell();
                    tcStyle.Font.Bold = false;
                    tcStyle.BorderColor = Color.Transparent;
                    tCell23.ApplyStyle(tcStyle);
                    tCell23.Text = "".ToString();
                    tRow1.Cells.Add(tCell23); // 0

                    // 2^ riga: 4^ colonna
                    TableCell tCell24a = new TableCell();
                    tCell24a.Text = "";
                    tRow1.Cells.Add(tCell24a); // 0

                    // 2^ riga: 5^ colonna
                    TableCell tCell25 = new TableCell();
                    tCell25.Text = "";
                    tRow1.Cells.Add(tCell25); // 0

                    // 2^ riga: 6^ colonna
                    TableCell tCell26 = new TableCell();
                    tCell26.Text = "";
                    tRow1.Cells.Add(tCell26); // 0

                    // 2^ riga: 7^ colonna
                    TableCell tCell27 = new TableCell();
                    tCell27.Text = "";
                    tRow1.Cells.Add(tCell27); // 0

                    // 1^ riga: 8^ colonna
                    TableCell tCell28 = new TableCell();
                    tCell28.Text = "".ToString();
                    tRow1.Cells.Add(tCell28); // 0


                    //if (codgara == "3270")
                    //    ss = "";
                    // aggiungo testo tempi
                    /*
                      tCell26.Style.Add("text-align", "Midle");
                      tCell26.Style.Add("Verticalalign", "Middle");
                      tCell26.Style.Add("border-color", "black");
                    //cel1.Style(HtmlTextWriterStyle.FontSize) = 9
                    //TableCell tCell24 = new TableCell();
                    //tCell24.Text = ds.Tables["gare"].Rows[r]["Denominazione"].ToString();
                    //tRow1.Cells.Add(tCell24); */
                    TableRow tRow3 = new TableRow();
                    tdati.Rows.Add(tRow3);  // aggiungo riga
                    // 1^ riga: 8^ colonna
                    TableCell tCell30 = new TableCell();
                    tCell30.Height = 17;
                    tRow3.Cells.Add(tCell30); // 0
                }
                r++; // incremento numero di righe
            }
            tRighetrovate.Text = righetrovate.ToString() + " occorrenze trovate.";
            // aggiungo spiegazioni
            TableRow tR1 = new TableRow(); Tu.Rows.Add(tR1);
            TableCell tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC1 = new TableCell(); tC1.Text = String.Format("<img src='{0}' />", "lentepiu28x28.png"); tR1.Cells.Add(tC1); tC1.Style.Add("text-align", "Center");
            TableCell tC11 = new TableCell(); tC11.Text = "Visualizza ulteriori dettagli;"; tR1.Cells.Add(tC11);  tC11.Style.Add("text-align", "Left");
            //----------------
            tR1 = new TableRow(); Tu.Rows.Add(tR1);
            tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC2 = new TableCell(); tC2.Text = String.Format("<img src='{0}' />", "Folder33x33.jpg"); tR1.Cells.Add(tC2); tC2.Style.Add("text-align", "Center");
            s = "Per accededere a tutti i documenti della procedura di gara pubblicati nella sezione Bandi e Appalti del portale dell'Agenzia Provinciale per gli Appalti e Contratti;";
            TableCell tC22 = new TableCell(); tC22.Text = s; tR1.Cells.Add(tC22); tC22.Style.Add("text-align", "Left");
            tR1 = new TableRow(); Tu.Rows.Add(tR1);
            tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC3 = new TableCell(); tC3.Text = String.Format("<img src='{0}' />", "luceverde.png"); tR1.Cells.Add(tC3); tC3.Style.Add("text-align", "Center");
            s = " Tempi delle procedure di gara previsti, pienamente rispettati;";
            TableCell tC32 = new TableCell(); tC32.Text = s; tR1.Cells.Add(tC32); tC32.Style.Add("text-align", "Left");
            tR1 = new TableRow(); Tu.Rows.Add(tR1);
            tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC4 = new TableCell(); tC4.Text = String.Format("<img src='{0}' />", "lucegialla.png"); tR1.Cells.Add(tC4); tC4.Style.Add("text-align", "Center");
            s = " Tempi delle procedure di gara in linea con quanto prestabilito. Scadenza conclusione gara o fase di gara molto vicina;";
            TableCell tC42 = new TableCell(); tC42.Text = s; tR1.Cells.Add(tC42); tC42.Style.Add("text-align", "Left");
            tR1 = new TableRow(); Tu.Rows.Add(tR1);
            tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC5 = new TableCell(); tC5.Text = String.Format("<img src='{0}' />", "lucerossa.png"); tR1.Cells.Add(tC5); tC5.Style.Add("text-align", "Center");
            s = " Tempi delle procedure di gara previsti superati;";
            TableCell tC52 = new TableCell(); tC52.Text = s; tR1.Cells.Add(tC52); tC52.Style.Add("text-align", "Left");
            tR1 = new TableRow(); Tu.Rows.Add(tR1);
            tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC6 = new TableCell(); tC6.Text = "(-gg)"; tR1.Cells.Add(tC6); tC6.Style.Add("text-align", "Center");
            s = "Indica che le procedure di gara sono state portate a termine in anticipo del numero di giorni indicato;";
            TableCell tC62 = new TableCell(); tC62.Text = s; tR1.Cells.Add(tC62); tC62.Style.Add("text-align", "Left");
            tR1 = new TableRow(); Tu.Rows.Add(tR1);
            tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC7 = new TableCell(); tC7.Text = "(+gg)"; tR1.Cells.Add(tC7); tC7.Style.Add("text-align", "Center");
            s = "Indica che le procedure di gara sono state portate a termine in ritardo del numero di giorni indicato;";
            TableCell tC72 = new TableCell(); tC72.Text = s; tR1.Cells.Add(tC72); tC72.Style.Add("text-align", "Left");
            tR1 = new TableRow(); Tu.Rows.Add(tR1);
            tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC8 = new TableCell(); tC8.Text = "(data)"; tR1.Cells.Add(tC8); tC8.Style.Add("text-align", "Center");
            s = "Indica il periodo stimato per la conclusione delle procedure di gara;";
            TableCell tC82 = new TableCell(); tC82.Text = s; tR1.Cells.Add(tC82); tC82.Style.Add("text-align", "Left");
            tR1 = new TableRow(); Tu.Rows.Add(tR1);
            tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC9 = new TableCell(); tC9.Text = "(data *)"; tR1.Cells.Add(tC9); tC9.Style.Add("text-align", "Center");
            s = "Indica il periodo rideterminato per la conclusione delle procedure di gara. La rideterminazione potrà verificarsi a fronte di fatti imprevedibili quali ad esempio numero offerte pervenute. Nella pagina di dettaglio si potranno trovare indicazione in merito.";
            TableCell tC92 = new TableCell(); tC92.Text = s; tR1.Cells.Add(tC92); tC92.Style.Add("text-align", "Left");
        }
        else
        {
            tStato.Text = "nessun elemento trovato.";
            tRighetrovate.Text =  "0 occorrenze trovate.";
        }
    }

    protected string costruiscistringa(string sp, int n)
    {
        if (n <= 0) return ("");
        string s = "";
        for (int i = 1; i <= n; i++) s += sp;
        return (s);
    }

    protected string virgolette(string s)
    {
        string ss = "";
        string virgoletta = "'";
        string doppie = string.Format("{0}", "\"");
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == virgoletta[0])
                ss += virgoletta+virgoletta;
            else ss += s[i];
            /*else
                if (s[i] == doppie[0])
                    ss += virgoletta;
            else
                ss += s[i]; */
        }
        return (ss);
    }
 
    protected long getdata(string select, DataSet dset, string strTableName, SqlConnection conn)
    {
        long rr = -1;
        if (conn.State != ConnectionState.Open)
        {
            tStato.Text = string.Format("Error: Aprire la connessione al DB prima di chiamare getdata!");
            return (rr);
        }
        try
        {
            SQLda = new SqlDataAdapter(select, conn);
            //da.SelectCommand.CommandText = select;
            //if (ds.Tables[strTableName].Rows.Count > 0 ) ds.Tables[strTableName].Clear(); // evito il doppi caricamento dei dati
            SQLda.Fill(dset, strTableName);
            rr = dset.Tables[strTableName].Rows.Count;         
        }
        catch (Exception ex)
        {
            tStato.Text = string.Format("Error: Failed to retrieve the required data from the DataBase.\n{0}", ex.Message);
            return (rr);
        }
        finally
        {
            conn.Close();
        }
        return (rr);
    }

    protected void cbreset_Click(object sender, EventArgs e)
    {
        toggetto.Text = "";
        ddlente.Text = "";
        ddlstato.Text = "";
        ddltipo.Text = "";
        tStato.Text = "";
        tRighetrovate.Text = "";
        cbContenzioso.Checked = false;
        //Asterisco.Text = "";
    }

    protected void Login_Click(object sender, EventArgs e)
    {
        if (Login.Text == "Accedi")
            Response.Redirect("login.aspx");
        else  // logout
        {
            Session.Abandon();
            Login.Text = "Accedi";
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Style pulito = new Style();
        pulito.BorderColor = System.Drawing.Color.Green;

        if (Button1.BorderColor == System.Drawing.Color.Black)
        {
            Button1.BorderColor = System.Drawing.Color.Red;
            tdati.Style.Add("display", "none");
        }
        else
        {
            Button1.BorderColor = System.Drawing.Color.Black;
            tdati.ApplyStyle(pulito);
        }
            
        /*
        tdati.Visible = !tdati.Visible;
        if (tdati.Visible)
        {
            EventArgs ee = new EventArgs();
            cbGo_Click(this, ee);
        }
        */
    }
}


/*
 * codice per mandare una mail
        //gmail gm = new gmail();
        //                mandamail(string server, int port, string userName,          string password, string dachi,                     string[] achi, string[] achicc, string[] achiccn, string subject, string body)
        //string[] achi = new string[] { "tiziano.donati@provincia.tn.it" };
        //bool spedita = gm.mandamail("", -1, "", "", "tiziano.donati@provincia.tn.it", achi, achi, achi, "oggetto teorico", "keess, lowe and peace!");
*/