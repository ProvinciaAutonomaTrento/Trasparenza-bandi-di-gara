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
    public DataSet DSet = new DataSet();
    public SqlConnection sqlcnn;
    public ConnessioneSQL SQLClass = new ConnessioneSQL();
    public SqlDataAdapter da = new SqlDataAdapter();
    public string strSelect = "";
    public DataSet ds = new DataSet();
    public bool loggato;
    public string ms = "";
    public string formatoeuro = "###.###.###";
    public string formatodata = "dd-MM-yyyy";
    public DateTime data;
    public int[] tempi = new int[20];
    public string s = "";
    TableItemStyle tcStyle = new TableItemStyle();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)  // SOLO LA PRIMA VOLTA CHE CARICO LA PAGINA.... vedi comando in accessi o altro
        {
            string parametri = Request.RawUrl;            
            parametri = parametri.Substring(parametri.IndexOf("id=") + 3);
            sqlcnn = SQLClass.openaSQLConn(out ms);
            if (sqlcnn.State == ConnectionState.Open)
            {
                int i = 0, fine = 0;
                string s, f = "";

                //< img alt = "Vai alla pagina precedente" longdesc = "Vai alla pagina precedente" src = "frecciasx.png" OnClientClick = "JavaScript: window.history.back(1); return false;" runat = "server" style = "width: 24px; height: 23px" />< br />
                // <a href="#" onclick="history.go(-1);return false;">Back</a>
                //s = string.Format("<a href = '#' onclick='history.go(-1); return false;'>< img alt = 'torna alla home page' longdesc = 'torna alla home page' src = 'home.png' style = 'width: 23px; height: 23px'/></ a >");
                //tnavi.Text = s; navi.Cells.Add(tnavi);

                bool mettolamail = parametri.Substring(parametri.IndexOf("s=") + 2, 1) == "3" ? true : false; 
                
                i = 0; fine = parametri.IndexOf("l=") - 1;
                f = " richieste.id=" + parametri.Substring(i, fine);
                i = parametri.IndexOf("l=") +2; fine = parametri.IndexOf("s=") - 1;
                f += " and lotto.id=" + parametri.Substring(i,fine-i);
                /*s = "SELECT richieste.codice, richieste.id, richieste.anno, richieste.datarichiesta, richieste.ente, tipoente.enteesterno, richieste.tipologia_EK, richieste.settoretipologialavoro_EK, settore.descrizione, richieste.RiNote, richieste.tempiapprovazioneavvio, richieste.sistemaaffidamento_EK, richieste.procedura_EK, procedura.procedura, richieste.criterioaggiudicazione_EK, richieste.proposta_EK, proposta.proposta, richieste.propostadata, richieste.funzionarioindividuato_EK, richieste.dataaccettazioneproposta, richieste.idonea, richieste.Revocata, Lotto.RevocataAnnullata, richieste.Ritirata, Lotto.web, richieste.TipologiaLavori_EK, tipologia.classificazione, Criterioaggiudicazione.Criterio, Datigara.Soprasogliacomunitaria, Datigara.DatarichiestachiarimentistrutturaEnte, Datigara.DatarispostachiarimentistrutturaEnte, Datigara.Datapubblicazionebando, Datigara.Datascadenzapresentazioneistanzedipartecipazione, Datigara.DataSpedizioneInvito, Datigara.Datascadenzapresentazioneofferte, Datigara.DataPRESUNTASCADENZAPRESENTAZIONEOFFERTE, Lotto.Dataricezioneverbalicommissionetecnica, Lotto.Lotto, Lotto.*, statogara.descrizione, qryTizTempisticaSeduta3.dataSeduta3, qryTizTempisticaSeduta1.dataSeduta1, Datigara.Soprasogliacomunitaria, ";
                s += "Fasi.cod, Datimonitoraggio.ultimafaseconclusa, [ultimafaseconclusa] AS duratafa, descrizionefasi.[descrizione fase], Ditte.Denominazione, richieste.dataassegnazioneincarico, richieste.Servizio_competente, tipoente.tipoente, richieste.RiNote, richieste.Note, richieste.GaraTelematica, Datigara.DataNominaCommTec, datigara.url, Ditte.Denominazione, qryTizTempiPianificazione.lotto_ek, qryTizTempiPianificazione.MaxDidatestamp, pianificazione.Bando, pianificazione.InCorso, Lotto.web, Datimonitoraggio.ultimafaseconclusa ";
                s += "FROM ((Ditte RIGHT JOIN((procedura RIGHT JOIN((proposta RIGHT JOIN(settore RIGHT JOIN(Criterioaggiudicazione RIGHT JOIN(tipologia RIGHT JOIN(funzionari RIGHT JOIN((((tipoente RIGHT JOIN(ElencoStrutture RIGHT JOIN(((richieste LEFT JOIN Datigara ON richieste.id = Datigara.Richiesta_EK) LEFT JOIN Lotto ON richieste.id = Lotto.Lottorichiesta_EK) LEFT JOIN qryTizTempisticaSeduta3 ON Lotto.id = qryTizTempisticaSeduta3.Consulenza_EK) ON ElencoStrutture.Struttura = richieste.ente) ON tipoente.id = ElencoStrutture.TipoEnte_EK) LEFT JOIN Datimonitoraggio ON(Lotto.Lotto = Datimonitoraggio.Lotto) AND(Lotto.Lottorichiesta_EK = Datimonitoraggio.IDgara_EK)) LEFT JOIN Fasi ON Datimonitoraggio.idtipogara_ek = Fasi.idtipogara) LEFT JOIN statogara ON Lotto.StatoBando_EK = statogara.id) ON funzionari.id = richieste.funzionarioindividuato_EK) ON tipologia.id = richieste.tipologia_EK) ON Criterioaggiudicazione.Idca = richieste.criterioaggiudicazione_EK) ON settore.ID = richieste.settoretipologialavoro_EK) ON proposta.id = richieste.proposta_EK) LEFT JOIN qryTizTempisticaSeduta1 ON Lotto.id = qryTizTempisticaSeduta1.Consulenza_EK) ON procedura.id = richieste.procedura_EK) LEFT JOIN descrizionefasi ON Datimonitoraggio.ultimafaseconclusa = descrizionefasi.cofa) ON Ditte.id = Lotto.Impresaaggiudicataria_EK) LEFT JOIN qryTizTempiPianificazione ON Lotto.id = qryTizTempiPianificazione.lotto_ek) LEFT JOIN pianificazione ON (qryTizTempiPianificazione.lotto_ek = pianificazione.lotto_ek) AND(qryTizTempiPianificazione.MaxDidatestamp = pianificazione.datestamp) ";
                s += "WHERE (((richieste.proposta_EK) = 2) AND ((richieste.idonea) = 0 Or (richieste.idonea) Is Null) AND((richieste.Revocata) = 0) AND((Lotto.RevocataAnnullata) = 0 Or(Lotto.RevocataAnnullata) Is Null) AND((richieste.Ritirata) = 0 Or(richieste.Ritirata) Is Null) AND ((Lotto.web) = 1) AND ((Datigara.Soprasogliacomunitaria) = 1) AND((Datimonitoraggio.ultimafaseconclusa) >= 3))  and  (contenzioso = 0 or contenzioso is null)  ORDER BY richieste.codice, Lotto.Lotto ";
                */

                s = "SELECT richieste.codice, richieste.id, richieste.anno, richieste.datarichiesta, richieste.ente, tipoente.enteesterno, richieste.tipologia_EK, richieste.settoretipologialavoro_EK, settore.descrizione, richieste.RiNote, richieste.tempiapprovazioneavvio, richieste.sistemaaffidamento_EK, richieste.procedura_EK, procedura.procedura, richieste.criterioaggiudicazione_EK, richieste.proposta_EK, proposta.proposta, richieste.propostadata, richieste.funzionarioindividuato_EK, richieste.dataaccettazioneproposta, richieste.idonea, richieste.Revocata, Lotto.RevocataAnnullata, richieste.Ritirata, richieste.appaltointegrato, Lotto.web, richieste.TipologiaLavori_EK, tipologia.classificazione, Criterioaggiudicazione.Criterio, Datigara.Soprasogliacomunitaria, Datigara.DatarichiestachiarimentistrutturaEnte, Datigara.DatarispostachiarimentistrutturaEnte, Datigara.Datapubblicazionebando, Datigara.Datascadenzapresentazioneistanzedipartecipazione, Datigara.DataSpedizioneInvito, Datigara.Datascadenzapresentazioneofferte, Datigara.DataPRESUNTASCADENZAPRESENTAZIONEOFFERTE, Lotto.Dataricezioneverbalicommissionetecnica, Lotto.Lotto, Lotto.*, statogara.descrizione, qryTizTempisticaSeduta3.dataSeduta3, qryTizTempisticaSeduta1.dataSeduta1, Datigara.Soprasogliacomunitaria, Fasi.cod, Datimonitoraggio.ultimafaseconclusa, [ultimafaseconclusa] AS duratafa, descrizionefasi.[descrizione fase], Ditte.Denominazione, richieste.dataassegnazioneincarico, richieste.Servizio_competente, tipoente.tipoente, richieste.RiNote, richieste.Note, richieste.GaraTelematica, Datigara.DataNominaCommTec, datigara.url, Ditte.Denominazione, qryTizTempiPianificazione.lotto_ek, qryTizTempiPianificazione.MaxDidatestamp, pianificazione.Bando, pianificazione.InCorso, Lotto.web, Lotto.notetutor ";
                s += "FROM ((Ditte RIGHT JOIN((procedura RIGHT JOIN((proposta RIGHT JOIN(settore RIGHT JOIN(Criterioaggiudicazione RIGHT JOIN(tipologia RIGHT JOIN(funzionari RIGHT JOIN((((tipoente RIGHT JOIN(ElencoStrutture RIGHT JOIN(((richieste LEFT JOIN Datigara ON richieste.id = Datigara.Richiesta_EK) LEFT JOIN Lotto ON richieste.id = Lotto.Lottorichiesta_EK) LEFT JOIN qryTizTempisticaSeduta3 ON Lotto.id = qryTizTempisticaSeduta3.Consulenza_EK) ON ElencoStrutture.Struttura = richieste.ente) ON tipoente.id = ElencoStrutture.TipoEnte_EK) LEFT JOIN Datimonitoraggio ON(Lotto.Lotto = Datimonitoraggio.Lotto) AND(Lotto.Lottorichiesta_EK = Datimonitoraggio.IDgara_EK)) LEFT JOIN Fasi ON Datimonitoraggio.idtipogara_ek = Fasi.idtipogara) LEFT JOIN statogara ON Lotto.StatoBando_EK = statogara.id) ON funzionari.id = richieste.funzionarioindividuato_EK) ON tipologia.id = richieste.tipologia_EK) ON Criterioaggiudicazione.Idca = richieste.criterioaggiudicazione_EK) ON settore.ID = richieste.settoretipologialavoro_EK) ON proposta.id = richieste.proposta_EK) LEFT JOIN qryTizTempisticaSeduta1 ON Lotto.id = qryTizTempisticaSeduta1.Consulenza_EK) ON procedura.id = richieste.procedura_EK) LEFT JOIN descrizionefasi ON Datimonitoraggio.ultimafaseconclusa = descrizionefasi.cofa) ON Ditte.id = Lotto.Impresaaggiudicataria_EK) LEFT JOIN qryTizTempiPianificazione ON Lotto.id = qryTizTempiPianificazione.lotto_ek) LEFT JOIN pianificazione ON(qryTizTempiPianificazione.lotto_ek = pianificazione.lotto_ek) AND(qryTizTempiPianificazione.MaxDidatestamp = pianificazione.datestamp) ";
                s += "WHERE " + (f != "" ? " " + f + " " : "");
                s += "ORDER BY richieste.codice, Lotto.Lotto";

				if (getdata(s, ds, "gare", sqlcnn) > 0)  // carico i dati della gara 
                {
                    DateTime oggi = DateTime.Now;
                    tStato.Text = "";
                    string stato = "";
                    double euro;

                    //tRighetrovate.Text = ds.Tables["gare"].Rows.Count + " occorrenze trovate.";
                    for (int r = 0; r < ds.Tables["gare"].Rows.Count; r++)  // tanto c'è solo una riga
                    {
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
                        tCell3.Text = ds.Tables["gare"].Rows[r]["ente"].ToString();
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
                        tCell23.Text = ds.Tables["gare"].Rows[r]["oggetto"].ToString();
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
                        double.TryParse(ds.Tables["gare"].Rows[r]["basedasta"].ToString(), out euro);
                        tCell33.Text = euro.ToString("c");
                        //tCell33.Text = ds.Tables["gare"].Rows[r]["basedasta"].ToString();
                        tRow3.Cells.Add(tCell33); // 0

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
                        tCell43.Text = ds.Tables["gare"].Rows[r]["classificazione"].ToString();
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
                        tCell53.Text = ds.Tables["gare"].Rows[r]["procedura"].ToString();
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
                        tCell63.Text = ds.Tables["gare"].Rows[r]["criterio"].ToString();
                        tRow6.Cells.Add(tCell63); // 0

                        //int.TryParse(ds.Tables["gare"].Rows[r]["ultimafaseconclusa"].ToString(), out ufc);

                        DateTime dif5 = Convert.ToDateTime("1900-1-1");  // data inizio fase 5
                        DateTime dff5 = Convert.ToDateTime("1900-1-1");  // data fine fase 5
                        DateTime dsa = Convert.ToDateTime("1900-1-1");   // data seduta di aggiudicazione
                        int gbs = 0;  // giorni bando stimati
                        int gcs = 0;  // giorni in corso stima
                        int bando = 0;
                        Int32.TryParse(ds.Tables["gare"].Rows[r]["bando"].ToString(), out gbs);
                        Int32.TryParse(ds.Tables["gare"].Rows[r]["incorso"].ToString(), out gcs);
                        if (ds.Tables["gare"].Rows[r]["datapubblicazionebando"] != DBNull.Value)
                            dif5 = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["datapubblicazionebando"]);
                        else
                            if (ds.Tables["gare"].Rows[r]["DataSpedizioneInvito"] != DBNull.Value)
                            dif5 = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["DataSpedizioneInvito"]);
                        if (ds.Tables["gare"].Rows[r]["dataseduta1"] != DBNull.Value)
                            dsa = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["dataseduta1"]);

                        if (gbs > 0) bando = gbs;
                        if (ds.Tables["gare"].Rows[r]["datascadenzapresentazioneofferte"] != DBNull.Value)
                            dff5 = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["datascadenzapresentazioneofferte"]);
                        else
                            dff5 = dif5.AddDays(bando);

                        if (dif5.ToShortDateString().ToString() != "01/01/1900")
                            stato = "bandita";
                        if (dff5.ToShortDateString().ToString() != "01/01/1900" && dff5 <= oggi)
                            stato = "in corso";
                        if (dsa.ToShortDateString().ToString() != "01/01/1900")
                            stato = "aggiudicata";
                        if (dsa.ToShortDateString().ToString() != "01/01/1900" && dsa.AddDays(45) < oggi)
                            stato = "stipula del contratto";

                        // tempitipo = "" & rs.Fields("tipologia_ek") & "" & rs.Fields("procedura_ek") & "" & rs.Fields("criterioaggiudicazione_EK") & "" & IIf(rs.Fields("appaltointegrato"), "AI", "NI") & "" & IIf(rs.Fields("Soprasogliacomunitaria"), "SU", "SO")
                        string codicetipogara = "";
                        codicetipogara = calcolatipogara(ds.Tables["gare"].Rows[r]["tipologia_EK"].ToString(), ds.Tables["gare"].Rows[r]["procedura_EK"].ToString(), ds.Tables["gare"].Rows[r]["criterioaggiudicazione_EK"].ToString(), ds.Tables["gare"].Rows[r]["appaltointegrato"].ToString(), ds.Tables["gare"].Rows[r]["Soprasogliacomunitaria"].ToString());

                        // settima riga    
                        TableRow tRow7 = new TableRow();
                        tdatidettaglio.Rows.Add(tRow7);  // aggiungo riga
                        // 7^ riga: 1^ colonna                
                        TableCell tCell71 = new TableCell();
                        tCell71.Text = "stato procedura".ToString();
                        tRow7.Cells.Add(tCell71); // 0

                        // 7^ riga: 2^ colonna
                        TableCell tCell72 = new TableCell();
                        tCell72.Text = "".ToString();
                        tRow7.Cells.Add(tCell72);

                        // 7^ riga: 3^ colonna
                        TableCell tCell73 = new TableCell();
                        tCell73.Text = stato;   // + "      " + codicetipogara;
                        tRow7.Cells.Add(tCell73); // 0

                        // ottava riga    
                        TableRow tRow8 = new TableRow();
                        tdatidettaglio.Rows.Add(tRow8);  // aggiungo riga
                        // 8^ riga: 1^ colonna                
                        TableCell tCell81 = new TableCell();
                        s = ds.Tables["gare"].Rows[r]["datapubblicazionebando"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables["gare"].Rows[r]["datapubblicazionebando"]).ToString(formatodata);
                        if (s == "")
                        {
                            ms = "data spedizione inviti";
                            s = ds.Tables["gare"].Rows[r]["dataspedizioneinvito"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables["gare"].Rows[r]["dataspedizioneinvito"]).ToString(formatodata);
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
                        s = ds.Tables["gare"].Rows[r]["datascadenzapresentazioneofferte"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables["gare"].Rows[r]["datascadenzapresentazioneofferte"]).ToString(formatodata);
                        tCell93.Text = s;
                        tRow9.Cells.Add(tCell93); // 0

                        // visualizzo dati sedute
                        DataRow[] doc;
                        DataRow[] offerte;
                        DataRow[] economica;
                        string docu = "", off = "", eco = "";
                        sqlcnn = SQLClass.openaSQLConn(out ms);
                        if (sqlcnn.State == ConnectionState.Open)
                        {
                            i = parametri.IndexOf("l=") + 2;
                            fine = parametri.IndexOf("s=") - 1;
                            s = "select * from sedutedigara where consulenza_ek = " + parametri.Substring(i, fine-i)  + " order by data desc";
                            if (getdata(s, ds, "sedute", sqlcnn) > 0) // carico le sedute di gara nel dataset
                            {
                                int rr = ds.Tables["sedute"].Rows.Count;
                                try
                                {
                                    doc = ds.Tables["sedute"].Select("classificazione_ek = '2'");
                                    docu = doc.Length > 0 ? Convert.ToDateTime(doc[0]["data"]).ToString(formatodata) : "";
                                    offerte = ds.Tables["sedute"].Select("classificazione_ek = '3'");
                                    off = offerte.Length > 0 ? Convert.ToDateTime(offerte[0]["data"]).ToString(formatodata) : "";
                                    economica = ds.Tables["sedute"].Select("classificazione_ek = '4'");
                                    eco = economica.Length > 0 ? Convert.ToDateTime(economica[0]["data"]).ToString(formatodata) : "";
                                }
                                catch (Exception ex)
                                {
                                    ms = "non ci sono le sedute ricercate : " + ex;
                                }
                            }
                        }

                        // Seduta documentazione
                        // 11^ riga    
                        TableRow tRow11 = new TableRow();
                        tdatidettaglio.Rows.Add(tRow11);  // aggiungo riga
                        // 11^ riga: 1^ colonna                
                        TableCell tCell111 = new TableCell();
                        tCell111.Text = "seduta apertura documentazione amministrativa".ToString();
                        tRow11.Cells.Add(tCell111); // 0

                        // 11^ riga: 2^ colonna
                        TableCell tCell112 = new TableCell();
                        tCell112.Text = "".ToString();
                        tRow11.Cells.Add(tCell112);

                        // 11^ riga: 3^ colonna
                        TableCell tCell113 = new TableCell();
                        tCell113.Text = docu;
                        tRow11.Cells.Add(tCell113); // 0

                        // Seduta apertura offerte tecniche
                        // 12^ riga    
                        TableRow tRow12 = new TableRow();
                        tdatidettaglio.Rows.Add(tRow12);  // aggiungo riga
                        // 12^ riga: 1^ colonna                
                        TableCell tCell121 = new TableCell();
                        tCell121.Text = "seduta apertura offerte tecniche".ToString();
                        tRow12.Cells.Add(tCell121); // 0

                        // 12^ riga: 2^ colonna
                        TableCell tCell122 = new TableCell();
                        tCell122.Text = "".ToString();
                        tRow12.Cells.Add(tCell122);

                        // 12^ riga: 3^ colonna
                        TableCell tCell123 = new TableCell();
                        tCell123.Text = off;
                        tRow12.Cells.Add(tCell123); // 0

                        // Valutazione offerta tecnica
                        // 13a^ riga    
                        TableRow tRow13d = new TableRow();
                        tdatidettaglio.Rows.Add(tRow13d);  // aggiungo riga
                        // 13^ riga: 1^ colonna                
                        TableCell tCell131d = new TableCell();
                        tCell131d.Text = new string(' ', 2) + "-" + new string(' ', 2) + "nomina commissione tecnica".ToString();
                        tCell131d.ForeColor = System.Drawing.Color.Black;
                        tRow13d.Cells.Add(tCell131d); // 0

                        // 13^ riga: 2^ colonna
                        TableCell tCell132d = new TableCell();
                        tCell132d.Text = "".ToString();
                        tRow13d.Cells.Add(tCell132d);

                        // 13^ riga: 3^ colonna
                        TableCell tCell133d = new TableCell();
                        s = ds.Tables["gare"].Rows[r]["datanominacommTec"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables["gare"].Rows[r]["datanominacommTec"]).ToString(formatodata);
                        tCell133d.Text = s;
                        //tdatidettaglio.Rows[12].Cells[2].BackColor = color;
                        tRow13d.Cells.Add(tCell133d);

                        // solo testo con scritta valutazione offerta tecnica
                        // 13e^ riga    
                        TableRow tRow13e = new TableRow();
                        tdatidettaglio.Rows.Add(tRow13e);  // aggiungo riga
                        // 13^ riga: 1^ colonna                
                        TableCell tCell131e = new TableCell();
                        tCell131e.Text = new string(' ', 2) + "-" + new string(' ', 2) + "conclusione lavori commissione".ToString();
                        tCell131e.ForeColor = System.Drawing.Color.Black;
                        tRow13e.Cells.Add(tCell131e); // 0

                        // 13^ riga: 2^ colonna
                        TableCell tCell132e = new TableCell();
                        tCell132e.Text = "".ToString();
                        tRow13e.Cells.Add(tCell132e);

                        // 13^ riga: 3^ colonna dataricezioneverbalicommissionetecnica
                        TableCell tCell133e = new TableCell();
                        s = ds.Tables["gare"].Rows[r]["dataricezioneverbalicommissionetecnica"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables["gare"].Rows[r]["dataricezioneverbalicommissionetecnica"]).ToString(formatodata);
                        tCell133e.Text = s;
                        tCell133e.ForeColor = System.Drawing.Color.Black;
                        tRow13e.Cells.Add(tCell133e);


                        // Seduta offerte economiche
                        // 13^ riga    
                        TableRow tRow13 = new TableRow();
                        tdatidettaglio.Rows.Add(tRow13);  // aggiungo riga
                        // 13^ riga: 1^ colonna                
                        TableCell tCell131 = new TableCell();
                        tCell131.Text = "seduta apertura offerte economiche".ToString();
                        tRow13.Cells.Add(tCell131); // 0

                        // 13^ riga: 2^ colonna
                        TableCell tCell132 = new TableCell();
                        tCell132.Text = "".ToString();
                        tRow13.Cells.Add(tCell132);

                        // 13^ riga: 3^ colonna
                        TableCell tCell133 = new TableCell();
                        tCell133.Text = eco;
                        tRow13.Cells.Add(tCell133); // 0

                        // valutazione offerta anomala
                        // 13a^ riga    
                        TableRow tRow13c = new TableRow();
                        tdatidettaglio.Rows.Add(tRow13c);  // aggiungo riga
                                                           // 13^ riga: 1^ colonna                
                        TableCell tCell131c = new TableCell();
                        tCell131c.Text = new string(' ', 2) + "-" + new string(' ', 2) + "invio offerte per anomalia".ToString();
                        tCell131c.ForeColor = System.Drawing.Color.Black;
                        tRow13c.Cells.Add(tCell131c); // 0

                        // 13^ riga: 2^ colonna
                        TableCell tCell132c = new TableCell();
                        tCell132c.Text = "".ToString();
                        tRow13c.Cells.Add(tCell132c);

                        // 13^ riga: 3^ colonna
                        TableCell tCell133c = new TableCell();
                        s = ds.Tables["gare"].Rows[r]["datainvioofferteperanomalia"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables["gare"].Rows[r]["datainvioofferteperanomalia"]).ToString(formatodata);
                        tCell133c.Text = s;
                        tCell133c.ForeColor = System.Drawing.Color.Black;
                        tRow13c.Cells.Add(tCell133c);

                        // valutazione offerta anomala
                        // 13a^ riga    
                        TableRow tRow13f = new TableRow();
                        tdatidettaglio.Rows.Add(tRow13f);  // aggiungo riga
                                                           // 13^ riga: 1^ colonna                
                        TableCell tCell131f = new TableCell();
                        tCell131f.Text = new string(' ', 2) + "-" + new string(' ', 2) + "ricezione esito anomalia".ToString();
                        tCell131f.ForeColor = System.Drawing.Color.Black;
                        tRow13f.Cells.Add(tCell131f); // 0

                        // 13^ riga: 2^ colonna
                        TableCell tCell132f = new TableCell();
                        tCell132f.Text = "".ToString();
                        tRow13f.Cells.Add(tCell132f);

                        // 13^ riga: 3^ colonna
                        TableCell tCell133f = new TableCell();
                        s = ds.Tables["gare"].Rows[r]["dataricezioneesitoanomalia"] == DBNull.Value ? "" : Convert.ToDateTime(ds.Tables["gare"].Rows[r]["dataricezioneesitoanomalia"]).ToString(formatodata);
                        tCell133f.Text = s;
                        tCell133f.ForeColor = System.Drawing.Color.Black;
                        tRow13f.Cells.Add(tCell133f);


                        // Seduta di aggiudicazione
                        // 14^ riga    
                        TableRow tRow14 = new TableRow();
                        tdatidettaglio.Rows.Add(tRow14);  // aggiungo riga
                        // 14^ riga: 1^ colonna                
                        TableCell tCell141 = new TableCell();
                        tCell141.Text = "data seduta di aggiudicazione".ToString();
                        tRow14.Cells.Add(tCell141); // 0

                        // 14^ riga: 2^ colonna
                        TableCell tCell142 = new TableCell();
                        tCell142.Text = "".ToString();
                        tRow14.Cells.Add(tCell142);

                        // 14^ riga: 3^ colonna
                        TableCell tCell143 = new TableCell();                        
                        s = dsa.ToShortDateString().ToString() != "01/01/1900" ? dsa.ToString(formatodata) : "";
                        tCell143.Text = s;
                        tRow14.Cells.Add(tCell143); // 0

                        // ditta aggiudicataria
                        // 16^ riga    
                        TableRow tRow16 = new TableRow();
                        tdatidettaglio.Rows.Add(tRow16);  // aggiungo riga
                        // 16^ riga: 1^ colonna                
                        TableCell tCell161 = new TableCell();
                        tCell161.Text = "aggiudicataria".ToString();
                        tRow16.Cells.Add(tCell161); // 0

                        // 16^ riga: 2^ colonna                        
                        TableCell tCell162 = new TableCell();
                        tCell162.Text = "".ToString();
                        tRow16.Cells.Add(tCell162);

                        // 16^ riga: 3^ colonna
                        TableCell tCell163 = new TableCell();
                        tCell163.Text = dsa.ToShortDateString().ToString() != "01/01/1900" ? ds.Tables["gare"].Rows[r]["denominazione"].ToString() : "";
                        tcStyle.Font.Bold = true;
                        tcStyle.BorderColor = Color.Transparent;
                        tCell163.ApplyStyle(tcStyle);
                        tRow16.Cells.Add(tCell163); // 0 */

                                                    // Contenzioso in corso
                                                    //if ((bool)ds.Tables["gare"].Rows[r]["contenzioso"] == true)
                                                    //{
                                                    // 15^ riga    
                        TableRow tRow15 = new TableRow();
                        tdatidettaglio.Rows.Add(tRow15);  // aggiungo riga
                        // 15^ riga: 1^ colonna                
                        TableCell tCell151 = new TableCell();
                        tCell151.Text = "contenzioso in corso".ToString();
                        tRow15.Cells.Add(tCell151); // 0
    
                        // 15^ riga: 2^ colonna
                        TableCell tCell152 = new TableCell();
                        tCell152.Text = "".ToString();
                        tRow15.Cells.Add(tCell152);

                        // 11^ riga: 3^ colonna
                        s = "NO";
                        TableCell tCell153 = new TableCell();
                        if (ds.Tables["gare"].Rows[r]["contenzioso"] != DBNull.Value )
                            s = ds.Tables["gare"].Rows[r]["contenzioso"].ToString() == "1" ? "SI" : "";
                        tCell153.Text = s;
                        tRow15.Cells.Add(tCell153);
                        //}

                        // 15^ riga    
                        TableRow tRow15a = new TableRow();
                        tdatidettaglio.Rows.Add(tRow15a);  // aggiungo riga
                                                          // 15^ riga: 1^ colonna                
                        TableCell tCell151a = new TableCell();
                        tCell151a.Text = "data stipula contratto".ToString();
                        tRow15a.Cells.Add(tCell151a); // 0

                        // 15^ riga: 2^ colonna
                        TableCell tCell152a = new TableCell();
                        tCell152a.Text = "".ToString();
                        tRow15a.Cells.Add(tCell152a);

                        // 11^ riga: 3^ colonna
                        TableCell tCell153a = new TableCell();
                        s = "";
                        if (ds.Tables["gare"].Rows[r]["data_contratto"] != DBNull.Value)
                            s = Convert.ToDateTime(ds.Tables["gare"].Rows[r]["data_contratto"]).ToString(formatodata); 
                        tCell153a.Text = s;
                        tRow15a.Cells.Add(tCell153a);
                
                        // Spazio TUTOR
                        // 17^ riga    
                        TableRow tRow17 = new TableRow();
                        tdatidettaglio.Rows.Add(tRow17);  // aggiungo riga
                        // 17^ riga: 1^ colonna                
                        TableCell tCell171 = new TableCell();
                        tCell171.Text = "note".ToString();
                        tcStyle.Font.Bold = true;
                        tcStyle.BorderColor = Color.Transparent;
                        tCell171.ApplyStyle(tcStyle);
                        tRow17.Cells.Add(tCell171); // 0

                        // 17^ riga: 2^ colonna                        
                        TableCell tCell172 = new TableCell();
                        tCell172.Text = "".ToString();
                        tRow17.Cells.Add(tCell172);

                        // 17^ riga: 3^ colonna
                        TableCell tCell173 = new TableCell();
                        s = ds.Tables["gare"].Rows[r]["Notetutor"] == DBNull.Value ? "" : ds.Tables["gare"].Rows[r]["Notetutor"].ToString();
                        tCell173.Text = s;
                        tRow17.Cells.Add(tCell173); // 0 */


                        // tempistica
                        System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml("#FFDFD991");
                        if (mettolamail)
                            tStato.Text = "Per ulteriori informazioni relative alla presente procedura è possibile inviare richiesta all'indirizzo e-mail monitoraggio.gare@provincia.tn.it";
                    }
                }

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
        }
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
            da = new SqlDataAdapter(select, conn);
            //da.SelectCommand.CommandText = select;
            //if (ds.Tables[strTableName].Rows.Count > 0 ) ds.Tables[strTableName].Clear(); // evito il doppi caricamento dei dati
            // dset.Clear();
            da.Fill(dset, strTableName);
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