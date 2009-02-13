String.prototype.nl2br = function() {
    var text = escape(this);
    if (text.indexOf('%0D%0A') > -1) {
        re_nlchar = /%0D%0A/g;
    } else if (text.indexOf('%0A') > -1) {
        re_nlchar = /%0A/g;
    } else if (text.indexOf('%0D') > -1) {
        re_nlchar = /%0D/g;
    }
    else return unescape(text);
    return unescape(text.replace(re_nlchar, '<br />'));
};

String.prototype.trim = function() {
    return this.replace(/^\s+|\s+$/, ''); 
};

String.prototype.chop = function(limit) {
    return (this.length > limit) ? (this.substring(0, limit) + "...") : this;
};

String.prototype.escapeAngles = function() {
    return this.replace(/>/g, '&gt;').replace(/</g, '&lt;');
};

String.prototype.coalesce = function(defaultValue) {
    return this == "" ? defaultValue : this;
};

function enableMarkupInsertion() {
    var buildTip = function(title, tip) { return "<h2>" + title + "</h2><p>" + tip + "</p>"; };
    $(".insert").hover(
        function() {
            var selection = $("#comment").getSelection().text.chop(10).escapeAngles().nl2br();
            var tip = "";
            var tag = $(this).attr("title");
            if (tag == "bold") tip = buildTip("Bold Text", "*" + selection.coalesce("bold text") + "*");
            if (tag == "pre") tip = buildTip("Code Listing", "[Listing]<br />" + selection.coalesce("DoStuff()") + "<br />[/Listing]");
            if (tag == "link") tip = buildTip("Hyperlink", "&lt;" + selection.coalesce("www.foo.com") + "&gt;<br />" + "&lt;" + selection.coalesce("www.foo.com") + "|Description&gt;");
            $("#inserttip").html(tip);
        },
        function() { });
    $(".insert").mousedown(
        function() {
            var selection = $("#comment").getSelection().text;
            var replacement = "";
            var tag = $(this).attr("title");
            if (tag == "bold") replacement = "*" + selection.coalesce("bold text") + "*";
            if (tag == "pre") replacement = "[Listing]\r\n" + selection.coalesce("DoStuff()") + "\r\n[/Listing]";
            if (tag == "link") replacement = "<" + selection.coalesce("www.foo.com|Description of link") + ">";
            $("#comment").replaceSelection(replacement, true);
        });
}

function refreshGravatar() {
    var value = $("#email").attr("value");
    if (value != null && value.length > 0) {
        value = value.toLowerCase().trim();
        var hash = hex_md5(value);
        var source = "http://www.gravatar.com/avatar/" + hash + ".jpg?size=50";
        $("#gravatarpreview").attr("src", source).show();
    } else {
        $("#gravatarpreview").hide();
    }
}

function enableGravatarLookup() {
    $("#email").blur(
        function() {
            refreshGravatar();
        });
}

function scrollToValidationFailure() {
    if ($(".validation-failure:first").size() > 0) {
        $.scrollTo(".validation-failure:first");
    }
}

$(document).ready(
    function() {
        scrollToValidationFailure();
        dp.SyntaxHighlighter.ClipboardSwf = '/flash/clipboard.swf';
        dp.SyntaxHighlighter.HighlightAll(null, false, false);
        enableMarkupInsertion();
        enableGravatarLookup();
        refreshGravatar();
    }
);