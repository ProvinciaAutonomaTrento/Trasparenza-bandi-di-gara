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
using System.Drawing;

public partial class dettaglio : System.Web.UI.Page
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
    public string idgara = "";
    public double basedastasomma = 0;
    public string s = "";
    TableItemStyle tcStyle = new TableItemStyle();
    public bool mettolamail = false;
    public string stato = "";
    public DateTime oggi = DateTime.Now;
    public int lotti = 0;
    string codicetipogara = "";
    static string rawstringa;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA....
        {
            rawstringa = Request.RawUrl;
            //tStato.Text = string.Format("Benvenuto {0}.", (Environment.UserDomainName + "\\" + Environment.UserName));
            EventArgs ea = new EventArgs();
            //testa();
            cbGo_Click(this, ea);
        }
    }

    protected void cbGo_Click(object sender, EventArgs e)
    {
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

        idgara = codicegara();
        string f = "richieste.id = " + idgara + " and ";

        f += cbContenzioso.Checked ? " contenzioso = 1 and " : " (contenzioso = 0 or contenzioso is null) and ";
        if (toggetto.Text.Length > 0) // mi preparo la select
        {
            f += " lotto.oggetto like @poggetto and ";
            SQLc.Parameters.AddWithValue("@poggetto", "%" + toggetto.Text.ToString() + "%");  // '%Desk%'
        }

        if (ddlLotti.Text.Length > 0)
        {   int tx = Convert.ToInt16(ddlLotti.Text);
            if (tx > 0)
                f += " lotto.lotto = " + tx.ToString() + " and ";  
        }
        f = f.Length > 0 ? f.Substring(0, f.Length - 4) : ""; // tolgo ultimo and

		s = "SELECT richieste.codice, richieste.id,  richieste.titolo, richieste.anno, richieste.datarichiesta, richieste.ente, tipoente.enteesterno, richieste.tipologia_EK, richieste.settoretipologialavoro_EK, settore.descrizione, richieste.RiNote, richieste.tempiapprovazioneavvio, richieste.sistemaaffidamento_EK, richieste.procedura_EK, procedura.procedura, richieste.criterioaggiudicazione_EK, richieste.proposta_EK, proposta.proposta, richieste.propostadata, richieste.funzionarioindividuato_EK, richieste.dataaccettazioneproposta, richieste.idonea, richieste.Revocata, Lotto.RevocataAnnullata, richieste.Ritirata, Lotto.web, richieste.TipologiaLavori_EK, tipologia.classificazione, Criterioaggiudicazione.Criterio, Datigara.Soprasogliacomunitaria, Datigara.DatarichiestachiarimentistrutturaEnte, Datigara.DatarispostachiarimentistrutturaEnte, Datigara.Datapubblicazionebando, Datigara.Datascadenzapresentazioneistanzedipartecipazione, Datigara.DataSpedizioneInvito, Datigara.Datascadenzapresentazioneofferte, Datigara.DataPRESUNTASCADENZAPRESENTAZIONEOFFERTE, Datigara.Dataricezioneverbalicommissionetecnica, Lotto.Lotto, Lotto.*, statogara.descrizione, qryTizTempisticaSeduta3.dataSeduta3, qryTizTempisticaSeduta1.dataSeduta1, Datigara.Soprasogliacomunitaria, Fasi.cod, Datimonitoraggio.ultimafaseconclusa, Datimonitoraggio.*, Fasi.*, Datimonitoraggio.ultimafaseconclusa AS duratafa, descrizionefasi.[descrizione fase], Ditte.Denominazione, richieste.dataassegnazioneincarico, richieste.Servizio_competente, tipoente.tipoente, richieste.RiNote, richieste.Note, richieste.GaraTelematica, Datigara.DataNominaCommTec, Datigara.url, Ditte.Denominazione, Lotto.web, Datimonitoraggio.ultimafaseconclusa, Lotto.Contenzioso, qryUltimaPianificazione.Bando, qryUltimaPianificazione.InCorso, qryUltimaPianificazione.Rideterminazione ";
		s += "FROM(Ditte RIGHT JOIN((procedura RIGHT JOIN((proposta RIGHT JOIN(settore RIGHT JOIN(Criterioaggiudicazione RIGHT JOIN(tipologia RIGHT JOIN(funzionari RIGHT JOIN((((tipoente RIGHT JOIN(ElencoStrutture RIGHT JOIN(((richieste LEFT JOIN Datigara ON richieste.id = Datigara.Richiesta_EK) LEFT JOIN Lotto ON richieste.id = Lotto.Lottorichiesta_EK) LEFT JOIN qryTizTempisticaSeduta3 ON Lotto.id = qryTizTempisticaSeduta3.Consulenza_EK) ON ElencoStrutture.Struttura = richieste.ente) ON tipoente.id = ElencoStrutture.TipoEnte_EK) LEFT JOIN Datimonitoraggio ON(Lotto.Lottorichiesta_EK = Datimonitoraggio.IDgara_EK) AND(Lotto.Lotto = Datimonitoraggio.Lotto)) LEFT JOIN Fasi ON Datimonitoraggio.idtipogara_ek = Fasi.idtipogara) LEFT JOIN statogara ON Lotto.StatoBando_EK = statogara.id) ON funzionari.id = richieste.funzionarioindividuato_EK) ON tipologia.id = richieste.tipologia_EK) ON Criterioaggiudicazione.Idca = richieste.criterioaggiudicazione_EK) ON settore.ID = richieste.settoretipologialavoro_EK) ON proposta.id = richieste.proposta_EK) LEFT JOIN qryTizTempisticaSeduta1 ON Lotto.id = qryTizTempisticaSeduta1.Consulenza_EK) ON procedura.id = richieste.procedura_EK) LEFT JOIN descrizionefasi ON Datimonitoraggio.ultimafaseconclusa = descrizionefasi.cofa) ON Ditte.id = Lotto.Impresaaggiudicataria_EK) LEFT JOIN qryUltimaPianificazione ON Lotto.id = qryUltimaPianificazione.lotto_ek ";
		s += "WHERE(((richieste.proposta_EK) = 2) AND((richieste.idonea) = 0 Or(richieste.idonea) Is Null) AND((richieste.Revocata) = 0) AND((richieste.Ritirata) = 0 Or(richieste.Ritirata) Is Null)  AND ((Lotto.web) = 1) AND ((Lotto.Contenzioso) = 0 Or(Lotto.Contenzioso) Is Null)) ";
		s += (f != "" ? " and " + f + " " : " "); // aggiungo filtro da pharsing
		s += "ORDER BY richieste.codice, Lotto.Lotto ";

		if (TEST == "SI")
        {    // solo per test: include tutte le gare
			s = "SELECT richieste.codice, richieste.id,  richieste.titolo, richieste.anno, richieste.datarichiesta, richieste.ente, tipoente.enteesterno, richieste.tipologia_EK, richieste.settoretipologialavoro_EK, settore.descrizione, richieste.RiNote, richieste.tempiapprovazioneavvio, richieste.sistemaaffidamento_EK, richieste.procedura_EK, procedura.procedura, richieste.criterioaggiudicazione_EK, richieste.proposta_EK, proposta.proposta, richieste.propostadata, richieste.funzionarioindividuato_EK, richieste.dataaccettazioneproposta, richieste.idonea, richieste.Revocata, Lotto.RevocataAnnullata, richieste.Ritirata, Lotto.web, richieste.TipologiaLavori_EK, tipologia.classificazione, Criterioaggiudicazione.Criterio, Datigara.Soprasogliacomunitaria, Datigara.DatarichiestachiarimentistrutturaEnte, Datigara.DatarispostachiarimentistrutturaEnte, Datigara.Datapubblicazionebando, Datigara.Datascadenzapresentazioneistanzedipartecipazione, Datigara.DataSpedizioneInvito, Datigara.Datascadenzapresentazioneofferte, Datigara.DataPRESUNTASCADENZAPRESENTAZIONEOFFERTE, Datigara.Dataricezioneverbalicommissionetecnica, Lotto.Lotto, Lotto.*, statogara.descrizione, qryTizTempisticaSeduta3.dataSeduta3, qryTizTempisticaSeduta1.dataSeduta1, Datigara.Soprasogliacomunitaria, Fasi.cod, Datimonitoraggio.ultimafaseconclusa, Datimonitoraggio.*, Fasi.*, Datimonitoraggio.ultimafaseconclusa AS duratafa, descrizionefasi.[descrizione fase], Ditte.Denominazione, richieste.dataassegnazioneincarico, richieste.Servizio_competente, tipoente.tipoente, richieste.RiNote, richieste.Note, richieste.GaraTelematica, Datigara.DataNominaCommTec, Datigara.url, Ditte.Denominazione, Lotto.web, Datimonitoraggio.ultimafaseconclusa, Lotto.Contenzioso, qryUltimaPianificazione.Bando, qryUltimaPianificazione.InCorso, qryUltimaPianificazione.Rideterminazione ";
			s += "FROM(Ditte RIGHT JOIN((procedura RIGHT JOIN((proposta RIGHT JOIN(settore RIGHT JOIN(Criterioaggiudicazione RIGHT JOIN(tipologia RIGHT JOIN(funzionari RIGHT JOIN((((tipoente RIGHT JOIN(ElencoStrutture RIGHT JOIN(((richieste LEFT JOIN Datigara ON richieste.id = Datigara.Richiesta_EK) LEFT JOIN Lotto ON richieste.id = Lotto.Lottorichiesta_EK) LEFT JOIN qryTizTempisticaSeduta3 ON Lotto.id = qryTizTempisticaSeduta3.Consulenza_EK) ON ElencoStrutture.Struttura = richieste.ente) ON tipoente.id = ElencoStrutture.TipoEnte_EK) LEFT JOIN Datimonitoraggio ON(Lotto.Lottorichiesta_EK = Datimonitoraggio.IDgara_EK) AND(Lotto.Lotto = Datimonitoraggio.Lotto)) LEFT JOIN Fasi ON Datimonitoraggio.idtipogara_ek = Fasi.idtipogara) LEFT JOIN statogara ON Lotto.StatoBando_EK = statogara.id) ON funzionari.id = richieste.funzionarioindividuato_EK) ON tipologia.id = richieste.tipologia_EK) ON Criterioaggiudicazione.Idca = richieste.criterioaggiudicazione_EK) ON settore.ID = richieste.settoretipologialavoro_EK) ON proposta.id = richieste.proposta_EK) LEFT JOIN qryTizTempisticaSeduta1 ON Lotto.id = qryTizTempisticaSeduta1.Consulenza_EK) ON procedura.id = richieste.procedura_EK) LEFT JOIN descrizionefasi ON Datimonitoraggio.ultimafaseconclusa = descrizionefasi.cofa) ON Ditte.id = Lotto.Impresaaggiudicataria_EK) LEFT JOIN qryUltimaPianificazione ON Lotto.id = qryUltimaPianificazione.lotto_ek ";
			s += "WHERE(((richieste.proposta_EK) = 2) AND((richieste.idonea) = 0 Or(richieste.idonea) Is Null) AND((richieste.Revocata) = 0) AND((richieste.Ritirata) = 0 Or(richieste.Ritirata) Is Null)  AND ((Lotto.web) = 1) AND ((Lotto.Contenzioso) = 0 Or(Lotto.Contenzioso) Is Null)) ";
			s += (f != "" ? " and " + f + " " : " "); // aggiungo filtro da pharsing
			s += "ORDER BY richieste.codice, Lotto.Lotto ";
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
        testa();

        DateTime dufc = Convert.ToDateTime("1900-1-1");  // data ultima fase conclusa
        DateTime dsf = DateTime.Now; // data scadenza fase attuale
        DateTime dif5 = Convert.ToDateTime("1900-1-1");  // data inizio fase 5
        DateTime dff5 = Convert.ToDateTime("1900-1-1");  // data fine fase 5
        DateTime dsa = Convert.ToDateTime("1900-1-1");   // data seduta di aggiudicazione
        DateTime daa = Convert.ToDateTime("1900-1-1");   // data avviso di aggiudicazione
        DateTime dcg = Convert.ToDateTime("1900-1-1");   // data conclusione prevista della gara
        DateTime dcc = Convert.ToDateTime("1900-1-1");   // data conclusione lavori commissione tecnica
		DateTime dsc = Convert.ToDateTime("1900-1-1");   // data stipula contratto (lotto)
		int step = 0;
        int sefp = 0;
        string semaforo = "";
		bool ritirata = false;

        if (rr > 0)  // carico gli enti e li aggiungo alla ddlEnti 
        {
            int tabelle = ds.Tables.Count;
            int i;
            tStato.Text = "";

            //Titolo.Text = ds.Tables["gare"].Rows[0]["Titolo"] != DBNull.Value ? ds.Tables["gare"].Rows[0]["Titolo"].ToString() : "";
            //Titolo.Text = Titolo.Text == "" ? ds.Tables["gare"].Rows[0]["Titolo"].ToString() : "";
            // definisco uno stile da applicare alla cella
            TableItemStyle tcStyle = new TableItemStyle();
            tcStyle.HorizontalAlign = HorizontalAlign.Left;
            tcStyle.VerticalAlign = VerticalAlign.Top;
            int r = 0;
            int righetrovate = 0;
            bool okloop = false;

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
                dcc = Convert.ToDateTime("1900-1-1");   // data conclusione lavori commissione tecnica
				dsc = Convert.ToDateTime("1900-1-1");  // data contratto
				int gbs = 0;  // giorni bando stimati
                int gcs = 0;  // giorni in corso stima
                int gcns = 0; // giorniin corso rideterminati
                Int32.TryParse(ds.Tables["gare"].Rows[r]["bando"].ToString(), out gbs);
                Int32.TryParse(ds.Tables["gare"].Rows[r]["incorso"].ToString(), out gcs);
                Int32.TryParse(ds.Tables["gare"].Rows[r]["rideterminazione"].ToString(), out gcns);
                int bando = 0, incorso = 0;
                string codgara;
                codgara = ds.Tables["gare"].Rows[r]["Codice"].ToString();
                //if (codgara.ToString() == "2711")
                //    incorso = 0;
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
                if (ddlstato.Text.Length > 3 && ddlstato.Text != stato)
                    okloop = false;
                else okloop = true;
                if (okloop)
                {
                    righetrovate++;
                    // 2^ riga ----------------------------------
                    TableRow tRow1 = new TableRow();
                    tdati.Rows.Add(tRow1);  // aggiungo riga

                    // 2^ riga: 1^ colonna
                    TableCell tCell21 = new TableCell();

                    s = string.Format("<a href=dettaglio.aspx?id={0};l={1};s={2}>", ds.Tables["gare"].Rows[r]["id"].ToString(), ds.Tables["gare"].Rows[r]["id1"].ToString(), semaforo);
                    s += string.Format("<img src='{0}' alt='clicca per accedere alla pagina di dettaglio.' /></a>", "lentepiu20x20.png");
                    tCell21.Text = s;
                    tcStyle.HorizontalAlign = HorizontalAlign.Center;
                    tCell21.ApplyStyle(tcStyle);
                    tRow1.Cells.Add(tCell21);

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
                    //int ste = 0; // somma teorica
                    step = 0; // somma teorica parziale
                    int stm = 0; // somma teorica tempi mancanti alla conclusione della gara
                    //int stmottimistica = 0; // somma teorica tempi mancanti vista ottimistica
                    //int stmpessimistica = 0; // somma teorica tempi mancanti vista ottimistica
                    codicetipogara = ds.Tables["gare"].Rows[r]["idtipogara"] == DBNull.Value ? "" : ds.Tables["gare"].Rows[r]["idtipogara"].ToString();
                    of1 = ds.Tables["gare"].Columns["f1"].Ordinal;
                    //if (codgara == "1211")
                    //    ss = "";
                    /*for (i = of1; i < of1 + 14; i++)  // calcolo tempi teorici
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

                    Int32.TryParse(ds.Tables["gare"].Rows[r]["revocata"].ToString(), out i);
                    if (i == 1) s = "revocata"; // solo se non è a lotti.....
                    
                    // 2^ colonna
                    TableCell tCell22 = new TableCell();
                    tCell22.Text = ds.Tables["gare"].Rows[r]["oggetto"].ToString();
                    tcStyle.HorizontalAlign = HorizontalAlign.Left;
                    tCell22.ApplyStyle(tcStyle);
                    tRow1.Cells.Add(tCell22); // 2

                    // 3^ colonna
                    TableCell tCell23a = new TableCell();
                    f = "";
                    if (ds.Tables["gare"].Rows[r]["url"] != DBNull.Value)
                        f = ds.Tables["gare"].Rows[r]["url"].ToString();
                    else
                        f = "http://www.appalti.provincia.tn.it";
                    s = String.Format("<a href=\"{0}\" target=\"_blank\">", f);
                    s += string.Format("<img src='{0}' alt='clicca per accedere alla documentazione di gara.' /></a>", "Folder20x20.png");
                    tCell23a.Text = s;
                    tcStyle.HorizontalAlign = HorizontalAlign.Center;
                    tCell23a.ApplyStyle(tcStyle);
                    tRow1.Cells.Add(tCell23a);

                    int nonaggiudicata = 0; // solo se gara ad n unico lotto
                    if (ds.Tables["gare"].Rows[r]["Nonaggiudicata"] != DBNull.Value)
                        nonaggiudicata = ds.Tables["gare"].Rows[r]["Nonaggiudicata"].ToString() == "False" ? 0 : 1;

                    // 4^ colonna STATO
                    // se gara con + lotti:     gara con xx lotti
                    // se gara NON aggiudicata: ''
                    // se gara RITIRATA:        'Revocata/Annullata'
                    TableCell tCell23 = new TableCell();
                    tCell23.Text = stato;
                    tCell23.Text = nonaggiudicata == 0 ? tCell23.Text : "non aggiudicata";
                    tRow1.Cells.Add(tCell23); // 3                
                    tCell23.Font.Bold = true;
                    tcStyle.HorizontalAlign = HorizontalAlign.Center;
                    tCell23.ApplyStyle(tcStyle);
					if ((stato == "procedura conclusa" && nonaggiudicata == 0) || stato == "revocata/annullata" )
					{
						tCell23.ForeColor = Color.Black;
						tCell23.BackColor = Color.LightGray;
					}
					else
					{
						tCell23.ForeColor = Color.Black;
						tCell23.BackColor = Color.White;
					}

					// 5^ colonna
					TableCell tCell24 = new TableCell();
                    s = nonaggiudicata == 1 ? "" : ds.Tables["gare"].Rows[r]["Denominazione"].ToString();
                    tCell24.Text = s;
                    tcStyle.HorizontalAlign = HorizontalAlign.Left;
                    tCell24.ApplyStyle(tcStyle);
                    tRow1.Cells.Add(tCell24); // 4

                    // 6^ colonna
                    TableCell tCell25 = new TableCell();
                    double euro = ds.Tables["gare"].Rows[r]["basedasta"] == DBNull.Value ? 0 : Convert.ToDouble(ds.Tables["gare"].Rows[r]["basedasta"]);
                    tCell25.Text = euro.ToString("c");
                    tRow1.Cells.Add(tCell25); // 5
                    tcStyle.HorizontalAlign = HorizontalAlign.Right;
                    tCell25.ApplyStyle(tcStyle);
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
						//if (codgara == "3270")
						//    ss = "";
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

						// 7^ colonna (semaforo)
						TableCell tCell26 = new TableCell();  // luce semaforica
						tCell26.Text = "";
						dcg = dif5;
						string vialibera = "";
						if ((int)(step * 0.9) >= sefp || (ufc >= 14 && sefp <= step))
						{ s = "luceverde20x20.png"; vialibera = "verde"; }
						else
							if (step > sefp)
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
						tCell26.Text = nonaggiudicata != 1 ? ss : "";
						// sefp = somma effettiva parziale; step = somma teorica parziale
						tcStyle.HorizontalAlign = HorizontalAlign.Center;
						tCell26.ApplyStyle(tcStyle);
						tRow1.Cells.Add(tCell26); // aggiungo cmq la cella sia che sia vuota che con img

						// 8^ colonna
						TableCell tCell27 = new TableCell();  // luce semaforica
															  //tCell26.Text += " " + sefp.ToString() + ":" + step.ToString()+" u"+ufc.ToString()+ " 5f"+dif5.ToShortDateString();
															  // se gara conclusa ?
						if (nonaggiudicata == 0)  // solo se un unico lotto
						{
							if (ds.Tables["gare"].Rows[r]["dataseduta1"] != DBNull.Value)
								tCell27.Text = String.Format("({0}gg.)", ((sefp - step) > 0 ? "+" : "") + (sefp - step).ToString("G"));
							else
								tCell27.Text = String.Format("{0:" + formatodata + "}", dif5.AddDays(bando + incorso));
						}
						else
							tCell27.Text = "";

						tcStyle.HorizontalAlign = HorizontalAlign.Center;
						tCell27.ApplyStyle(tcStyle);
						tRow1.Cells.Add(tCell27);

						// 9^ colonna rideterminazione
						TableCell tCell28 = new TableCell();  // luce semaforica
															  //s = ds.Tables["gare"].Rows[r]["rideterminazione"] == DBNull.Value ? "" : ds.Tables["gare"].Rows[r]["rideterminazione"].ToString();
						tCell28.Text = gcns > 0 ? dif5.AddDays(bando + gcns).ToString(formatodata) + " *" : "";
						tcStyle.HorizontalAlign = HorizontalAlign.Center;
						tCell28.ApplyStyle(tcStyle);
						tRow1.Cells.Add(tCell28);

						// aggiungo testo tempi
						/*
						  tCell26.Style.Add("text-align", "Midle");
						  tCell26.Style.Add("Verticalalign", "Middle");
						  tCell26.Style.Add("border-color", "black");
						//cel1.Style(HtmlTextWriterStyle.FontSize) = 9
						//TableCell tCell24 = new TableCell();
						//tCell24.Text = ds.Tables["gare"].Rows[r]["Denominazione"].ToString();
						//tRow1.Cells.Add(tCell24); */
					}
                }
                r++; // incremento numero di righe
            }
            tRighetrovate.Text = righetrovate.ToString() + " occorrenze trovate.";

            // navigazione
            TableRow navirow = new TableRow();
            TableCell tnavi = new TableCell();
            tnavi.Text = string.Format("<a href = \"default.aspx\"><img src = \"home.png\" alt = \"torna alla home page\" height=\"23\" width=\"23\"/></a>");
            navirow.Cells.Add(tnavi);
            TableCell tnavi1 = new TableCell();
            tnavi1.Text = "             ";
            navirow.Cells.Add(tnavi1);
            navirow.Cells.Add(tnavi1); // doppio
            TableCell tnavi2 = new TableCell();
            tnavi2.Text = "<a href=\"javascript: history.back()\"><img src = \"frecciasx.png\" alt = \"Vai alla pagina precedente\" /></a>";
            navirow.Cells.Add(tnavi2);
            navi2.Rows.Add(navirow); // aggiungo la riga alla tabella

            // aggiungo spiegazioni
            TableRow tR1 = new TableRow(); Tu.Rows.Add(tR1);
            TableCell tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC1 = new TableCell(); tC1.Text = String.Format("<img src='{0}' />", "lentepiu28x28.png"); tR1.Cells.Add(tC1); tC1.Style.Add("text-align", "Center");
            TableCell tC11 = new TableCell(); tC11.Text = "Visualizza ulteriori dettagli;"; tR1.Cells.Add(tC11); tC11.Style.Add("text-align", "Left");
            //----------------
            tR1 = new TableRow(); Tu.Rows.Add(tR1);
            tcSpace = new TableCell(); tcSpace.Text = ""; tR1.Cells.Add(tcSpace);
            TableCell tC2 = new TableCell(); tC2.Text = String.Format("<img src='{0}' />", "Folder33x33.jpg"); tR1.Cells.Add(tC2); tC2.Style.Add("text-align", "Center"); tR1.Cells.Add(tC2); tC2.Style.Add("text-align", "Center");
            s = "Per accedere a tutti i documenti della procedura di gara pubblicati nella sezione Bandi e Appalti del portale dell'Agenzia Provinciale per gli Appalti e Contratti;";
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
            tRighetrovate.Text = "0 occorrenze trovate.";
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

    protected string codicegara()
    {
        string cg = "";
        cg = rawstringa;
        //parametri = Request.QueryString["id"];
        cg = cg.Substring(cg.IndexOf("id=") + 3);
        int fine = cg.IndexOf("l=") - 1;
        cg = cg.Substring(0, fine);
        return (cg);
    }
    protected void testa()
    {
        idgara = codicegara();        //string par = Request.QueryString["loggato"];
        //loggato = par == "si" ? true : false;
        //Session.Add("arrivo_da", (string)"default"); // vediamo se lo fa ogni volta che carico la pagina o solo la prima volta

        strSelect = "select richieste.id, richieste.titolo, richieste.ente, richieste.tipologia_ek, richieste.procedura_EK, richieste.criterioaggiudicazione_EK, richieste.appaltointegrato,  lotto.id, lotto.lotto, lotto.basedasta, lotto.oggetto, classificazione, procedura, criterio, datigara.* ";
        strSelect += "FROM richieste LEFT JOIN Lotto ON richieste.id = Lotto.Lottorichiesta_EK ";
        strSelect += "LEFT JOIN datigara ON richieste.id = datigara.richiesta_ek ";
        strSelect += "LEFT JOIN tipologia ON richieste.tipologia_ek = tipologia.id ";
        strSelect += "LEFT JOIN procedura ON richieste.procedura_ek = procedura.id ";
        strSelect += "LEFT JOIN criterioaggiudicazione ON richieste.criterioaggiudicazione_EK = criterioaggiudicazione.idca ";
        strSelect += "where richieste.id = " + idgara;
        sqlcnn = SQLClass.openaSQLConn(out ms);  // creo e apro la connessione
        if (sqlcnn.State == ConnectionState.Open)
        {
            if (getdata(strSelect, ds, "lottiselect", sqlcnn) > 0)  // carico gli enti e li aggiungo alla ddlEnti 
            {
                // tStato.Text = string.Format("Nome tabella letta: {0}, n. righe: {1}", ds.Tables[0].TableName, ds.Tables[0].Rows.Count);
                ddlLotti.Items.Clear();
                ddlLotti.Items.Insert(0, ""); // inserisco uno spazio
                lotti = ds.Tables["lottiselect"].Rows.Count;
                for (int i = 0; i < ds.Tables["lottiselect"].Rows.Count; i++)
                {
                    ddlLotti.Items.Insert(i + 1, ds.Tables["lottiselect"].Rows[i]["lotto"].ToString());
                    basedastasomma += ds.Tables["lottiselect"].Rows[i]["basedasta"] == DBNull.Value ? 0 : Convert.ToDouble(ds.Tables["lottiselect"].Rows[i]["basedasta"]);
                }
            }
        }
        sqlcnn.Close();

        if (ddlstato.Items.Count == 0)
        {
			ddlstato.Items.Insert(0, "");
			ddlstato.Items.Insert(1, "bandita");
			ddlstato.Items.Insert(2, "in corso");
			ddlstato.Items.Insert(3, "aggiudicata");
			ddlstato.Items.Insert(4, "stipula del contratto");
			ddlstato.Items.Insert(5, "contratto stipulato");
			ddlstato.Items.Insert(6, "procedura conclusa");
		}

        // descrivo dati comuni
        TableRow tRow = new TableRow();
        tdatidettaglio.Rows.Add(tRow);  // aggiungo riga

        // 1^ riga: 1^ colonna
        TableCell tCell1 = new TableCell();
        tCell1.Text = "committente".ToString();
        tRow.Cells.Add(tCell1); // 0

        // 1^ riga: 2^ colonna
        TableCell tCell2 = new TableCell();
        tCell2.Text = "".ToString();
        tRow.Cells.Add(tCell2);

        // 1^ riga: 3^ colonna
        TableCell tCell3 = new TableCell();
        tCell3.Text = ds.Tables["lottiselect"].Rows[0]["ente"].ToString();
        tRow.Cells.Add(tCell3); // 0

        TableRow tRowvuota = new TableRow();
        tdatidettaglio.Rows.Add(tRowvuota);  // aggiungo riga

        // seconda riga    
        TableRow tRow2 = new TableRow();
        tdatidettaglio.Rows.Add(tRow2);  // aggiungo riga
                                         // 2^ riga: 1^ colonna                
        TableCell tCell21 = new TableCell();
        tCell21.Text = "oggetto della gara".ToString();
        tRow2.Cells.Add(tCell21); // 0

        // 2^ riga: 2^ colonna
        TableCell tCell22 = new TableCell();
        tCell22.Text = "".ToString();
        tRow2.Cells.Add(tCell22);

        // 2^ riga: 3^ colonna
        TableCell tCell23 = new TableCell();
        tCell23.Text = ds.Tables["lottiselect"].Rows[0]["titolo"].ToString();
        tRow2.Cells.Add(tCell23); // 0

        // terza riga
        TableRow tRow3 = new TableRow();
        tdatidettaglio.Rows.Add(tRow3);  // aggiungo riga

        // 3^ riga: 1^ colonna
        TableCell tCell31 = new TableCell();
        tCell31.Text = "importo a base di gara".ToString();
        tRow3.Cells.Add(tCell31); // 0

        // 3^ riga: 2^ colonna
        TableCell tCell32 = new TableCell();
        tCell32.Text = "".ToString();
        tRow3.Cells.Add(tCell32);

        // 3^ riga: 3^ colonna
        TableCell tCell33 = new TableCell();
        tCell33.Text = basedastasomma.ToString("c");
        //tCell33.Text = ds.Tables["gare"].Rows[r]["basedasta"].ToString();
        tRow3.Cells.Add(tCell33); // 0

        // terza riga a
        TableRow tRow3a = new TableRow();
        tdatidettaglio.Rows.Add(tRow3a);  // aggiungo riga

        // 3^ riga: 1^ colonna
        TableCell tCell31a = new TableCell();
        tCell31a.Text = "numero lotti".ToString();
        tRow3a.Cells.Add(tCell31a); // 0

        // 3^ riga: 2^ colonna
        TableCell tCell32a = new TableCell();
        tCell32a.Text = "".ToString();
        tRow3a.Cells.Add(tCell32a);

        // 3^ riga: 3^ colonna
        TableCell tCell33a = new TableCell();
        tCell33a.Text = lotti.ToString();
        //tCell33.Text = ds.Tables["gare"].Rows[r]["basedasta"].ToString();
        tRow3a.Cells.Add(tCell33a); // 0

        TableRow tRow4 = new TableRow();
        tdatidettaglio.Rows.Add(tRow4);  // aggiungo riga

        // 4^ riga: 1^ colonna                
        TableCell tCell41 = new TableCell();
        tCell41.Text = "tipologia".ToString();
        tRow4.Cells.Add(tCell41); // 0

        // 4^ riga: 2^ colonna
        TableCell tCell42 = new TableCell();
        tCell42.Text = "".ToString();
        tRow4.Cells.Add(tCell42);

        // 4^ riga: 3^ colonna
        TableCell tCell43 = new TableCell();
        tCell43.Text = ds.Tables["lottiselect"].Rows[0]["classificazione"].ToString();
        tRow4.Cells.Add(tCell43); // 0

        // quinta riga    
        TableRow tRow5 = new TableRow();
        tdatidettaglio.Rows.Add(tRow5);  // aggiungo riga
                                         // 5^ riga: 1^ colonna                
        TableCell tCell51 = new TableCell();
        tCell51.Text = "procedura di affidamento".ToString();
        tRow5.Cells.Add(tCell51); // 0

        // 5^ riga: 2^ colonna
        TableCell tCell52 = new TableCell();
        tCell52.Text = "".ToString();
        tRow5.Cells.Add(tCell52);

        // 5^ riga: 3^ colonna
        TableCell tCell53 = new TableCell();
        tCell53.Text = ds.Tables["lottiselect"].Rows[0]["procedura"].ToString();
        tRow5.Cells.Add(tCell53); // 0

        // sesta riga    
        TableRow tRow6 = new TableRow();
        tdatidettaglio.Rows.Add(tRow6);  // aggiungo riga
                                         // 6^ riga: 1^ colonna                
        TableCell tCell61 = new TableCell();
        tCell61.Text = "criterio di aggiudicazione".ToString();
        tRow6.Cells.Add(tCell61); // 0

        // 6^ riga: 2^ colonna
        TableCell tCell62 = new TableCell();
        tCell62.Text = "".ToString();
        tRow6.Cells.Add(tCell62);

        // 6^ riga: 3^ colonna
        TableCell tCell63 = new TableCell();
        tCell63.Text = ds.Tables["lottiselect"].Rows[0]["criterio"].ToString();
        tRow6.Cells.Add(tCell63); // 0

        //int.TryParse(ds.Tables["gare"].Rows[r]["ultimafaseconclusa"].ToString(), out ufc);

        DateTime dufc = Convert.ToDateTime("1900-1-1");  // data ultima fase conclusa
        DateTime dsf = DateTime.Now; // data scadenza fase attuale
        DateTime dif5 = Convert.ToDateTime("1900-1-1");  // data inizio fase 5
        DateTime dff5 = Convert.ToDateTime("1900-1-1");  // data fine fase 5
        DateTime dsa = Convert.ToDateTime("1900-1-1");   // data seduta di aggiudicazione
        DateTime daa = Convert.ToDateTime("1900-1-1");   // data avviso di aggiudicazione
        DateTime dcg = Convert.ToDateTime("1900-1-1");   // data conclusione prevista della gara

        if (ds.Tables["lottiselect"].Rows[0]["datapubblicazionebando"] != DBNull.Value)
            dif5 = Convert.ToDateTime(ds.Tables["lottiselect"].Rows[0]["datapubblicazionebando"]);
        else
            if (ds.Tables["lottiselect"].Rows[0]["DataSpedizioneInvito"] != DBNull.Value)
            dif5 = Convert.ToDateTime(ds.Tables["lottiselect"].Rows[0]["DataSpedizioneInvito"]);

        if (ds.Tables["lottiselect"].Rows[0]["datascadenzapresentazioneofferte"] != DBNull.Value)
            dff5 = Convert.ToDateTime(ds.Tables["lottiselect"].Rows[0]["datascadenzapresentazioneofferte"]);
        // tempitipo = "" & rs.Fields("tipologia_ek") & "" & rs.Fields("procedura_ek") & "" & rs.Fields("criterioaggiudicazione_EK") & "" & IIf(rs.Fields("appaltointegrato"), "AI", "NI") & "" & IIf(rs.Fields("Soprasogliacomunitaria"), "SU", "SO")

        codicetipogara = calcolatipogara(ds.Tables["lottiselect"].Rows[0]["tipologia_EK"].ToString(), ds.Tables["lottiselect"].Rows[0]["procedura_EK"].ToString(), ds.Tables["lottiselect"].Rows[0]["criterioaggiudicazione_EK"].ToString(), ds.Tables["lottiselect"].Rows[0]["appaltointegrato"].ToString(), ds.Tables["lottiselect"].Rows[0]["Soprasogliacomunitaria"].ToString());

        // ottava riga    
        TableRow tRow8 = new TableRow();
        tdatidettaglio.Rows.Add(tRow8);  // aggiungo riga
                                         // 8^ riga: 1^ colonna                
        TableCell tCell81 = new TableCell();
        s = ds.Tables["lottiselect"].Rows[0]["datapubblicazionebando"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables["lottiselect"].Rows[0]["datapubblicazionebando"]).ToString(formatodata);
        if (s == "")
        {
            ms = "data spedizione inviti";
            s = ds.Tables["lottiselect"].Rows[0]["dataspedizioneinvito"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables["lottiselect"].Rows[0]["dataspedizioneinvito"]).ToString(formatodata);
        }
        else
            ms = "data pubblicazione bando";

        tCell81.Text = ms;
        tRow8.Cells.Add(tCell81); // 0

        // 8^ riga: 2^ colonna
        TableCell tCell82 = new TableCell();
        tCell82.Text = "".ToString();
        tRow8.Cells.Add(tCell82);

        // 8^ riga: 3^ colonna
        TableCell tCell83 = new TableCell();
        //s = string.Format(formatodata);
        tCell83.Text = s;
        tRow8.Cells.Add(tCell83); // 0

        // nona riga    
        TableRow tRow9 = new TableRow();
        tdatidettaglio.Rows.Add(tRow9);  // aggiungo riga
                                         // 9^ riga: 1^ colonna                
        TableCell tCell91 = new TableCell();
        tCell91.Text = "data scadenza presentazione offerte".ToString();
        tRow9.Cells.Add(tCell91); // 0

        // 9^ riga: 2^ colonna
        TableCell tCell92 = new TableCell();
        tCell92.Text = "".ToString();
        tRow9.Cells.Add(tCell92);

        // 9^ riga: 3^ colonna
        TableCell tCell93 = new TableCell();
        s = ds.Tables["lottiselect"].Rows[0]["datascadenzapresentazioneofferte"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables["lottiselect"].Rows[0]["datascadenzapresentazioneofferte"]).ToString(formatodata);
        tCell93.Text = s;
        tRow9.Cells.Add(tCell93); // 0

        // navigazione
        TableRow navirow = new TableRow();
        TableCell tnavi = new TableCell();
        tnavi.Text = string.Format("<a href = \"default.aspx\"><img src = \"home.png\" alt = \"torna alla home page\" height=\"23\" width=\"23\"/></a>");
        navirow.Cells.Add(tnavi);
        TableCell tnavi1 = new TableCell();
        tnavi1.Text = "             ";
        navirow.Cells.Add(tnavi1);
        navirow.Cells.Add(tnavi1); // doppio
        TableCell tnavi2 = new TableCell();
        tnavi2.Text = "<a href=\"javascript: history.back()\"><img src = \"frecciasx.png\" alt = \"Vai alla pagina precedente\" /></a>";
        navirow.Cells.Add(tnavi2);
        navi.Rows.Add(navirow); // aggiungo la riga alla tabella
    }
    protected void cbreset_Click(object sender, EventArgs e)
    {
        toggetto.Text = "";
        ddlstato.Text = "";
        tStato.Text = "";
        tRighetrovate.Text = "";
        cbContenzioso.Checked = false;
        //Asterisco.Text = "";
        testa();
    }

    protected string calcolatipogara(string tipologia_EK, string procedura_EK, string criterioaggiudicazione_EK, string appaltointegrato, string Soprasogliacomunitaria)
    {
        string codice = "";
        tipologia_EK = tipologia_EK == null || tipologia_EK.Length <= 0 ? "" : tipologia_EK.Trim();
        procedura_EK = procedura_EK == null || procedura_EK.Length <= 0 ? "" : procedura_EK.Trim();
        criterioaggiudicazione_EK = criterioaggiudicazione_EK == null || criterioaggiudicazione_EK.Length <= 0 ? "" : criterioaggiudicazione_EK.Trim();
        appaltointegrato = appaltointegrato == null ? "" : appaltointegrato.Trim();
        appaltointegrato = appaltointegrato == "true" ? "AI" : "NI";
        Soprasogliacomunitaria = Soprasogliacomunitaria == null ? "" : Soprasogliacomunitaria.Trim();
        Soprasogliacomunitaria = Soprasogliacomunitaria == "true" ? "SU" : "SO";
        codice = (tipologia_EK != "" && procedura_EK != "" && criterioaggiudicazione_EK != "" && appaltointegrato != "" && Soprasogliacomunitaria != "") ?
            tipologia_EK + procedura_EK + criterioaggiudicazione_EK  + appaltointegrato + Soprasogliacomunitaria : "";
        return (codice);
    }
}



/*
 * protected void Page_Load(object sender, EventArgs e)
{
    // The data for the pie chart
    double[] data = {25, 18, 15, 12, 8, 30, 35};

    // The labels for the pie chart
    string[] labels = {"Labor", "Licenses", "Taxes", "Legal", "Insurance", "Facilities",
        "Production"};

    // Create a PieChart object of size 360 x 300 pixels
    PieChart c = new PieChart(360, 300);

    // Set the center of the pie at (180, 140) and the radius to 100 pixels
    c.setPieSize(180, 140, 100);

    // Set the pie data and the pie labels
    c.setData(data, labels);

    // Output the chart
    WebChartViewer1.Image = c.makeWebImage(Chart.PNG);

    // Include tool tip for the chart
    WebChartViewer1.ImageMap = c.getHTMLImageMap("", "", "title='{label}: US${value}K ({percent}%)'"
        );
}

*/