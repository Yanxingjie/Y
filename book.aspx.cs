using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class book : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void btnBorrow_Click(object sender, EventArgs e)
    {
        if (Session["uName"] == null)
        {
            Response.Write("<script>alert('您好，请登陆！'); location.href='SignIn.aspx';</script>");
        }
        else
        {
            string cxstr = ConfigurationManager.ConnectionStrings["book"].ConnectionString;
            using (SqlConnection cxconn = new SqlConnection(cxstr))
            {
                cxconn.Open();
                string seltext = "select * from Users where uID=@usersID";
                SqlCommand sel = new SqlCommand(seltext, cxconn);
                SqlParameter selpm = new SqlParameter("@usersID", Session["uID"]);
                sel.Parameters.Add(selpm);
                SqlDataReader seldr = sel.ExecuteReader();
                if (seldr.Read())
                {
                    int Bornum = Int32.Parse(seldr["Borrownum"].ToString());
                    cxconn.Close();
                    if (Bornum == 0)
                    {
                        if (Session["Type"].ToString() == "0")
                        {
                            Response.Write("<script>alert('对不起，您最多只能借三本书！'); location.href='Booksystem.aspx';</script>");
                        }
                        else
                        {
                            Response.Write("<script>alert('对不起，您最多只能借五本书！'); location.href='Booksystem.aspx';</script>");
                        }

                    }
                    else
                    {
                        //借书
                        int bkidd = Int32.Parse(Request.QueryString["bkid"].ToString());
                        string ljstrrrr = ConfigurationManager.ConnectionStrings["book"].ConnectionString;
                        using (SqlConnection ljconnnnn = new SqlConnection(ljstrrrr))
                        {
                            ljconnnnn.Open();
                            SqlCommand ljcmddddd = new SqlCommand("upGetBooksbkID", ljconnnnn);
                            ljcmddddd.CommandType = CommandType.StoredProcedure;
                            SqlParameter pm = new SqlParameter("@bkID", bkidd);
                            ljcmddddd.Parameters.Add(pm);
                            SqlDataReader dr = ljcmddddd.ExecuteReader();
                            if (dr.Read())
                            {
                                int bkQuanity = Int32.Parse(dr["bkQuanity"].ToString());
                                ljconnnnn.Close();
                                if (bkQuanity > 0)
                                {
                                    int bkid = Int32.Parse(Request.QueryString["bkid"].ToString());
                                    string B_Time = System.DateTime.Now.ToString();
                                    string B_ReturnTime = DateTime.Now.AddDays(30).ToString();
                                    string B_ReaderName = "";
                                    string yesno = "未还";
                                    string str = ConfigurationManager.ConnectionStrings["book"].ConnectionString;
                                    using (SqlConnection conn = new SqlConnection(str))
                                    {
                                        conn.Open();
                                        SqlCommand cmdd = new SqlCommand("borrow", conn);
                                        cmdd.CommandType = CommandType.StoredProcedure;
                                        SqlParameter[] ps = { new SqlParameter("@bkID", bkid),
                                new SqlParameter("@uuID",Session["uID"]),
                                        new SqlParameter("@btime",B_Time),
                                        new SqlParameter("@brt",B_ReturnTime),
                                        new SqlParameter("@rn",B_ReaderName),
                                            new SqlParameter("@yesno",yesno)};
                                        cmdd.Parameters.AddRange(ps);
                                        if (cmdd.ExecuteNonQuery() > 0)
                                        {
                                            bookinfo();
                                            Bookinfo();
                                            //每借一本书BOOK表的内容随之变化。（库存、已借数、人气）↓
                                        }
                                        else
                                        {
                                            ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('借阅失败');</script>");
                                        }
                                    }
                                }
                                else
                                {
                                    Response.Write("<script>alert('对不起，此图书已经全部借出！'); location.href='Booksystem.aspx';</script>");
                                }

                            }

                        }
                    }//借书
                }//
            }
        }
    }
    protected void btnxg_Click(object sender, EventArgs e)
    {
        int bkid = Int32.Parse(Request.QueryString["bkid"].ToString());
        if (Session["Type"].ToString() != "1")
        {
            ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('对不起，此功能仅供管理员使用！');</Script>");
        }
        else
        {
            Response.Redirect("修改资料.aspx?bkID=" + bkid + "");
        }
    }
    public void bookinfo()
    {
        int uid = Int32.Parse(Session["uID"].ToString());
        string ustr = ConfigurationManager.ConnectionStrings["book"].ConnectionString;
        using (SqlConnection uconn = new SqlConnection(ustr))
        {
            uconn.Open();
            string useltext = "select * from Users where uID=@uid";
            SqlCommand usel = new SqlCommand(useltext, uconn);
            SqlParameter uselpm = new SqlParameter("@uid", uid);
            usel.Parameters.Add(uselpm);
            SqlDataReader useldr = usel.ExecuteReader();
            if (useldr.Read())
            {
                int num = Int32.Parse(useldr["Borrownum"].ToString());
                uconn.Close();
                uconn.Open();
                SqlCommand uscmd = new SqlCommand("update Users set Borrownum=@bnum where uID=@uID", uconn);
                //SqlParameter upmM = new SqlParameter("@uID", uid);
                //uscmd.Parameters.Add(upmM);
                SqlParameter[] sps = { new SqlParameter("@bnum", num - 1),
                                     new SqlParameter("@uID",uid)};
                uscmd.Parameters.AddRange(sps);
                uscmd.ExecuteNonQuery();
            }
        }
    }
    public void Bookinfo()
    {


        //图书列表数据随借书变化
        int bkid = Int32.Parse(Request.QueryString["bkid"].ToString());
        string ljstr = ConfigurationManager.ConnectionStrings["book"].ConnectionString;
        using (SqlConnection ljconn = new SqlConnection(ljstr))
        {
            ljconn.Open();
            SqlCommand ljcmdd = new SqlCommand("upGetBooksbkID", ljconn);
            ljcmdd.CommandType = CommandType.StoredProcedure;
            SqlParameter pm = new SqlParameter("@bkID", bkid);
            ljcmdd.Parameters.Add(pm);
            SqlDataReader dr = ljcmdd.ExecuteReader();
            if (dr.Read())
            {
                int bkQuanity = Int32.Parse(dr["bkQuanity"].ToString());
                int bkBorrow = Int32.Parse(dr["bkBorrow"].ToString());
                int bkBorrowNum = Int32.Parse(dr["bkBorrowNum"].ToString());
                ljconn.Close();
                ljconn.Open();
                SqlCommand ljcmd = new SqlCommand("setbook", ljconn);
                ljcmd.CommandType = CommandType.StoredProcedure;
                SqlParameter pmM = new SqlParameter("@bkID", bkid);
                ljcmd.Parameters.Add(pmM);
                if (bkQuanity > 0)
                {
                    bool bor = true;
                    SqlParameter[] ps = { new SqlParameter("@bkQuanity",bkQuanity-1),
                                new SqlParameter("@bkBorrow",bkBorrow+1),
                                new SqlParameter("@bkBorrowNum",bkBorrowNum+1),
                                        new SqlParameter("@Borrowout",bor)};
                    ljcmd.Parameters.AddRange(ps);
                    if (ljcmd.ExecuteNonQuery() > 0)
                    {
                        Response.Write("<script>alert('借阅成功'); location.href='Booksystem.aspx';</script>");
                    }
                }
                else
                {
                    bool borr = false;
                    SqlParameter[] ps = { new SqlParameter("@Borrowout", borr),
                                            new SqlParameter("@bkQuanity",bkQuanity),
                                            new SqlParameter("@bkBorrow",bkBorrow),
                                            new SqlParameter("@bkBorrowNum",bkBorrowNum)
                                        };
                    ljcmd.Parameters.AddRange(ps);
                    if (ljcmd.ExecuteNonQuery() > 0)
                    {
                        Response.Write("<script>alert('对不起，此图书已经全部借出！'); location.href='Booksystem.aspx';</script>");

                    }
                }

            }
            //ljconn.Open();
            //SqlCommand ljcmd = new SqlCommand("upUpdateBooks", ljconn);
            //ljcmd.CommandType = CommandType.StoredProcedure;
            //SqlParameter[] ps = { new SqlParameter("@bkQuanity",bkQuanity-1),
            //                    new SqlParameter("@bkBorrow",bkBorrow+1),
            //                    new SqlParameter("@bkBorrowNum",bkBorrowNum+1)};
            //ljcmd.Parameters.AddRange(ps);
            //if (ljcmd.ExecuteNonQuery() > 0)
            //{
            //    ClientScript.RegisterStartupScript(GetType(), "", "<script>alert('借阅成功');</script>");
            //}

            //if (dr.Read())
            //{
            //    int bkQuanity = Int32.Parse(dr["bkQuanity"].ToString());
            //    int bkBorrow = Int32.Parse(dr["bkBorrow"].ToString());
            //    int bkBorrowNum = Int32.Parse(dr["bkBorrowNum"].ToString());
            //} 

        }
    }

}