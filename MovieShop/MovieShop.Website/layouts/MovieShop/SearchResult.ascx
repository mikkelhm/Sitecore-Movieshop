<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchResult.ascx.cs" Inherits="MovieShop.Website.layouts.MovieShop.SearchResult" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<asp:listview ID="lvMovies" runat="server" ItemType="MovieShop.BusinessLayer.Search.MovieSearchResultItem">
    <LayoutTemplate>
        <ul><asp:placeholder ID="itemPlaceholder" runat="server"/></ul>
    </LayoutTemplate>
    <ItemTemplate>
        <li>
            <div><a href="<%# Item.Url %>"><%# Item.OriginalTitle %> - <%# Item.Title %> - <%# Item.ItemId %></a></div>
            <div><img src="<%# Item.ImageUrl %>?mw=250"/></div>
        </li>
    </ItemTemplate>
</asp:listview>