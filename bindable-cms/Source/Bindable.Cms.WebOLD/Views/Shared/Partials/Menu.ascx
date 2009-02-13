<%@ Import Namespace="Bindable.Cms.Web.Application.Extensions"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="Bindable.Cms.Web.Views.Shared.Partials.Menu" %>


<ul>
    <li <%= IsActive("Home") %>><%= Html.ActionLink("Home", "Home", "General") %></li>
    <li <%= IsActive("Architecture") %>><%= Html.WikiLink("Client Architecture", "Architecture", "Home") %></li>
    <li <%= IsActive("Patterns") %>><%= Html.WikiLink("Patterns & Samples", "Patterns", "Home") %></li>
    <li <%= IsActive("Projects") %>><%= Html.ActionLink("Projects", "Projects", "General")%></li>
    <li <%= IsActive("Presentations") %>><%= Html.ActionLink("Presentations", "Presentations", "General") %></li>
    <li <%= IsActive("Blog") %>><%= Html.ActionLink("Blog", "Blog", "General")%></li>
    <li <%= IsActive("Contact") %>><%= Html.ActionLink("Contact", "Contact", "General") %></li>
</ul>