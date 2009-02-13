<%@ Page Title="Contact" Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Views/Shared/Site.Master" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/Content/Scripts/JQuery/JQuery.min.js"></script>  
</asp:Content>

<asp:Content ContentPlaceHolderID="NavigationContent" runat="server">
    <h2>Contact</h2>
    <p>
        For questions or comments about this site, please don't hesitate to contact me.
    </p>
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <p>
        The following is an <a href="http://microformats.org/wiki/hcard" class="external">hCard</a> with my contact information. 
        Browser add-ins, such as <a href="https://addons.mozilla.org/en-US/firefox/addon/4106">Operator</a> for Firefox or 
        <a href="http://visitmix.com/Lab/Oomph">Oomph</a> for Internet Explorer, should recognise this card and allow you to save or bookmark 
        my contact details. Alternatively, click here for an <a href="/Content/Attachments/PaulStovell.vcf">Outlook vCard</a>. 
    </p>
    <div class="vcard">
        <img class="photo" src="/Resources/Images/Me.jpg" alt="Paul Stovell" title="Paul Stovell" />
        <div class="vcard-in">
            <h3 class="fn">Paul Stovell</h3>
            <ul>
                <li><label for="email">Email:</label> <a id="email" href="mailto:paul@paulstovell.com" class="email">paul@paulstovell.com</a></li>
                <li><span class="tel">
                    <span class="type noscreen">Cell</span>
                        <label for="tel">Mobile:</label> 
                        <span id="tel" class="value">+61.420.314.127</span>
                    </span></li>
                <li><label for="msnim">MSN:</label> <a href="msnim:chat?contact=paulstovell@hotmail.com">paulstovell@hotmail.com</a></li>
                <li><label for="twit">Twitter:</label> <a id="twit" href="http://twitter.com/paulstovell">paulstovell</a></li>
            </ul>
        </div>
    </div>

</asp:Content>
