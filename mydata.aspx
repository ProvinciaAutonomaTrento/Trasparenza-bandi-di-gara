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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mydata.aspx.cs" Inherits="myapac" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 6px;
        }
        .auto-style3 {
            height: 23px;
        }
        .auto-style4 {
            width: 6px;
            height: 23px;
        }
        .auto-style5 {
            width: 255px;
        }
        .auto-style6 {
            height: 23px;
            width: 255px;
        }
        .auto-style10 {
            width: 254px;
        }
        .auto-style12 {
            height: 23px;
            width: 254px;
        }
        .auto-style13 {
            width: 254px;
            height: 26px;
        }
        .auto-style14 {
            width: 168px;
        }
        .auto-style15 {
            height: 23px;
            width: 168px;
        }
        .auto-style16 {
            height: 26px;
            width: 168px;
        }
    </style>
</head>
<body>
    <div class="lungo"><img class="auto-testata" src="Testata_APACsemplice.gif" alt="Logo per la stampa" width="100%"/></div>
    <form id="formmyapac" runat="server">
    <div style="margin: 0px auto; text-align: center;">
        <asp:HyperLink ID="hlhometop" runat="server" NavigateUrl="~/Default.aspx" Target="_self" style="margin: 0px auto;">Home</asp:HyperLink>
    </div>    

    <div style="margin: 0px auto;">
       <table align="center" border="0" style="border: medium solid #1F5DB2; border-radius: 6px; padding: 10px;">
            <tr>
                <td class="auto-style31"></td>
                <td class="auto-style2"></td>
                <td class="auto-style5"></td>
                <td class="auto-style10"></td>
                <td class="auto-style14"></td>
                <td class="auto-style23"></td>
            </tr>
            <tr>
                <td class="auto-style32"></td>
                <td class="auto-style2"></td>
                <td class="auto-style5">I miei dati</td>
                <td class="auto-style10"></td>
                <td class="auto-style15"></td>
                <td class="auto-style24">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">Username</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tNikname" runat="server" Width="241px"></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style31"></td>
                <td class="auto-style2"></td>
                <td class="auto-style5">Nome</td>
                <td class="auto-style10">
                    <asp:TextBox ID="tNome" runat="server" Width="241px"></asp:TextBox>
                </td>
                <td class="auto-style14"></td>
                <td class="auto-style23"></td>
            </tr>
            <tr>
                <td class="auto-style31">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">
                    Cognome</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tCognome" runat="server" Width="241px"></asp:TextBox></td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">E-mail</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tMail1" runat="server" Width="241px"></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">E-mail (ripeti email)</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tMail2" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td>Ente di appartenenza</td>
                <td><asp:DropDownList ID="ddlEnte" runat="server" Width="300"></asp:DropDownList><asp:TextBox ID="tEnte" runat="server" Width="300" Visible="false"></asp:TextBox></td>
                <td>
                    <asp:CheckBox ID="cbNonPresente" Text="non presente in elenco" runat="server" autopostback="true" OnCheckedChanged="cbNonPresente_CheckedChanged" />
                  </td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td>Indirizzo</td>
                <td>
                    <asp:TextBox ID="tIndirizzo" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">
                    Civico</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tCivico" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">
                    CAP</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tCap" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">Città</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tCitta" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">Telefono</td>
                <td class="auto-style13">
                    <asp:TextBox ID="tTelefono" runat="server" Width="241px" ></asp:TextBox>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>

            <tr>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style6">&nbsp;</td>
                <td class="auto-style12">
                    &nbsp;</td>
                <td class="auto-style15">&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3"></td>
                <td class="auto-style4"></td>
                <td class="auto-style6" colspan="2">Do il consenso al trattamento dei dati personali al solo fine della lavorazione della pratica</td>
                <td class="auto-style12">
                    <asp:CheckBox ID="cbConsenso" runat="server" />
                </td>
                
                <td class="auto-style3"></td>
            </tr>
            <tr>
                <td class="auto-style34"></td>
                <td class="auto-style2"></td>
                <td class="auto-style5">
                    <asp:Button ID="bCambiaPassword" runat="server" Text="Cambia password" text-align="left" Style="text-align: left;" OnClick="bCambiaPassword_Click" UseSubmitBehavior="False"/>
                </td>
                <td class="auto-style10"></td>
                <td class="auto-style16"></td>
                <td class="auto-style26"></td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style13">&nbsp;</td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style33">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td class="auto-style5">
                    &nbsp;</td>
                <td class="auto-style13">
                    <asp:Button ID="cbRegistrati" runat="server" Text="Chiedi registrazione" Align="center" BackColor="#FFB94F" Width="250px" EnableTheming="True" UseSubmitBehavior="False" OnClick="cbRegistrati_Click"/>
                </td>
                <td class="auto-style14">&nbsp;</td>
                <td class="auto-style25">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style31"></td>
                <td class="auto-style2"></td>
                <td class="auto-style5"></td>
                <td class="auto-style10"></td>
                <td class="auto-style14"></td>
                <td class="auto-style23"></td>
            </tr>
        </table>
    
    </div>
    <p style="clear:both;"></p>
    <div style="margin: 0px auto; text-align: center;">
        <asp:Label ID="tStato" runat="server" BorderStyle="None" Width="1200px" Align="Center"></asp:Label>
    </div>
    <div style="margin: 0px auto; text-align: center;">
        <asp:HyperLink ID="hlHome" runat="server" NavigateUrl="~/Default.aspx" Target="_self" style="margin: 0px auto;">Home</asp:HyperLink>
    </div>    
    </form>
</body>
</html>