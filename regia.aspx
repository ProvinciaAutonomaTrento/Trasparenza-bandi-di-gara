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

<%@ Page Title="Richieste servizi APAC" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="regia.aspx.cs" Inherits="regia" %>


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
        Width: 1200px; margin: auto; border: 2px; background-color: white; height: 8px;
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
    .coltext { width:220px; text-align: left; }
    .colin { width: 240px; text-align: left; }
    .coler { width: auto; text-align: left; }
    .menu { width: 1200px; height: 33px; margin: 0px auto; background-color: lightsalmon; border-radius: 6px; box-sizing:border-box; padding:3px; padding-left: 7px; padding-right: 7px;z-index: -22; height: 28px;}
 
        .auto-style2 {
            width: 10px;
            height: 23px;
        }
        .auto-style3 {
            width: 220px;
            text-align: left;
            height: 23px;
        }
        .auto-style4 {
            width: 240px;
            text-align: left;
            height: 23px;
        }
 
    </style>
<br />
<div class="hrbianca"></div>
<div class="auto-style1" style="height: 22px;">AMMINISTRAZIONE</div><br /><div class="hrbianca"></div>
<div class="menu">
    <div style="width: 593px; float: left; text-align: left; ">
        <asp:Label ID="LBenvenuto" runat="server" Text=" "></asp:Label>
    </div>
    <div style="width: 100px; float: right;">
    <asp:Button ID="bUscita" runat="server" Text="Uscita" Width="90px" OnClick="bUscita_Click" />
    </div>
    <div style="width: 130px; float: right;">
        <asp:Button ID="bSalva" runat="server" Text="Salva" Width="120px" OnClick="bSalva_Click" visible="false" />
    </div>
    <div style="width: 130px; float: right;">
        <asp:Button ID="bDel" runat="server" Text="Elimina" Width="120px" OnClick="bDel_Click" visible="false" />
    </div>
    <div style="width: 100px; float: right;">
        <asp:Button ID="bDisabilitati" runat="server" Text="NON Abilitati" Width="90px" visible="true" OnClick="bDisabilitati_Click" />
    </div>
    <div style="width: 90px; float: left;">
        <asp:Button ID="cbMyAccount" runat="server" OnClick="cbMyAccount_Click" Text="I miei dati" Visible="False" Width="85px" />
    </div>
</div>
<hr class="hrbianca" />    
<div class="hrbianca"></div>
<div class="container" style="background-color:cornsilk; border-width: 6px; border-radius: 4px; height:auto; margin: 0px auto; box-sizing: border-box;">
    <asp:GridView ID="dgvComm" runat="server" Width="1200" AutoGenerateColumns="false" Visible="true"  AllowSorting="True" 
            DataKeyNames="id" PageSize="999" ShowHeaderWhenEmpty="false" HorizontalAlign="Center"
            style="text-align: left; float: left; " CellPadding="2" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="dgvComm_SelectedIndexChanged" Font-Size="Small" >
       <Columns>
            <asp:CommandField ButtonType="Button" SelectText="Modifica" headertext="Comando" ShowSelectButton="True"><ItemStyle Width="80px" /></asp:CommandField>
            <asp:Boundfield DataField="ente" headertext="Ente/struttura" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="300px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield>
            <asp:Boundfield DataField="NIKNAME" headertext="User" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="80px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield>  
            <asp:Boundfield DataField="nome" headertext="Nome" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="80px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield>
            <asp:Boundfield DataField="cognome" headertext="Cognome" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="110px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="telefono" headertext="tel" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="80px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="mail" headertext="Mail" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="100px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="abilitatook" headertext="OK!" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="110px" HorizontalAlign="center" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="ric" headertext="n. ric." HeaderStyle-HorizontalAlign="center"><ItemStyle Width="70px" HorizontalAlign="center" Wrap="true"/></asp:Boundfield>  
            <asp:Boundfield DataField="power" headertext="Potere" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="70px" HorizontalAlign="center" Wrap="true"/></asp:Boundfield> 
       </Columns>
       <SortedAscendingCellStyle BackColor="#E9E7E2" />
       <SortedAscendingHeaderStyle BorderStyle="Solid" BackColor="#506C8C" />
       <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
       <SortedDescendingCellStyle BackColor="#FFFDF8" />
       <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
   
    <asp:Panel ID="pGara" runat="server">
        <table border="0" style="border: medium solid #1F5DB2; border-radius: 3px; padding: 10px; float: left;">
            <tr>
                <td class="col1"></td>
                <td class="coltext"><strong>Ente/servizio richiedente</strong></td>
                <td class="col1"></td>
                <td colspan="6"><div style="width: 800px; text-align: left; float: left;"><strong><asp:Label ID="lEnte" runat="server" ></asp:Label></strong></div>
                    <div style="width:100px; float: left;">
                    </div></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="auto-style2"></td>
                <td class="auto-style3"></td>
                <td class="auto-style2"></td>
                <td class="auto-style4"></td>
                <td class="auto-style2"></td>
                <td class="auto-style3"></td>
                <td class="auto-style2"></td>
                <td class="auto-style4"></td>
                <td class="auto-style2"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Nome</td>
                <td class="col1"></td>
                <td class="colin">
                    <asp:Label ID="lNome" runat="server" width="150"></asp:Label>
                </td>
                <td class="col1"></td>
                <td class="coltext">Cognome</td>
                <td class="col1"></td>
                <td class="colin"><asp:Label ID="lCognome" runat="server" width="150"></asp:Label></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Telefono</td>
                <td class="col1"></td>
                <td class="colin"><asp:Label ID="lTel" runat="server" width="150"></asp:Label></td>
                <td class="coli1"></td>
                <td class="coltext">Mail</td>
                <td class="col1"></td>
                <td class="colin"><asp:Label ID="lMail" runat="server" width="250"></asp:Label></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Città</td>
                <td class="col1"></td>
                <td class="colin"><asp:Label ID="lCitta" runat="server" width="250"></asp:Label></td>
                <td class="col1"></td>
                <td class="coltext">Cap</td>
                <td class="col1"></td>
                <td class="colin"><asp:Label ID="lCap" runat="server" width="250"></asp:Label></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Indirizzo</td>
                <td class="col1"></td>
                <td class="colin"><asp:Label ID="lIndirizzo" runat="server" width="250"></asp:Label></td>
                <td class="col1"></td>
                <td class="coltext">Civico</td>
                <td class="col1"></td>
                <td class="colin"><asp:Label ID="lCivico" runat="server" width="250"></asp:Label></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Utente abilitato ?</td>
                <td class="col1"></td>
                <td class="colin"><asp:CheckBox ID="cbAbilitato" runat="server" /></td>
                <td class="col1"></td>
                <td class="coltext">Potere/profilo utente</td>
                <td class="col1"></td>
                <td class="colin"><asp:DropDownList ID="ddlPotere" runat="server" Width="240"></asp:DropDownList></td>
                <td class="col1"></td>
            </tr>
            <tr>
                <td class="col1"></td>
                <td class="coltext">Convalidare solo dopo questa data</td>
                <td class="col1"></td>
                <td class="colin">
                    <asp:Calendar ID="cldLimite" runat="server" Autopostback="true" SelectedDayStyle-BackColor="Blue"></asp:Calendar>
                </td>
                <td class="col1"></td>
                <td class="coltext"></td>
                <td class="col1"></td>
                <td class="colin"></td>
                <td class="col1"></td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pDettaglio" runat="server" Visible ="true">
        <asp:GridView ID="gvDettaglio" runat="server" Width="1200" AutoGenerateColumns="false" Visible="true"
            DataKeyNames="id" PageSize="999" ShowHeaderWhenEmpty="True" HorizontalAlign="Center"
            style="text-align: left; float: left; " CellPadding="2" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="dgvComm_SelectedIndexChanged" Font-Size="Small" >
       <Columns>
            <asp:CommandField ButtonType="Button" SelectText="Modifica" headertext="Comando" ShowSelectButton="True"><ItemStyle Width="80px" /></asp:CommandField>
            <asp:Boundfield DataField="Titolo" headertext="Titolo" HeaderStyle-HorizontalAlign="left" DataFormatString="{0:C2}"><ItemStyle Width="480px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield>
            <asp:Boundfield DataField="importoasta" headertext="Importo" HeaderStyle-HorizontalAlign="right" DataFormatString="{0:C2}"><ItemStyle Width="110px" HorizontalAlign="right" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="Stat" headertext="Stato" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="80px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="fin.a" headertext="Fin.a" HeaderStyle-HorizontalAlign="center"><ItemStyle Width="40px" HorizontalAlign="center" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="tipologia" headertext="Tipologia" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="100px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="servizio" headertext="Servizio" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="160px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield>  
            <asp:Boundfield DataField="procedura" headertext="Procedura" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="130px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
            <asp:Boundfield DataField="affidamento" headertext="Affidamento" HeaderStyle-HorizontalAlign="left"><ItemStyle Width="130px" HorizontalAlign="left" Wrap="true"/></asp:Boundfield> 
       </Columns>
    </asp:GridView>
    </asp:Panel>
    <div style="clear: both;"></div><p></p>
    <div>
    <div style="margin: 0px auto; text-align: center; float: left;">
        <asp:Button ID="bConferma" runat="server" Text="Conferma cancellazione" Width="149px" OnClick="bConferma_Click" visible="false" />
    </div>
    <div style="margin: 0px auto; text-align: center; float: left;">
        <asp:Button ID="bAnnulla" runat="server" Text="ANNULLA" Width="90px" OnClick="bAnnulla_Click" visible="false" />
    </div>
    </div>
    <div id="idHelp" style="margin: 0px auto; border-color: darkblue; border:2px; border-width: 2px; width: 1200px; padding: 4px; box-sizing:padding-box; border-radius: 4px;"></div>     
</div>

<hr class="hrbianca" style="height:6px;" />
<p class="centra-text clearfix" style="clear:both;">
<asp:TextBox ID="sStato" runat="server" style="height: 28px; border-width: 1px; border-radius: 4px; Border-Color: blue; width: 1200px; box-sizing: border-box;"></asp:TextBox>
</p>
</asp:Content>


