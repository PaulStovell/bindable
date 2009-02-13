<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PaulStovell.Web.Views.Login.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigationContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    
    <% if (ViewData["Message"] != null) { %>
        <div class="notification-error">
            <%= Html.Encode(ViewData["Message"].ToString())%>
        </div>
    <% } %>
    
    <p>Log in before accessing the administration area: </p>
    <form action="Authenticate?ReturnUrl=<%=HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]) %>" method="post">
        <label for="openidIdentifier">OpenID: </label>
        <input id="openidIdentifier" name="openidIdentifier" size="40" />
        <input type="submit" value="Login" />
    </form>
</asp:Content>
