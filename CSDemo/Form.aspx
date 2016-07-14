<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Form.aspx.cs" Inherits="Default2" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Enter text below</h1>
        <p class="lead">
            By entering text below, you will submit your input to the Microsoft Cognitive Services API that will return your results
            and show you all the cool things it's capable of doing.
        </p>
        <asp:Label runat="server" ID="InputLabel">Input: </asp:Label>
        <asp:TextBox runat="server" ID="InputTextBox"></asp:TextBox>
        <asp:Button runat="server" ID="InputSubmitButton" text="Submit!" OnClick="InputSubmitButton_Click" />
    </div>
</asp:Content> 
