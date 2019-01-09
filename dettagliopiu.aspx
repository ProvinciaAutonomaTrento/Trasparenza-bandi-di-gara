<%@ Page Title="Trasparenza - dettaglio procedura di gara" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="dettagliopiu.aspx.cs" Inherits="dettaglio" %>

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
    <div class="row">
        <asp:Table ID="tdatidettaglio" runat="server" BackColor="White" ClientIDMode="Predictable" Font-Size="Small" ForeColor="Black" HorizontalAlign="NotSet" Width="1200px">
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" HorizontalAlign="Left" Width="340px"></asp:TableCell>
                <asp:TableCell runat="server" HorizontalAlign="Left" Width="10px"></asp:TableCell>
                <asp:TableCell runat="server" HorizontalAlign="left" Width="850px" Wrap="True"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <hr />
        <p>
        <asp:Table ID="navi" runat="server" HorizontalAlign="Center">
        <asp:TableRow runat="server">
            <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
            <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
            <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
            <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
        </asp:TableRow>                
        </asp:Table>
        </p>
    </div>
    <div class="row">
        <div class="col-md-4">
            <table style="width: 1024px">
                <tr>
                    <td><asp:Button ID="cbGo" runat="server" Text="Esegui ricerca" OnClick="cbGo_Click" /> </td>
                    <td><asp:Button ID="cbreset" runat="server" Text="reset valori" OnClick="cbreset_Click" /></td>
                    <td>&nbsp;</td>
                    <td><asp:TextBox ID="tRighetrovate" runat="server" Width="1112px" BorderStyle="None"></asp:TextBox></td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <div>
             <asp:Table ID="tFiltri" runat="server" BackColor="#2E6194" ClientIDMode="Predictable" ForeColor="White" Width="808px" Font-Size="Small" HorizontalAlign="Left" Height="38px" ToolTip="selezionare la stazione appaltante di interesse dall'elenco">
                <asp:TableRow runat="server" HorizontalAlign="Left">
                    <asp:TableCell runat="server" Width="20px"></asp:TableCell>                    
                    <asp:TableCell runat="server" Width="160px">testo nell'oggetto gara <asp:TextBox ID="toggetto" runat="server" Width="200"></asp:TextBox></asp:TableCell>
                    <asp:TableCell runat="server" Width="10px"></asp:TableCell>
                    <asp:TableCell runat="server" Width="80px">lotti <asp:DropDownList ID="ddlLotti" runat="server" Width="80"></asp:DropDownList></asp:TableCell>
                    <asp:TableCell runat="server" Width="10px"></asp:TableCell>
                    <asp:TableCell runat="server" Width="170px">stato procedura <asp:DropDownList ID="ddlstato" runat="server" Width="150" ItemType="text"></asp:DropDownList></asp:TableCell>
                    <asp:TableCell runat="server" Width="10px"></asp:TableCell>                    
                    <asp:TableCell runat="server" Width="100px"><asp:CheckBox ID="cbContenzioso" runat="server" />contenzioso</asp:TableCell>    
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
                    <asp:TableCell runat="server" Width="465px" HorizontalAlign="Left"  BorderColor="Black" BorderWidth="1px" Wrap="True">Oggetto lotto</asp:TableCell>                    
                    <asp:TableCell runat="server" Width="30px" HorizontalAlign="Center"  BorderColor="Black" BorderWidth="1px"></asp:TableCell> 
                    <asp:TableCell runat="server" Width="105px" HorizontalAlign="Center" BorderColor="Black" BorderWidth="1px">stato</asp:TableCell>                    
                    <asp:TableCell runat="server" Width="215px" HorizontalAlign="Left"  BorderColor="Black" BorderWidth="1px" Wrap="True">Aggiudicataria</asp:TableCell>  
                    <asp:TableCell runat="server" Width="113px" HorizontalAlign="Right" BorderColor="Black" BorderWidth="1px">Importo</asp:TableCell>
                    <asp:TableCell runat="server" Width="28px" HorizontalAlign="Center" BorderColor="Black" BorderWidth="1px"></asp:TableCell>                                        
                    <asp:TableCell runat="server" Width="107px" HorizontalAlign="center" BorderColor="Black" BorderWidth="1px">Tempi stimati</asp:TableCell>
                    <asp:TableCell runat="server" Width="107px" HorizontalAlign="center" BorderColor="Black" BorderWidth="1px">Tempi rideterminati</asp:TableCell>
                </asp:TableRow>                
            </asp:Table>
        </div>  
        <div>
            <asp:Table ID="tdati" runat="server" BackColor="White" ClientIDMode="Predictable" ForeColor="Black" Width="1200px" Font-Size="Small" HorizontalAlign="NotSet">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" Width="30px"  HorizontalAlign="Left"></asp:TableCell>
                    <asp:TableCell runat="server" Width="460px" HorizontalAlign="center" Wrap="True"></asp:TableCell>
                    <asp:TableCell runat="server" Width="33px" HorizontalAlign="Center"></asp:TableCell> 
                    <asp:TableCell runat="server" Width="103px" HorizontalAlign="Center"></asp:TableCell>
                    <asp:TableCell runat="server" Width="215px" HorizontalAlign="Left" Wrap="True"></asp:TableCell>                    
                    <asp:TableCell runat="server" Width="113px" HorizontalAlign="Right"></asp:TableCell>
                    <asp:TableCell runat="server" Width="28px" HorizontalAlign="Center"></asp:TableCell>                                        
                    <asp:TableCell runat="server" Width="105px" HorizontalAlign="center" VerticalAlign="Middle"></asp:TableCell>                                        
                    <asp:TableCell runat="server" Width="107px" HorizontalAlign="center" VerticalAlign="Middle"></asp:TableCell>                                        
                </asp:TableRow>                
            </asp:Table>
            <br />
            <p>
            <asp:Table ID="navi2" runat="server" HorizontalAlign="Center">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
                    <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
                    <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
                <asp:TableCell runat="server"  HorizontalAlign="Left"></asp:TableCell>
            </asp:TableRow>                
            
            </asp:Table>
            </p>                  
            <asp:Table ID="Tu" runat="server" BackColor="White" ClientIDMode="Predictable" ForeColor="Black" Width="1200px" Font-Size="Small" HorizontalAlign="NotSet">
                <asp:TableRow runat="server">
                    <asp:TableCell runat="server" Width="20px"  HorizontalAlign="Left"></asp:TableCell>
                    <asp:TableCell runat="server" Width="80px"  HorizontalAlign="Center"></asp:TableCell>
                    <asp:TableCell runat="server" Width="980px"  HorizontalAlign="Left"></asp:TableCell>
                </asp:TableRow>                
            </asp:Table>
        </div>
    <div class="col-md-4">
        <asp:TextBox ID="tStato" runat="server" Width="1200px" Wrap="True" Font-Size="Small" CssClass="col-md-offset-0" BorderStyle="None"></asp:TextBox>
    </div>
    </div>
</asp:Content>


