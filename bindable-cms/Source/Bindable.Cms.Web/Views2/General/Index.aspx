<%@ Page Title="Home" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
                    
<asp:Content ContentPlaceHolderID="NavigationContent" runat="server">
    <h2>Introduction</h2>
    <p>
        Paul Stovell is an Australian-based software consultant and trainer working for <a href="http://www.readify.net/">Readify</a>. Paul is 
        a <a href="#">Microsoft MVP</a> for <a href="#">Client Application Development</a>, a member of the <a href="#">WPF Disciples group</a>, and 
        a Microsoft Certified Professional Developer. Paul has <a href="#">presented</a> at many user groups and events accross Australia. His 
        favourite subjects revolve around application architecture and <a href="#">Windows Presentation Foundation</a>.
    </p>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <img src="/Resources/Images/Banner.jpg" alt="Brisbane, Australia" title="A picture of grass that I do not have the skills to take" class="thumb" />
    
    <ul class="columns">
      <li class="col1">
        <h2><a href="#">Client Architecture</a></h2>
        <p>
          This wiki serves as a knowledge base for application architecture, especially targetted at applications built on Windows Presentation Foundation. It covers 
          all stages of the architecture process, from vision and scope through to development processes.
        </p>   
      </li>
      <li class="col2">
        <h2><a href="#">Patterns &amp; Samples</a></h2>
        <p>
          This section contains descriptions and downloadable code samples for many common scenarios that can arise during client application development. You are invited to 
          comment on the patterns and share your thoughts. 
        </p>   
      </li>
      <li class="col3">
        <h2><a href="#">Blog</a></h2>
        <p>
          I keep a blog of updates made to this site. This is the place to go to see what has changed recently. You can 
          also subscribe to the <a href="#">RSS feed</a>.
        </p>
      </li>
    </ul>
    <div class="clear" />
    
</asp:Content>
