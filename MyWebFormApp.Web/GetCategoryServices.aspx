<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="GetCategoryServices.aspx.vb" Inherits="MyWebFormApp.Web.GetCategoryServices" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="d-sm-flex align-items-center justify-content-between mb-4">
            <h1 class="h3 mb-0 text-gray-800">Get Categories</h1>
        </div>

        <div class="col-lg-12">
            <div class="row">
                <!-- Basic Card Example -->
                <div class="col-lg-6 ">
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h6 class="m-0 font-weight-bold text-primary">Get Categories</h6>
                        </div>
                        <div class="card-body">
                            <asp:GridView ID="gvCategories" runat="server"></asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="card shadow mb-4">
                        <asp:Literal ID="ltMessage" runat="server" />
                        <div class="card-header py-3">
                            <h6 class="m-0 font-weight-bold text-primary">Insert Categories</h6>
                        </div>
                        <div class="card-body">
                            <asp:TextBox ID="txtCategoryName" runat="server" class="form-control" placeholder="Enter Category Name" />
                            <br />
                            <asp:Button ID="btnInsert" runat="server" Text="Insert" class="btn btn-primary" OnClick="btnInsert_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
