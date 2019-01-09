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

<%@ Page Title="Trasparenza" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Global site tag (gtag.js) - Google Analytics -->
<script async src="https://www.googletagmanager.com/gtag/js?id=UA-112036310-1"></script>
<script>
window.dataLayer = window.dataLayer || [];
function gtag(){dataLayer.push(arguments);}
gtag('js', new Date());

gtag('config', 'UA-112036310-1');
</script>
       <style>
    .marginesx200 {
        position: relative;
        left: 200px;
    }
        .auto-style2 {
            width: 66px;
        }
        .auto-style6 {
            width: 780px;
        }
        .auto-style7 {
            width: 109px;
        }
        .auto-style9 {
            width: 96px;
        }
        .auto-style10 {
            width: 248px;
        }
        .auto-style11 {
            width: 52px;
        }
        .auto-style12 {
            width: 1024px;
        }
        </style>
    <div class="jumbotron">
    </div>

    <div class="row">
        <div class="col-md-4">
            <table class="auto-style12">
                <tr>
                    <td><asp:Button ID="cbGo" runat="server" Text="Esegui ricerca" OnClick="cbGo_Click" Width="160px"/> </td>
                    <td><asp:Button ID="cbreset" runat="server" Text="reset valori" OnClick="cbreset_Click" Width="120px"/></td>
                    <td>&nbsp;</td>
                    <td><asp:Label ID="tRighetrovate" runat="server" Width="832px" BorderStyle="None"></asp:Label></td>
                    <td>
              <asp:Button ID="Login" runat="server" Text="Accedi"  OnClick="Login_Click" Visible="true" Width="60px" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
             <asp:Table ID="tFiltri" runat="server" BackColor="#2E6194" ClientIDMode="Predictable" ForeColor="White" Width="1200px" Font-Size="Small" HorizontalAlign="Left" Height="38px" ToolTip="selezionare la stazione appaltante di interesse dall'elenco">
                <asp:TableRow runat="server" HorizontalAlign="Left">
                    <asp:TableCell runat="server" Width="20px"></asp:TableCell>                    
                    <asp:TableCell runat="server" Width="300px">testo nell'oggetto gara <asp:TextBox ID="toggetto" runat="server" Width="200"></asp:TextBox></asp:TableCell>
                    <asp:TableCell runat="server" Width="10px"></asp:TableCell>
                    <asp:TableCell runat="server" Width="300px">stazione appaltante <asp:DropDownList ID="ddlente" runat="server" Width="250"></asp:DropDownList></asp:TableCell>
                    <asp:TableCell runat="server" Width="10px"></asp:TableCell>
                    <asp:TableCell runat="server" Width="180px">stato procedura <asp:DropDownList ID="ddlstato" runat="server" Width="150" ItemType="text"></asp:DropDownList></asp:TableCell>
                    <asp:TableCell runat="server" Width="10px"></asp:TableCell>                    
                    <asp:TableCell runat="server" Width="100px"><asp:CheckBox ID="cbContenzioso" runat="server" />contenzioso</asp:TableCell>    
                    <asp:TableCell runat="server" Width="10px"></asp:TableCell>
                    <asp:TableCell runat="server" Width="180px">tipologia procedura <asp:DropDownList ID="ddltipo" runat="server" Width="150"></asp:DropDownList></asp:TableCell>
                    <asp:TableCell runat="server" Width="10px"></asp:TableCell>
                </asp:TableRow>                
            </asp:Table>
            <br />
             
            <br />
        </div>
        <div><br />
             <asp:Table ID="titolo" runat="server" BackColor="White" ClientIDMode="Predictable" ForeColor="Black" Width="1200px" Font-Size="Small" HorizontalAlign="NotSet">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" Width="30px"  HorizontalAlign="Left" BorderColor="Black" BorderWidth="1px"></asp:TableCell>
                    <asp:TableCell runat="server" Width="460px" HorizontalAlign="Left"  BorderColor="Black" BorderWidth="1px" Wrap="True">Amministrazione aggiudicatrice/oggetto</asp:TableCell>                    
                    <asp:TableCell runat="server" Width="33px"  HorizontalAlign="center" BorderColor="Black" BorderWidth="1px"></asp:TableCell>                    
                    <asp:TableCell runat="server" Width="105px" HorizontalAlign="center" BorderColor="Black" BorderWidth="1px">stato</asp:TableCell>                    
                    <asp:TableCell runat="server" Width="215px" HorizontalAlign="Left" BorderColor="Black" BorderWidth="1px" Wrap="True">Aggiudicataria</asp:TableCell>  
                    <asp:TableCell runat="server" Width="113px" HorizontalAlign="Right" BorderColor="Black" BorderWidth="1px">Importo</asp:TableCell>
                    <asp:TableCell runat="server" Width="28px" HorizontalAlign="center" BorderColor="Black" BorderWidth="1px"></asp:TableCell>                                        
                    <asp:TableCell runat="server" Width="107px" HorizontalAlign="center" BorderColor="Black" BorderWidth="1px">Tempi stimati</asp:TableCell>
                    <asp:TableCell runat="server" Width="107px" HorizontalAlign="center" BorderColor="Black" BorderWidth="1px">Tempi rideterminati</asp:TableCell>
                </asp:TableRow>                
            </asp:Table>
        </div>  
        <div>
            <asp:Table ID="tdati" runat="server" BackColor="White" ClientIDMode="Predictable" ForeColor="Black" Width="1200px" Font-Size="Small" HorizontalAlign="NotSet">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" Width="30px"  HorizontalAlign="Left" BorderStyle="None"></asp:TableCell>
                    <asp:TableCell runat="server" Width="460px" HorizontalAlign="Left" Wrap="True"></asp:TableCell>
                    <asp:TableCell runat="server" Width="33px" HorizontalAlign="center" BorderStyle="None"></asp:TableCell>
                    <asp:TableCell runat="server" Width="103px" HorizontalAlign="center" Wrap="True"></asp:TableCell>
                    <asp:TableCell runat="server" Width="215px" HorizontalAlign="Left" Wrap="True"></asp:TableCell>                    
                    <asp:TableCell runat="server" Width="113px" HorizontalAlign="Right"></asp:TableCell>
                    <asp:TableCell runat="server" Width="28px" HorizontalAlign="center"></asp:TableCell>                                        
                    <asp:TableCell runat="server" Width="105px" HorizontalAlign="center" VerticalAlign="Middle"></asp:TableCell>                                        
                    <asp:TableCell runat="server" Width="107px" HorizontalAlign="center" VerticalAlign="Middle"></asp:TableCell>                                        
                </asp:TableRow>                
            </asp:Table>
            <br />
            <br />
           
           <asp:Table ID="Tu" runat="server" BackColor="White" ClientIDMode="Predictable" ForeColor="Black" Width="1200px" Font-Size="Small" HorizontalAlign="NotSet">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" Width="20px"  HorizontalAlign="Left"></asp:TableCell>
                    <asp:TableCell runat="server" Width="80px"  HorizontalAlign="Center"></asp:TableCell>
                    <asp:TableCell runat="server" Width="980px"  HorizontalAlign="Left"></asp:TableCell>
                </asp:TableRow>                
            </asp:Table>
        </div>
        <div class="col-md-4">
            <asp:TextBox ID="tStato" runat="server" Width="1200px" Wrap="True" Font-Size="Small" CssClass="col-md-offset-0" BorderStyle="None" Visible="false"></asp:TextBox>
        </div> 
        <br />
        <div style="font-size: small">APAC ha scelto quale criterio per la pubblicazione delle procedure di gara su questa pagina web il seguente: in data 13 febbraio 2017 sono state pubblicate su questa pagina web le procedure di gara per l'acquisizione di lavori,<br />
            servizi e forniture bandite dopo il 19/04/2016 (entrata in vigore del nuovo Codice degli Appalti D.lgs 19 Aprile 2016, n. 50) ma non ancora aggiudicate al momento della pubblicazione.</div>
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" Visible="False" />
    </div>
</asp:Content>
