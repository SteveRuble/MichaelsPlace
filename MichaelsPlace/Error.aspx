<%@ Page Language="C#" AutoEventWireup="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div>
    <h1>Error</h1>
        <<%= Server.GetLastError() == null ? "No error" : Server.GetLastError().Message %>
            <pre>
        <<%= Server.GetLastError() == null ? "No error" : Server.GetLastError().ToString() %>
</pre>

    </div>
</body>
</html>
