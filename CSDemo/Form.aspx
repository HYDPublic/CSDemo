<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Form.aspx.cs" Inherits="Default2" MasterPageFile="~/Site.Master" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <div class="center">
            <h1>Speedy Touch Type! &trade;</h1>
        </div>
        <p class="lead">
            The <span class="courier">Delete</span> and <span class="courier">Backspace</span> keys have been disabled 
            for this trial. Do you best to type the following text into the below field <span class="bold">without</span>
            looking at your keyboard! You will be scored based on how many words are spelled correctly. There is also a 
            time bonus.
        </p>

        <div class="center">
            <blockquote class="blockquote">
                <h2 class="m-b-0">
                    <span class="courier">
                        <asp:Label runat="server" ID="InputText"></asp:Label>
                    </span>
                </h2>
            </blockquote>
        </div>

        <div class="row center">
            <div class="row">
                <asp:Label runat="server" ID="InputLabel">Input </asp:Label>
                <asp:TextBox runat="server" ID="InputTextBox" CssClass="inline form-control"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Button runat="server" ID="InputSubmitButton" OnClick="InputSubmitButton_Click" text="Submit!" CssClass="button btn btn-success" />
                <asp:Button runat="server" ID="NewSentenceButton" text="New Sentence" CssClass="button btn btn-warning" OnClick="NewSentenceButton_Click" />
                <asp:Button runat="server" ID="ResetButton" text="Reset" OnClick="ResetButton_Click" CssClass="button btn btn-danger" /> 
            </div>
        </div>

        <hr />

        <%--<asp:Label runat="server">Output: </asp:Label>--%>
        <br />
        <asp:Label runat="server" ID="OutputLabel"><h2>Your score will appear here.</h2></asp:Label>

    </div>

    <script type="text/javascript">
        $("#<%=InputTextBox.ClientID %>").keydown(function (e) {
            // Disable backspace (8) and delete (46) keys in input text.
            if (e.keyCode == 8 || e.keyCode == 46) {
                e.preventDefault();
            }
        });
    </script>
</asp:Content>