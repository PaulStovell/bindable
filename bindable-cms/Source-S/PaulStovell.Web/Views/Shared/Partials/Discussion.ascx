<%@ Control Language="C#" CodeBehind="Discussion.ascx.cs" Inherits="PaulStovell.Web.Views.Shared.Partials.Discussion" %>

<div class="comments">
    <div class="comments-in">
        <h2>Comments</h2>
    
        <div class="comment">
            <div class="comment-info">
                <p>
                    Tuesday, December 02, 2008 2:58 PM
                </p>
            </div>
            <div class="comment-gravatar">
                <img class="thumb" src="http://www.gravatar.com/avatar.php?gravatar_id=4c15db850da7812971b14ab491ced5b2&rating=PG&size=50&default=identicon" alt="Michael Taylor" />
            </div>
            <div class="comment-body">
                <p>
                    I tried using CopySourceAsHtml but it couldn't handle even simple code without messing things 
                    up. It uses a static algorithm such that the order in which you select your code determines what 
                    colors things showed up as. Inline CSS didn't help matters.
                </p>
                <p>
                    I eventually found CodeHtmler (http://www.codeplex.com/CodeHtmler) and really liked what I saw. 
                    It could handle code from any number of languages and properly handle colorizing them. Furthermore 
                    it could be configured to use my existing CSS classes rather than creating new ones of its own. 
                    It was a standalone library so I put a wrapper around it for use in VS and now that is all I 
                    use. 
                </p>
            </div>
            <div class="comment-author">
                <p>
                    <a href="#">Michael Taylor</a>
                </p>
            </div>
        </div>
        
        <div class="comment author">
            <div class="comment-info">
                <p>
                    Wednesday, December 03, 2008 5:58 PM
                </p>
            </div>
            <div class="comment-gravatar">
                <img class="thumb" src="http://www.gravatar.com/avatar/2094bc37f443e885894612df3464afe8.jpg?size=50" alt="Paul Stovell" />
            </div>
            <div class="comment-body">
                <p>
                    Michael,
                </p>
                <p>
                    Not having an osx version keeps me from using Writer. Before I left Windows, I did use Writer and 
                    liked it but...msft no likey osx too much, eh? :-)
                </p>
            </div>
            <div class="comment-author">
                <p>
                    <a href="#">Paul Stovell</a>
                </p>
            </div>
        </div>
        
        
        <div class="comment">
            <div class="comment-info">
                <p>
                    Wednesday, December 03, 2008 9:31 PM
                </p>
            </div>
            <div class="comment-gravatar">
                <img class="thumb" src="http://www.gravatar.com/avatar.php?gravatar_id=4c15db960da7812971b14ab491ced5b2&rating=PG&size=50&default=identicon" alt="Dave" />
            </div>
            <div class="comment-body">
                <p>
                    Hey, how about fixing your site - I'm getting Javascript errors everytime I leave a page - looks 
                    like its probably coming from your ads?
                </p>
                <p>
                    Better yet, how about you use your insider position at microsoft to have them put an option in IE, to 
                    only show debug messages for domains of my choosing?
                </p>
            </div>
            <div class="comment-author">
                <p>
                    Dave
                </p>
            </div>
        </div>
    </div>
</div>

<div class="new-comment">
    <div class="new-comment-in">
        <h2>Leave a comment</h2>
        
        <% using (Html.BeginForm(new { action = "Submit", path = ViewData["WikiPath"] }))
           { %>
            <div>
                <input type="hidden" name="wiki" value='<%= ViewData["Wiki"] %>' />
                <input type="hidden" name="path" value='<%= ViewData["WikiPath"] %>' />
            </div>
            <p><label class="required" for='name'>Name: </label><input id="name" name="name" type="text" class="large" />
                <%= Html.ValidationFailure("AuthorName.Required", "name")%></p>
            <p>
                <img id="gravatarpreview" alt="yourgravitar" style="display: none;" />
                <label for='email'>Email: </label><input id="email" name="email" type="text" class="large" /><br />
                <span>Will be used for your <a target="_blank" tabindex="-1" href="http://www.gravatar.com">gravatar</a>.</span>
            </p>
            <p><label for='url'>URL: </label><input id="url" name="url" type="text" class="large" /></p>
            <p>
                <span>
                You can use the following markup: 
                    <a class="insert" title="bold">Bold</a>
                    <a class="insert" title="pre">Code</a>
                    <a class="insert" title="link">Hyperlink</a>
                    </span>
                <%= Html.ValidationFailure("Comment.Required", "comment")%>
                <div id="inserttip">
                </div>
                <textarea rows="12" cols="80" id="comment" name="comment" class="normal"></textarea>
            </p>
            <p><input type="submit" id="submit" class="submit" value="Submit" /></p>
            <p><span class="notification-wait" id="submitnotification"></span></p>
        <% } %>
    </div>
</div>