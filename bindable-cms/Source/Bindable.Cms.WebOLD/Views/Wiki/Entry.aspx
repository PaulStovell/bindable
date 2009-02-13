<%@ Page Title="Wiki" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<%@ Import Namespace="Bindable.Cms.Web.Application.Extensions"%>
<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        Page.Title = (string) ViewData.Eval("Entry.Title");
        base.OnInit(e);
    }
</script>
<asp:Content ContentPlaceHolderID="NavigationContent" runat="server">
    <h2>Summary</h2>
    <p>
        <%= ViewData.Eval("Entry.Summary") %>
    </p>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <% if (ViewData.Eval("Entry.Parent") != null) { %>
            <strong>Back to: <%=Html.WikiLink((string)ViewData.Eval("Entry.Parent"), "Architecture", (string)ViewData.Eval("Entry.Parent"))%></strong>
        <% } %>
    </p>
    <h1><%= ViewData.Eval("Entry.Title") %></h1>
    <%= ViewData.Eval("Entry.Html") %>
    <p>
        <% if (ViewData.Eval("Entry.Parent") != null) { %>
            <strong>Back to: <%=Html.WikiLink((string)ViewData.Eval("Entry.Parent"), "Architecture", (string)ViewData.Eval("Entry.Parent"))%></strong>
        <% } %>
    </p>
    
    <% Html.RenderPartial("Partials/Discussion"); %>
    
</asp:Content>
