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

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rappresentazionedinamica.aspx.cs" Inherits="rappresentazionedinamica" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .div1 {
            max-width: 1024px;
            height: 60px;
            border: 1px solid blue;
            margin-left: auto;
            margin-right: auto;
        }
    </style>
    </head>
<body>
    <form id="form1" runat="server">
        <div style="text-align: center; max-width=1024px"><asp:TextBox ID="tTitolo" runat="server" style="font-size: 28px; font-weight: normal; text-align: center" Enabled="false" Width="890px"></asp:TextBox>      
            <br />
            <br />
        </div>
        <div  style="text-align: center; max-width=1024px">
            <asp:Table ID="tablealign" runat="server" style="margin: auto;">
                <asp:TableRow HorizontalAlign="Left" BorderColor="Blue">
                    <asp:TableCell BackColor="#fffff4"><asp:Label ID="lTipo" runat="server" Text="Anno 2017:   " style="text-align: left;"></asp:Label></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell>
                        <asp:RadioButtonList ID="rblTipo" runat="server" RepeatDirection="Horizontal"  OnSelectedIndexChanged="rbltipo_SelectedIndexChanged" AutoPostBack="True">                
                        <asp:ListItem Selected="True">Numero registrazioni per SEDE</asp:ListItem>
                        <asp:ListItem>Numero registrazioni per orario</asp:ListItem>
                        <asp:ListItem>Numero registrazioni per mese</asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow HorizontalAlign="Left" BorderColor="Blue">
                    <asp:TableCell BackColor="#fffff4"><asp:Label ID="lPeriodo" runat="server" Text="Periodo rappresentato:   " style="text-align: left;"></asp:Label></asp:TableCell>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell>
                        <asp:RadioButtonList ID="rblist" runat="server" RepeatDirection="Horizontal"  OnSelectedIndexChanged="rblist_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Selected="True">Oggi</asp:ListItem>
                        <asp:ListItem>Ultima settimana</asp:ListItem>
                        <asp:ListItem>Ultimo mese</asp:ListItem>
                        <asp:ListItem>Da inizio anno</asp:ListItem>
                        <asp:ListItem>Anno precedente</asp:ListItem>
                    </asp:RadioButtonList>
                    </asp:TableCell>
                    <asp:TableCell style="text-align: left;">
                        <asp:Button ID="bEsci" runat="server" OnClick="bExit_Click" Text="Esci" Width="85px" OnClientClick="bExit_Click" />                    
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
        </div>
        <div style="text-align: center; max-width=1024px">
            <asp:Chart ID="Giornaliera" runat="server" Width="1015px" Height="500px" BackColor="CadetBlue" Palette="SemiTransparent" >
                <series>
                    <asp:Series Name="Series1" YValuesPerPoint="2">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea BackColor="Bisque" BackGradientStyle="TopBottom" Name="ChartArea1">
                    </asp:ChartArea>                  
                </chartareas>
            </asp:Chart>
        </div>
        <div style="text-align: center; margin: auto;">
            <asp:TextBox ID="tStato" runat="server" style="text-align: center; margin: auto;" Width="1301px"></asp:TextBox>
        </div>
        <asp:Chart ID="Chart1" runat="server" BackGradientStyle="DiagonalLeft" BackHatchStyle="Cross" >
            <Series>
                <asp:Series ChartType="Doughnut" Name="Series1" YValuesPerPoint="6">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
    </form>
</body>
</html>
