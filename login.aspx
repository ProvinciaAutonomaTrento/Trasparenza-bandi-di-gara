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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="registrati" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Trasparenza APAC - Registrazione nuovo utente</title>
    <style type="text/css">
        .adx {
            position: relative;
            top: 0px;
            right: 0px;
            width: 171px;
            height: 78px;
        } 
        .auto-testata {
            max-width: 100%;
            height: 151px;
            z-index: -1;
        }
        .centra { text-align: center; }
        .auto-style3 {
            height: 10px;
            width: 11px;
        }
        .auto-style6 {
            width: 45px;
            height: 10px;
        }
        .coltxt {
            width: 207px;
            height: 10px;
            font-style: normal;
            font-size:medium;
            font : 500;
        }
        .colast {
            width: 20px;
            height: 10px;
        }
        .auto-style11 {
            height: 22px;
            width: 11px;
        }
        .auto-style12 {
            width: 45px;
            height: 22px;
        }
        .auto-style24 {
            height: 10px;
            width: 29px;
        }
        .auto-style26 {
            height: 22px;
            width: 29px;
        }
        .auto-style27 {
            height: 22px;
            width: 444px;
        }
        .auto-style32 {
            height: 10px;
            width: 37px;
        }
        .auto-style34 {
            height: 22px;
            width: 37px;
        }
        .auto-style35 {
            height: 23px;
            width: 37px;
        }
        .auto-style36 {
            width: 45px;
            height: 23px;
        }
        .auto-style38 {
            height: 23px;
            width: 11px;
        }
        .auto-style39 {
            height: 23px;
            width: 29px;
        }
        .auto-style40 {
            width: 37px;
        }
        .auto-style41 {
            width: 45px;
        }
        .auto-style43 {
            width: 11px;
        }
        .auto-style44 {
            width: 29px;
        }
        .auto-style54 {
            height: 10px;
        }
        .auto-style55 {
            width: 37px;
            height: 9px;
        }
        .auto-style56 {
            width: 45px;
            height: 9px;
        }
        .auto-style57 {
            width: 207px;
            height: 9px;
            font-style: normal;
            font-size: medium;
        }
        .auto-style58 {
            height: 9px;
        }
        .auto-style59 {
            width: 11px;
            height: 9px;
        }
        .auto-style60 {
            width: 29px;
            height: 9px;
        }
        .auto-style61 {
            text-align: center;
            height: 10px;
        }
    </style>
</head>
<body>
   <div class="lungo"><img class="auto-testata" src="Testata_APACsemplice.gif" alt="Logo per la stampa" width="100%"/></div>
     <form id="form1" runat="server">
    <div>   
        <table align="center" class="auto-style27" border="0" style="border: medium solid #1F5DB2; border-radius: 6px;">
            <tr>
                <td class="auto-style35"></td>
                <td class="auto-style36"></td>
                <td class="coltxt" colspan="2"></td>
                <td class="auto-style54"></td>
                <td class="auto-style38"></td>
                <td class="auto-style39"></td>
            </tr>
            <tr>
                <td class="auto-style32"></td>
                <td class="auto-style6"></td>
                <td class="coltxt" colspan="2">Accedi</td>
                <td class="auto-style54"></td>
                <td class="auto-style3"></td>
                <td class="auto-style24">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="coltxt" colspan="2">&nbsp;</td>
                <td class="auto-style54">&nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style35"></td>
                <td class="auto-style36"></td>
                <td class="coltxt" colspan="2">Username</td>
                <td class="auto-style54"></td>
                <td class="auto-style38"></td>
                <td class="auto-style39"></td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="coltxt" colspan="2">
                    <asp:TextBox ID="nikname" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td class="auto-style54">                   
                    <asp:Label ID="lAsterisconn" runat="server" CssClass="colast"></asp:Label>                   
                </td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="coltxt" colspan="2">&nbsp;</td>
                <td class="auto-style54">&nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="coltxt" style="width: 82px">Password</td>
                <td ></td>
                <td class="auto-style54">&nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="coltxt" colspan="2">
                    <asp:TextBox ID="password" runat="server" Width="200px" TextMode="Password"></asp:TextBox>
                </td>
                <td class="auto-style54">
                    <asp:Label ID="lAsteriscopwd" runat="server" CssClass="colast"></asp:Label>
                </td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="coltxt" colspan="2">&nbsp;</td>
                <td class="auto-style54">&nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="coltxt" colspan="2">
                    <asp:Button ID="cbAccedi" runat="server" Text="Accedi" font-size="Medium" Align="center" BackColor="#FFB94F" Width="200px" EnableTheming="True" OnClick="cbAccedi_Click" Height="36px"/>
                </td>
                <td class="auto-style54">&nbsp;</td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style55"></td>
                <td class="auto-style56"></td>
                <td class="auto-style57" colspan="2"></td>
                <td class="auto-style58"></td>
                <td class="auto-style59"></td>
                <td class="auto-style60"></td>
            </tr>
            <tr>
                <td class="auto-style34"></td>
                <td class="auto-style12"></td>
                <td colspan="2"> </td>
                <td class="auto-style54"></td>
                <td class="auto-style11"></td>
                <td class="auto-style26"></td>
            </tr>
            <tr>
                <td class="auto-style32"></td>
                <td class="auto-style6"></td>
                <td class="coltxt" colspan="2" style="text-align: center;"><asp:Button ID="cbRegistrati" runat="server" Text="Registrati" Width="200px" OnClick="cbRegistrati_Click" onmouseover="omiServizio(this);" onmouseout="omoServizio(this);" /></td>
                <td class="auto-style61">
                    <a href = "istruzioni.pdf" target="_blank">Istruzioni</a>
                </td>
                <td class="auto-style3"></td>
                <td class="auto-style24"></td>
            </tr>
            <tr>
                <td class="auto-style40">&nbsp;</td>
                <td class="auto-style41">&nbsp;</td>
                <td class="coltxt" colspan="2" style="text-align: center;"><asp:Button ID="cbpwddimenticata" runat="server" Text="Password dimenticata" Width="200px" align="center" OnClick="cbpwddimenticata_Click1"/></td>
                <td class="auto-style54"></td>
                <td class="auto-style43">&nbsp;</td>
                <td class="auto-style44">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style35"></td>
                <td class="auto-style36"></td>
                <td class="coltxt" colspan="2"></td>
                <td class="auto-style54"></td>
                <td class="auto-style38"></td>
                <td class="auto-style39"></td>
            </tr>
        </table>    
    </div>
    <br />
    <br />
    <div style="margin: 0px auto; text-align: center;">
        <asp:TextBox ID="tStato" runat="server" BorderStyle="None" Width="1200px" Align="center" style="text-align:center;"></asp:TextBox>
    </div>
    <div style="margin: 0px auto; text-align: center;">
    <div id="idHelp" style="margin: 0px auto; border-color: darkblue; border:2px; border-width: 2px; width: 1200px; height: 86px; padding: 4px; box-sizing:padding-box; border-radius: 4px;"></div>     

    <script type="text/javascript">
        function omiServizio(x) {
            var s = document.getElementById("idHelp");
            s.style.backgroundColor = "white";
            switch (x.id)
            {
                //case "ctl00_MainContent_ddlServizio": s.innerHTML = "Consulenza: si chiede la consulenza, la gara sarà bandita dal richiedente; Stazione appaltante: la gara sarà bandita da APAC per conto dell'ente rihiedente."; break;
                case "cbRegistrati": s.innerHTML = "ATTENZIONE: non possono registrarsi all'applicativo predisposto per la raccolta dei fabbisogni delle amministrazioni aggiudicatrici del Trentino persone fisiche o giuridiche diverse."; s.style.backgroundColor = "lightblue"; break;
            }           
        }
        function omoServizio(x) {
            var s = document.getElementById("idHelp");
            s.innerHTML = ""; s.style.backgroundColor = "white";
        }
    </script>
    </div>
    </form>
    <br />
    <div style="margin: 0px auto; text-align: center;">
        <asp:HyperLink ID="hlHome" runat="server" NavigateUrl="~/Default.aspx" Target="_self" style="margin: 0px auto;">Home</asp:HyperLink>
    </div>  
</body>
</html>