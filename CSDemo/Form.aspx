<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Form.aspx.cs" Inherits="Default2" MasterPageFile="~/Site.Master" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Enter text below</h1>
        <p class="lead">
            By entering text below, you will submit your input to the Microsoft Cognitive Services API that will return your results
            and show you all the cool things it's capable of doing.
        </p>

        <div class="row center">
            <div class="row">
                <asp:Label runat="server" ID="InputLabel">Input: </asp:Label>
                <asp:TextBox runat="server" ID="InputTextBox"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Button runat="server" ID="ResetButton" text="Reset" OnClick="ResetButton_Click" /> 
                <asp:Button runat="server" ID="InputSubmitButton" OnClick="InputSubmitButton_Click" text="Submit!" />
            </div>
        </div>

        <hr />

        <%--<asp:Label runat="server">Output: </asp:Label>--%>
        <br />
        <asp:Label runat="server" ID="OutputLabel"></asp:Label>

    </div>

    <script type="text/javascript">
        $("#<%=InputTextBox.ClientID %>").keydown(function (e) {
            // Disable backspace (8) and delete (46) key presses.
            if (e.keyCode == 8 || e.keyCode == 46) {
                e.preventDefault();
            }
            console.log(e.keyCode);
        });
    </script>
</asp:Content>