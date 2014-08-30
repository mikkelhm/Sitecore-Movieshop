<%@ Page Language="c#" CodePage="65001" AutoEventWireup="true" Inherits="MovieShop.Website.layouts.MovieShop.Main" CodeBehind="Main.aspx.cs" %>

<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<!DOCTYPE html>
<html class="no-js">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title></title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <link rel="stylesheet" href="/css/MovieShop/normalize.min.css">
    <link rel="stylesheet" href="/css/MovieShop/main.css">

    <script src="/scripts/MovieShop/vendor/modernizr-2.6.2.min.js"></script>
</head>
<body>
    <form method="post" runat="server" id="mainform">
        <sc:Placeholder runat="server" ID="plhContent" Key="content"/>
    </form>
    <script src="/scripts/MovieShop/main.js"></script>
</body>
</html>
