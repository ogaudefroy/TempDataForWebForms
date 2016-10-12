<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomPage.aspx.cs" Inherits="TempDataForWebForms.SampleApp.CustomPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SampleApp - WebForms Page</title>
</head>
<body>
    <h1>This is an ASP.Net WebForms page</h1>
    <div>
        <h2>Current TempData values: <%: this.TempData.Count %></h2>
        <ul runat="server" ID="list">
        </ul>
    </div>
    <form id="form1" runat="server">
        <div>
            <asp:Button runat="server" Text="Emit TempData and Redirect to MVC" OnClick="OnClick" />
        </div>
    </form>
</body>
</html>

