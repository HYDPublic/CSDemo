<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Form.aspx.cs" Inherits="Default2" MasterPageFile="~/Site.Master" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <div class="center">
            <h1>Speedy Touch Type! &trade;</h1>
        </div>
        <p class="lead">
            By entering text below, you will submit your input to the Microsoft Cognitive Services API that will return your results
            and show you all the cool things it's capable of doing.
        </p>

        <blockquote class="blockquote">
            <p class="m-b-0">
                Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the 
                industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and 
                scrambled it to make a type specimen book.
            </p>
        </blockquote>

        <div class="row center">
            <div class="row">
                <asp:Label runat="server" ID="InputLabel">Input </asp:Label>
                <asp:TextBox runat="server" ID="InputTextBox" CssClass="inline form-control"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Button runat="server" ID="ResetButton" text="Reset" OnClick="ResetButton_Click" CssClass="button btn btn-danger" /> 
                <asp:Button runat="server" ID="InputSubmitButton" OnClick="InputSubmitButton_Click" text="Submit!" CssClass="button btn btn-success" />
            </div>
        </div>

        <hr />

        <%--<asp:Label runat="server">Output: </asp:Label>--%>
        <br />
        <asp:Label runat="server" ID="OutputLabel"></asp:Label>

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