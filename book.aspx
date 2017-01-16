<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="book.aspx.cs" Inherits="book" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .books img
        {
            width: 160px;
            height: 240px;
            float: left;
            padding: 1px;
            background-color: Black;
            border: 1px solid white;
            margin-right: 60px;
            margin-top: 40px;
        }
        .books .info
        {
            float: left;
            width: 320px;
            height: 480px;
        }
        .info h3
        {
            color: White;
            text-align: center;
            margin-bottom: 10px;
        }
        .info h4
        {
            font-size: 12px;
            color: #666;
            text-align: center;
            margin-bottom: 10px;
        }
        .info p
        {
            color: #666;
            font-size: 12px;
        }
        .info b
        {
            font-size: 12px;
            color: #556;
            letter-spacing: 2px;
            line-height: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    &nbsp;<asp:ListView ID="ListView1" runat="server" DataSourceID="SQLBOOks" ItemPlaceholderID="hder">
        <LayoutTemplate>
            <div runat="server" id="hder">
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <div runat="server" class="books">
                <img title='<%#Eval("bkName") %>' src='<%#Eval("bkImage","Booksimges/{0}") %>' />
                <div class="info">
                    <h3>
                        【<%#Eval("bkName") %>】
                    </h3>
                    <h4>
                        作者：<%#Eval("bkAuthor") %><br /></h4>
                    <p>
                        类型：<%#Eval("tName") %><br /><br />出版社：<%#Eval("Press") %><br /><br />价格：<%#Eval("bkPrice","{0:C}") %>元</p>
                    <br />
                    <p>
                        库存：<asp:Label ID="Label1" runat="server" Text='<%#Eval("bkQuanity") %>'></asp:Label>
                        | 已借:<asp:Label ID="Label2" runat="server" Text='<%#Eval("bkBorrow") %>'></asp:Label></p>
                    <br />
                    <p>
                        人气:<asp:Label ID="Label3" runat="server" Text='<%#Eval("bkBorrowNum") %>'></asp:Label></p>
                    <br />
                    <p>
                        状态:
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%#Eval("Borrowout") %>' Text="可借"
                            Enabled="false" />
                    </p>
                    <br />
                    <b>详细说明：
                        <%#Eval("bkInfo")%>
                    </b>
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btnBorrow" runat="server" Text="借阅" BackColor="Transparent" ForeColor="lime"
                        Width="60" OnClick="btnBorrow_Click" />
                    <asp:Button ID="btnxg" runat="server" Text="修改资料" OnClick="btnxg_Click" BackColor="Transparent"
                        ForeColor="lime" BorderColor="Transparent" />
                </div>
                <div class="clear">
                </div>
            </div>
        </ItemTemplate>
    </asp:ListView>
    <asp:SqlDataSource ID="SQLBOOks" runat="server" ConnectionString="<%$ ConnectionStrings:BookConnectionString %>"
        SelectCommand="SELECT Books.*,BooksPress.Press,BooksType.* FROM Books inner join BooksPress on Books.B_Press=BooksPress.B_ID inner join BooksType on Books.tID=BooksType.tID WHERE ([bkID] = @bkID)">
        <SelectParameters>
            <asp:QueryStringParameter Name="bkID" QueryStringField="bkid" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
