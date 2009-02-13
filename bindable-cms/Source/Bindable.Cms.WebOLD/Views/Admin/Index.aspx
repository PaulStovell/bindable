<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Bindable.Cms.Web.Views.Login.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="NavigationContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <h1>Members Only Area </h1>
  <p>Congratulations, <b>
    <asp:LoginName ID="LoginName1" runat="server" />
  </b>. You have completed the OpenID login process. </p>
  <p>
    <%=Html.ActionLink("Logout", "logout") %>
  </p> 
</asp:Content>
