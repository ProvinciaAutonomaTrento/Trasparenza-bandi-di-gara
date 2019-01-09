<%--/**
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
 */--%>

<%@ Page Title="Richieste servizi APAC" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="menu.aspx.cs" Inherits="menu" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <style>
    .sottoadx {
        position: absolute;
        right: 0px;
        top: 1px;
        max-width: 100%;               
        background-repeat: repeat-x;
        z-index: -2;
    }
    .dentroadx {
        position: absolute;
        top: 11px;
        right: 10px;
        width: 171px;
        height: 78px;
        border: none;
    }
    .adx {
        position: absolute;
        top: 0px;
        right: 0px;
        width: 171px;
        height: 78px;
    }
    .centra-text {
        text-align: center;
    }
    .riempidietro {
        max-width: 100%;
        height: auto;
        z-index: -1;
    }
    .auto-testata {
        max-width: 100%;
        height: 121px;
        border: none;
        z-index: -1;
    }
    .centra {
        margin: auto;
    }
    .hrsalmone {
        Width: 1200px; margin: auto; background-color: lightsalmon; height: 6px;
    }
    .hrbianca {
        Width: 1200px; margin: auto; border: 2px; background-color: white; height: 2px;
        clear: both;
    }
    .content {
        width: 1200px;
        margin: 0px auto;
        text-align: center;
    }
    .container {
        position: relative;
        width: 1200px;
        margin: 0px auto;
        text-align: center;
        padding: 5px;
        box-sizing: border-box;
    }
    .containerdx {
        position: absolute;
        width: 512px;
        left: 512px;
        height: 25px;
        float: left;
    }
    .titolo { text-align: center; font-size:larger; }
    .num { text-align: right; }

    .asx {
        float: left;
        width : 508px;
        box-sizing: border-box;
        background-color: lightblue; color: black; text-align: center;        
        height: 22px;
        margin: 2px;
        border-radius: 4px;
    }
    .adx {
        float: right;        
        width : 508px;
        box-sizing: border-box;
        background-color: lightblue; color: black; text-align: center;
        padding: 1px;
        height: 22px;
        margin: 2px;
        border-radius: 4px;
    }
    .clearfix {
        content: "";
        clear: both;
        display: table;
    }
    .boxverde {
        background-color: lightblue;
        width: 1200px;
        border-radius: 4px;
    }
    .tb {
        border-width: 1px; border-radius: 4px; Border-Color: blue;  box-sizing: border-box; text-align: left; padding: 1px; margin: 2px; float:left;
    }
    .auto-style1 {
        text-align: center;
        font-size: 22px;
    }
    .col1 { width: 10px;  }
    .coltext { width:400px; text-align: left; font-size: large; }
    .colin { width: 722px; text-align: left; font-size: large; }
    .colinp { width: 502px; text-align: left; font-size: large; }
    .colpiu { width: 350px; font-size: large; }
    .colast { width: 50px; }
    .coler { width: auto; text-align: left; }
    .menu { width: 1200px; height: 33px; margin: 0px auto; background-color: lightsalmon; border-radius: 6px; box-sizing: border-box; padding:3px; padding-left: 7px; padding-right: 7px;z-index: -22; height: 28px;}
    </style>
<br />
<%-- <div class="hrbianca"></div>  --%>
<div class="titolo" style="height: 22px;">RICHIESTE SERVIZI APAC</div><br /><div class="hrbianca"></div>
<div class="menu">
    <div style="width: 420px; float: left; text-align: left; ">
        <asp:Label ID="LBenvenuto" runat="server" Text=" "></asp:Label>
    </div>
    <div style="width: 130px; float: right;">
    <asp:Button ID="bUscita" runat="server" Text="Uscita" Width="120px" OnClick="bUscita_Click" />
    </div>
    <div style="width: 130px; float: right;">
    <asp:Button ID="bAdd" runat="server" Text="Nuova Richiesta" Width="130px" OnClick="bAdd_Click" />
    </div>
    <%--<div style="width: 150px; float: right;">
        <asp:Button ID="bConvalida" runat="server" Text="Conferma Richiesta" Width="140px"  visible="false" OnClick="bConvalida_Click" />
    </div> --%>
    <div style="width: 130px; float: right;">
        <asp:Button ID="bDel" runat="server" Text="Elimina Richiesta" Width="120px" OnClick="bDel_Click" visible="false" />
    </div>
    <div style="width: 90px; float: right;">
        <asp:Button ID="cbMyAccount" runat="server" OnClick="cbMyAccount_Click" Text="I miei dati" Visible="true" Width="85px" />
    </div>
    <div style="width: 75px; float: right;">        
        <asp:Button ID="bHelp" runat="server" Text="Istruzioni" Width="70" onclientclick="window.open('istruzioni.pdf', 'newPage');" OnClick="bHelp_Click"/>
    </div>
    <div style="width: 210px; float: right;">
        <asp:Label ID="lSort" runat="server" text="Ordina per" Width="80" Height="23px"></asp:Label>
        <asp:DropDownList ID="ddlSortExpression" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSortExpression_SelectedIndexChanged" Height="23px" Width="120px" ></asp:DropDownList>
    </div>
</div>
<hr class="hrbianca" />    
<div class="hrbianca"></div>
<div class="container" style="width: 1200px; background-color:cornsilk; border-width: 6px; border-radius: 6px; height:auto; margin: 0px auto; box-sizing: border-box;">
    <asp:GridView ID="dgvComm" runat="server" Width="1200" AutoGenerateColumns="false" Visible="true"
            DataKeyNames="id" PageSize="999" ShowHeaderWhenEmpty="True" HorizontalAlign="Center"
            style="text-align: left; float: left; " CellPadding="2" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="dgvComm_SelectedIndexChanged" Font-Size="Small" OnRowDataBound="dgvComm_RowDataBound" >
       <Columns>
            <asp:CommandField ButtonType="Button" SelectText="Modifica" headertext="Comando" ShowSelectButton="True"><ItemStyle Width="80px" /></asp:CommandField>
            <asp:Boundfield DataField="convalidaasdata" headertext="Data conferma" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="100" HorizontalAlign="left" Wrap="true"/></asp:Boundfield>
            <asp:Boundfield DataField="titolo" headertext="Oggetto"  HeaderStyle-HorizontalAlign="left"><ItemStyle Width="804"  HorizontalAlign="left" Wrap="true"/></asp:Boundfield>
            <asp:Boundfield DataField="Importoasta" headertext="Base d'asta" HeaderStyle-HorizontalAlign="right" DataFormatString="{0:C2}"><ItemStyle Width="130px" HorizontalAlign="right"/></asp:Boundfield>
            <asp:Boundfield DataField="Stato" headertext="Stato ric." HeaderStyle-HorizontalAlign="center" ><ItemStyle Width="90px" HorizontalAlign="center"/></asp:Boundfield>
       </Columns>
    </asp:GridView>
   
    <asp:Panel ID="pGara" runat="server">
        <table border="0" style="border: medium solid #1F5DB2; border-radius: 6px; padding: 10px; box-sizing:padding-box; float: left;">
            <tr>
                <td class="col1"></td>
                <td class="coltext"><strong>Ente/servizio richiedente</strong></td>
                <td class="col1"></td>
                <td class="colimp" colspan="2"><strong><asp:Label ID="lEnte" runat="server" ></asp:Label></strong></td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext"></td>
                <td class="col1"></td>
                <td class="colinp"></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Servizio richiesto</td>
                <td class="col1"></td>
                <td class="colinp"><asp:DropDownList ID="ddlServizio" runat="server" Width="400" Name="Ser" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large"></asp:DropDownList></td>
                <td rowspan=3 class="colinp">
                    <asp:Panel ID="pconferma" runat="server" Visible="true" HorizontalAlign="center" >
                        <div class="colinpiu" visible="false" style="text-align:center; padding: 8px; background-color: lightsalmon; color: white; font: xx-large; border-radius: 13px; border: 2px solid; border-color: blue;">
                            <asp:Button ID="btConvalida" runat="server" Text="Conferma richiesta!" OnClick="btConvalida_Click" Visible="false" Font-Size="Large" />
                        </div>
                    </asp:Panel>                    
                </td>               
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Tipologia</td>
                <td class="col1"></td>
                <td class="colinp"><asp:DropDownList ID="ddlTipologia" runat="server" Name="Tip" Width="400"  onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" OnTextChanged="ddlTipologia_TextChanged" AutoPostBack="True" Font-Size="Large" ></asp:DropDownList></td>
                <td class="col1"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Sistema di affidamento</td>
                <td class="col1"></td>
                <td class="colinp"><asp:DropDownList ID="ddlSistemaAffidamento" runat="server" Name="Aff" Width="400"  onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" AutoPostBack="True" Font-Size="Large" ></asp:DropDownList>
                </td>
                <td class="col1"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Procedura di affidamento</td>
                <td class="col1"></td>
                <td class="colinp"><asp:DropDownList ID="ddlAffidamento" runat="server" Name="Aff" Width="400"  onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large"></asp:DropDownList></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Criterio di aggiudicazione</td>
                <td class="col1"></td>
                <td class="colinp"><asp:DropDownList ID="ddlCriterio" runat="server" Name="Cri" Width="400"  onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large" ></asp:DropDownList></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Settore/ambito</td>
                <td class="col1"></td>
                <td class="colinp"><asp:DropDownList ID="ddlSettore" runat="server" Name="Settore" Width="400"  onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large" ></asp:DropDownList></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Titolo</td>
                <td class="col1"></td>
                <td colspan=2 class="colin"><asp:TextBox ID="tOgg" runat="server" Width="831" maxlenght="255" Name="Ogg" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large" ></asp:TextBox></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="acol1"></td>
                <td class="coltext">Importo base d'asta</td>
                <td class="col1"></td>
                <td class="colinp"><asp:TextBox ID="tImporto" Name="Imp" runat="server" Autopostback="true" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" OnTextChanged="tImporto_TextChanged" maxlenght="15" datatextformatstring="{0:D2}" Font-Size="Large"></asp:TextBox></td>
                <td class="colinpiu"></td>
                <td class="col1"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Sopra soglia</td>
                <td class="col1"></td>
                <td class="colinp"><asp:DropDownList ID="ddlSoglia" runat="server" Name="Soglia" Width="70" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large"></asp:DropDownList></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Gara suddivisa in lotti</td>
                <td class="col1"></td>
                <td class="colinp"><asp:DropDownList ID="ddlLotti" runat="server" Name="Lot" Width="70" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large"></asp:DropDownList></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>

            <%--<tr>
                <td class="col1"></td>
                <td class="coltext">Settore</td>
                <td class="col1"></td>
                <td class="colin"><asp:DropDownList ID="ddlSettore" runat="server" Name="Set" Width="200"  onmouseover="omiServizio(this);" onmouseout="omoServizio(this);"></asp:DropDownList></td>
                <td class="coler"></td>
                <td class="col1"></td>
            </tr>--%>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Data invio documentazione</td>
                <td class="col1"></td>
                <td class="colinp"><asp:TextBox ID="tDocumentazione" runat="server" autopostback="true" Name="Dat" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" OnTextChanged="tDocumentazione_TextChanged" Font-Size="Large" Style="float: left;" ></asp:TextBox>
                    <div style="width: 40px; float: left;"><pre>   </pre></div>
                    <asp:Label ID="lQuadrimestre" runat="server" ></asp:Label>
                </td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <%--<tr>
                <td class="col1"></td>
                <td class="coltext">Data presunta pubblicazione bando/spedizione inviti</td>
                <td class="col1"></td>
                <td class="colinp">
                    <asp:TextBox ID="tBando" runat="server" Name="Dat" autopostback="true" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" OnTextChanged="tBando_TextChanged" Font-Size="Large" Style="float: left;"></asp:TextBox>
                </td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr> --%>
            <tr>
                <td class="col1"></td>
                <td class="coltext"><asp:Label ID="lFinanziamentoPAT" Text="Finanziamento PAT" runat="server" Visible="true"/></td>
                <td class="col1"></td>
                <td class="colinp"><asp:DropDownList ID="ddlFinanziamento" runat="server" Name="Fin" Width="70"  onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" visible="true" Font-Size="Large" AutoPostBack="true" OnSelectedIndexChanged="ddlFinanziamento_SelectedIndexChanged" ></asp:DropDownList></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext"><asp:Label ID="lMop" Text="MOP (numero pratica finanziamento)" runat="server" Visible="true"/></td>
                <td class="col1"></td>
                <td class="colinp"><asp:TextBox ID="tMop" runat="server" Name="Mop" maxlenght="15" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" visible="true" Font-Size="Large"></asp:TextBox></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext"><asp:Label ID="lNote" Text="Note" runat="server" Visible="true"/></td>
                <td class="col1"></td>
                <td colspan=2 class="colin"><asp:TextBox ID="tNote" runat="server" Name="Note" height="50px" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" MaxLength="255" width="852" TextMode="MultiLine"></asp:TextBox></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext"><asp:Label ID="lRef" Text="Referente" runat="server" Visible="true"/></td>
                <td class="col1"></td>
                <td class="colinp"><asp:TextBox ID="tRef" runat="server" Name="Ref"  Width="400" maxlenght="55" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large"></asp:TextBox></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext"><asp:Label ID="lRef_Tel" Text="Telefono referente" runat="server" Visible="true"/></td>
                <td class="col1"></td>
                <td class="colinp"><asp:TextBox ID="tRef_Tel" runat="server" Name="Ref_Tel"  Width="124" maxlenght="33" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large"></asp:TextBox></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext"><asp:Label ID="lRef_Mail" Text="Mail referente" runat="server" Visible="true"/></td>
                <td class="col1"></td>
                <td class="colinp"><asp:TextBox ID="tRef_Mail" runat="server" Name="Ref_Mail" maxlenght="100" Width="400" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large"></asp:TextBox></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext"><asp:Label ID="lRup" Text="Responsabile unico proc." runat="server" Visible="true"/></td>
                <td class="col1"></td>
                <td class="colinp"><asp:TextBox ID="tRup" runat="server" Name="Rup"  Width="400" maxlenght="99" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large"></asp:TextBox></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext"><asp:Label ID="lUscente" Text="Precedente ditta appaltatrice" runat="server" Visible="false"/></td>
                <td class="col1"></td>
                <td class="colinp"><asp:TextBox ID="tUscente" runat="server" Name="Ref"  Width="400" maxlenght="55" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" Font-Size="Large" Visible="false"></asp:TextBox></td>
                <td class="colinpiu"></td>
                <td class="collast"></td>
                <td class="col1"></td>
            </tr>
        </table>
    </asp:Panel>
    <div style="clear: both; height: 6px;"></div>
    <div>
    <div style="margin: 0px auto; text-align: center; float: left;"><br />
        <div style="width: 250px; float: left;">
            <asp:Button ID="btdwnloadfacsimile" runat="server" Text="Download fac-simile richiesta" Width="200px" height="22" visible="false" OnClick="btdwnloadfacsimile_Click" />
        </div>
        <div style="width: 200px; float: left; ">
            <asp:Button ID="bdwnloadrichiesta" runat="server" Text="Download scheda" Width="160px" height="22" visible="false" OnClick="bdwnloadrichiesta_Click" />
        </div>
        <div style="clear: both;"></div>
        <asp:HyperLink runat="server" ID="hlRichiesta" text="Visualizza richiesta" NavigateUrl="test" visible="false" Target="_blank"></asp:HyperLink>
        <asp:HyperLink runat="server" ID="hlModulo" text="Visualizza fac simile lettera" NavigateUrl="test" visible="false" Target="_blank" style="float: left;"></asp:HyperLink>
        <asp:Button ID="bConferma" runat="server" Text="Confermi la cancellazione della richiesta ?" Width="280px" OnClick="bConferma_Click" visible="false" style="float: left;"/>
        <pre style="float: left;">      </pre>
        <asp:Button ID="bAnnulla" runat="server" Text="ANNULLA" Width="90px" OnClick="bAnnulla_Click" visible="false" />
    </div>
    </div>
</div>
<div class="centra-text clearfix" style="clear:both; ">
<asp:Label ID="sStato" runat="server" multiline="true" border-color="blue" style="border-width: 2px; border-radius: 4px; Border-Color: blue; width: 1200px; box-sizing: border-box;"></asp:Label>
</div>
<div id="idHelp" style="margin: 0px auto; border-color: darkblue; border:2px; border-width: 2px; width: 1200px; height: 86px; padding: 4px; box-sizing:padding-box; border-radius: 4px;"></div>     

    <script type="text/javascript">
        function omiServizio(x) {
            var s = document.getElementById("idHelp");
            s.style.backgroundColor = "white";
            switch (x.id)
            {
                //case "ctl00_MainContent_ddlServizio": s.innerHTML = "Consulenza: si chiede la consulenza, la gara sarà bandita dal richiedente; Stazione appaltante: la gara sarà bandita da APAC per conto dell'ente rihiedente."; break;
                case "ctl00_MainContent_tOgg": s.innerHTML = "definire in maniera sintetica ed esaustiva l’oggetto dell’affidamento e utilizzare la medesima dicitura su tutti i documenti inerenti la procedura di gara"; s.style.backgroundColor = "lightblue"; break;
                case "ctl00_MainContent_tImporto": s.innerHTML = "Importo al netto di Iva e comprensivo degli oneri di sicurezza."; s.style.backgroundColor = "lightblue"; break;
                case "ctl00_MainContent_ddlLotti": s.innerHTML = "Il dettaglio dei lotti sarà definito al momento dell’avvio dell’istruttoria."; s.style.backgroundColor = "lightblue"; break;
                case "ctl00_MainContent_ddlSoglia": s.innerHTML = "<table style='border: 1px solid black; border-collapse: collapse;'><tr><td colspan=11 class='titolo' >Sopra soglia europea</td></tr><tr style='border:1px solid gray; border-collapse: collapse;'><td style='titolo2'>Tipologia</td><td></td><td style='titolo2'>Soglia di importo</td><td style='bordo'></td><td cstyle='titolo2'>Tipologia</td><td></td><td style='titolo2'>Soglia di importo</td><td style='bordo'></td><td style='titolo2'>Tipologia</td><td></td><td style='titolo2'>Soglia di importo</td></tr><tr><td>appalti lavori</td><td>≥</td><td class='num'>5 548 000 €</td><td style='bordo'></td><td>concessioni lavori/servizi</td><td>≥</td><td class='num'>5 548 000 €</td><td style='bordo'></td><td>settori speciali lavori</td><td>≥</td><td class='num'>5 548 000 €</td></tr><tr><td>appalti servizi/forniture</td><td>≥</td><td class='num'>221 000 €</td><td style='bordo'></td><td>servizi sociali e altri servizi specifici</td><td>≥</td><td class='num'>750 000 €</td><td style='bordo'></td><td>settori speciali servizi/forniture</td><td>≥</td><td class='num'>443 000 €</td></tr></table>"; s.style.backgroundColor = "lightblue"; break;
                //case "ctl00_MainContent_ddlTipologia": s.innerHTML = ""; break;
                //case "ctl00_MainContent_ddlAffidamento": s.innerHTML = "Il sistema propone in automatico il criterio di aggiudicazione più probabile sulla base delle informazioni precedentemente inserite. E’ necessario tuttavia verificare l’appropriatezza della scelta proposta dal sistema e intervenire qualora la scelta proposta non fosse corretta."; s.style.backgroundColor = "lightblue"; break;
                //case "ctl00_MainContent_ddlCriterio": s.innerHTML = "Il sistema propone in automatico il criterio di aggiudicazione più probabile sulla base delle informazioni precedentemente inserite. E’ necessario tuttavia verificare l’appropriatezza della scelta proposta dal sistema e intervenire qualora la scelta proposta non fosse corretta."; s.style.backgroundColor = "lightblue"; break;
                //case "ctl00_MainContent_ddlSettore": s.innerHTML = "Tipologia: si chiede la consulenza, la gara sarà bandita dal richiedente; Stazione appaltante: la gara sarà bandita da APAC per conto dell'ente rihiedente."; break;
                case "ctl00_MainContent_tDocumentazione" : s.innerHTML = "Indicare la data stimata entro cui è intenzione dell’ente/struttura PAT inoltrare ad APAC la documentazione necessaria per la pubblicazione della gara che dovrà essere conforme alle indicazioni reperibili sul sito di APAC al link http://www.appalti.provincia.tn.it/modulistica."; s.style.backgroundColor = "lightblue"; break;
                case "ctl00_MainContent_tBando" : s.innerHTML = "Indicare la data entro cui l’ente/struttura PAT ha necessità di bandire la procedura di gara tenuto conto che tra l’invio della  documentazione e la pubblicazione del bando decorrono mediamente 60 gg, in relazione alla complessità della procedura. "; s.style.backgroundColor = "lightblue"; break;
                //case "ctl00_MainContent_ddlFinanziamento": s.innerHTML = "Tipologia: si chiede la consulenza, la gara sarà bandita dal richiedente; Stazione appaltante: la gara sarà bandita da APAC per conto dell'ente rihiedente."; break;
                case "ctl00_MainContent_tNote" : s.innerHTML = "Indicare nelle note esigenze specifiche dalle quali derivino obblighi di pubblicazione entro una certa data."; s.style.backgroundColor = "lightblue"; break;
                case "ctl00_MainContent_tMop" : s.innerHTML = "Numero identificativo della assegnazione del finanziamento. Viene rilasciato dal Servizio Enti Locali."; s.style.backgroundColor = "lightblue"; break;
                case "ctl00_MainContent_tRup" : s.innerHTML = "Indicare il nome del Responsabile unico del procedimento."; s.style.backgroundColor = "lightblue"; break;
                case "ctl00_MainContent_tUscente" : s.innerHTML = "Indicare il nome dell'Azienda/Ditta/Rti/Ati che precedentemente aveva l'appalto."; s.style.backgroundColor = "lightblue"; break;
                //case "ctl00_MainContent_tRef": s.innerHTML = ""; break;
                //case "ctl00_MainContent_tRef_Tel": s.innerHTML = "Recapito telefonico del responsabile del procedimento."; break;
                //case "ctl00_MainContent_tRef_Mail": s.innerHTML = "Email  del responsabile del procedimento."; break;
            }           
        }
        function omoServizio(x) {
            var s = document.getElementById("idHelp");
            s.innerHTML = ""; s.style.backgroundColor = "white";
        }
</script>
</asp:Content>


