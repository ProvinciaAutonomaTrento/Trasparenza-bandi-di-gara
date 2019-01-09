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

<%@ Page Title="Trasparenza - dettaglio procedura di gara" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="dettaglio.aspx.cs" Inherits="dettaglio" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
<!-- Global site tag (gtag.js) - Google Analytics -->
<script async src="https://www.googletagmanager.com/gtag/js?id=UA-112036310-1"></script>
<script>
window.dataLayer = window.dataLayer || [];
function gtag(){dataLayer.push(arguments);}
gtag('js', new Date());

gtag('config', 'UA-112036310-1');
</script>
    <p></p><br /><br /><hr />
    <div>
        <asp:Table ID="tdatidettaglio" runat="server" BackColor="White" ClientIDMode="Predictable" ForeColor="Black" Width="1200px" Font-Size="Small" HorizontalAlign="NotSet">
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" Width="340px"  HorizontalAlign="Left"></asp:TableCell>
                <asp:TableCell runat="server" Width="10px" HorizontalAlign="Left"></asp:TableCell>
                <asp:TableCell runat="server" Width="850px" HorizontalAlign="left" Wrap="True"></asp:TableCell>
            </asp:TableRow>                
        </asp:Table>
    </div>
    <p>
    <asp:Table ID="navi" runat="server" HorizontalAlign="Center">
        <asp:TableRow runat="server">
            <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
            <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
            <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
        </asp:TableRow>                
    </asp:Table>
    </p>
    <div class="col-md-4">
        <asp:TextBox ID="tStato" runat="server" Width="1200px" Wrap="True" Font-Size="Small" CssClass="col-md-offset-0" BorderStyle="None"></asp:TextBox>
    </div>
</asp:Content>
