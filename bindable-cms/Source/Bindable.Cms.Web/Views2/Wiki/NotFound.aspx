<%@ Page Title="Not Found" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>

<asp:Content ContentPlaceHolderID="NavigationContent" runat="server">
    <h2>Error</h2>
    <p>
        The page requested could not be found. Below is a list of all pages on this site. If you followed a link from an external site, 
        please <%= Html.ActionLink("email me", "Contact", "General") %>.
    </p>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    TODO: List site content here.
</asp:Content>
